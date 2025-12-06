using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;
    public Equipment equipment;
    public PlayerStats playerStats;
    public BuildSystem buildSystem;

    void Start()
    {
        if (MainMenu.loadSavedData)
        {
            LoadData();
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveData();
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadData();
        }
        HasSaveChanged();
    }
    public void SaveData()
    {
        SaveData data = new SaveData
        {
            playerPosition = playerTransform.position,
            inventoryContent = Inventory.instance.GetContent(),
            equipHead = equipment.equipHead,
            equipChest = equipment.equipChest,
            equipLegs = equipment.equipLegs,
            equipFeet = equipment.equipFeet,
            equipHands = equipment.equipHands,
            equipWeapon = equipment.equipWeapon,
            currentHealth = playerStats.currentHealth,
            currenthunger = playerStats.currentHunger,
            currentThirst = playerStats.currentThirst,
            placedStructures = buildSystem.placedStructures.ToArray()

        };
        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + "/savedData.json";
        Debug.Log("Data saved to: " + path);
        System.IO.File.WriteAllText(path, json);
    }
    public bool HasSaveChanged()
{
    string path = Application.persistentDataPath + "/savedData.json";

    if (!System.IO.File.Exists(path))
        return true; // Pas de fichier = forcément différent

    // Lire le fichier existant
    string savedJson = System.IO.File.ReadAllText(path);

    // Générer un SaveData actuel (SANS sauvegarder)
    SaveData currentData = new SaveData
    {
        playerPosition = playerTransform.position,
        inventoryContent = Inventory.instance.GetContent(),
        equipHead = equipment.equipHead,
        equipChest = equipment.equipChest,
        equipLegs = equipment.equipLegs,
        equipFeet = equipment.equipFeet,
        equipHands = equipment.equipHands,
        equipWeapon = equipment.equipWeapon,
        currentHealth = playerStats.currentHealth,
        currenthunger = playerStats.currentHunger,
        currentThirst = playerStats.currentThirst,
        placedStructures = buildSystem.placedStructures.ToArray()
    };

    // JSON propre du state actuel
    string currentJson = JsonUtility.ToJson(currentData);

    // Comparaison directe
    return currentJson != savedJson;
}

    void LoadData()
    {
        if (!System.IO.File.Exists(Application.persistentDataPath + "/savedData.json"))
        {
            Debug.LogWarning("No save file found!");
            return;
        }
        string path = Application.persistentDataPath + "/savedData.json";
        string json = System.IO.File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        if(!MainMenu.respawnAuto)
        {
            playerTransform.position = data.playerPosition;
        }
        equipment.LoadEquipments(new ItemData[]{
            data.equipHead,
            data.equipChest,
            data.equipLegs,
            data.equipFeet,
            data.equipHands,
            data.equipWeapon
            });
        playerStats.currentHealth = data.currentHealth;
        buildSystem.LoadStructures(data.placedStructures);
        playerStats.currentHunger = data.currenthunger;
        playerStats.currentThirst = data.currentThirst;
        playerStats.UpdateHealthBarFill();
        Inventory.instance.SetContent(data.inventoryContent);
    }

}
public class SaveData
{
    public Vector3 playerPosition;
    public List<ItemInInventory> inventoryContent;
    public ItemData equipHead;
    public ItemData equipChest;
    public ItemData equipLegs;
    public ItemData equipFeet;
    public ItemData equipHands;
    public ItemData equipWeapon;
    public float currentHealth;
    public float currenthunger;
    public float currentThirst;
    public PlacedStucture[] placedStructures;
}
