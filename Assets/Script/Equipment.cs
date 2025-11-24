using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Equipment : MonoBehaviour
{
    [Header("Other scripts references")]
    [SerializeField] private ItemsActionSystem itemsActionSystem;
    [SerializeField] private PlayerStats playerStats;

    [Header("Equipment System Variables")]
    [SerializeField] private Button headSlotDesequipButton;
    [SerializeField] private Button chestSlotDesequipButton;
    [SerializeField] private Button legsSlotDesequipButton;
    [SerializeField] private Button feetSlotDesequipButton;
    [SerializeField] private Button handsSlotDesequipButton;
    [SerializeField] private Button weaponSlotDesequipButton;

    [SerializeField] private EquipmentLibrary equipmentLibrary;
    [SerializeField] Image HeadSlotImage;
    [SerializeField] Image ChestSlotImage;
    [SerializeField] Image LegsSlotImage;
    [SerializeField] Image FeetSlotImage;
    [SerializeField] Image WeaponSlotImage;
    [SerializeField] Image HandsSlotImage;
    [HideInInspector]
    public ItemData equipHead;
    [HideInInspector]
    public ItemData equipChest;
    [HideInInspector]
    public ItemData equipLegs;
    [HideInInspector]
    public ItemData equipFeet;
    [HideInInspector]
    public ItemData equipHands;
    [HideInInspector]
    public ItemData equipWeapon;

    [SerializeField] private AudioClip equipSound;
    [SerializeField] private AudioSource audioSource;
    private void DisablePreviousEquipedEquipment(ItemData itemToDisable)
    {
        if (itemToDisable == null) return;
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(Element => Element.itemData == itemToDisable).First();
        if (equipmentLibraryItem != null)
        {
            equipmentLibraryItem.itemPrefab.SetActive(false);
            for (int i = 0; i < equipmentLibraryItem.elementsToDisable.Length; i++)
            {
                equipmentLibraryItem.elementsToDisable[i].SetActive(true);
            }
        }
        playerStats.currentArmorPoints -= itemToDisable.armorPoints;
        Inventory.instance.AddItem(itemToDisable);
    }
    public void DesequipEquipment(EquipmentType equipmentType)
    {
        if (Inventory.instance.IsFull())
        {
            Debug.Log("Inventory is full, cannot desequip item.");
            return;
        }
        ItemData curentlyEquippedItem = null;
        switch (equipmentType)
        {
            case EquipmentType.Head:
                curentlyEquippedItem = equipHead;
                equipHead = null;
                HeadSlotImage.sprite = Inventory.instance.transparent;
                break;
            case EquipmentType.Chest:
                curentlyEquippedItem = equipChest;
                equipChest = null;
                ChestSlotImage.sprite = Inventory.instance.transparent;
                break;
            case EquipmentType.Legs:
                curentlyEquippedItem = equipLegs;
                equipLegs = null;
                LegsSlotImage.sprite = Inventory.instance.transparent;
                break;
            case EquipmentType.Feet:
                curentlyEquippedItem = equipFeet;
                equipFeet = null;
                FeetSlotImage.sprite = Inventory.instance.transparent;
                break;
            case EquipmentType.Hands:
                curentlyEquippedItem = equipHands;
                equipHands = null;
                HandsSlotImage.sprite = Inventory.instance.transparent;
                break;
            case EquipmentType.Weapon:
                curentlyEquippedItem = equipWeapon;
                equipWeapon = null;
                WeaponSlotImage.sprite = Inventory.instance.transparent;
                break;
        }
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(Element => Element.itemData == curentlyEquippedItem).FirstOrDefault();
        if (equipmentLibraryItem != null)
        {
            equipmentLibraryItem.itemPrefab.SetActive(false);
            for (int i = 0; i < equipmentLibraryItem.elementsToDisable.Length; i++)
            {
                equipmentLibraryItem.elementsToDisable[i].SetActive(true);
            }
        }
        if (curentlyEquippedItem != null)
        {
            playerStats.currentArmorPoints -= curentlyEquippedItem.armorPoints;
            Inventory.instance.AddItem(curentlyEquippedItem);
        }
        Inventory.instance.RefreshContent();
    }
    public void UpdateEquipmentsDesequipButtons()
    {
        if (headSlotDesequipButton != null)
        {
            headSlotDesequipButton.onClick.RemoveAllListeners();
            headSlotDesequipButton.onClick.AddListener(() => DesequipEquipment(EquipmentType.Head));
            headSlotDesequipButton.gameObject.SetActive(equipHead != null);
        }

        if (chestSlotDesequipButton != null)
        {
            chestSlotDesequipButton.onClick.RemoveAllListeners();
            chestSlotDesequipButton.onClick.AddListener(() => DesequipEquipment(EquipmentType.Chest));
            chestSlotDesequipButton.gameObject.SetActive(equipChest != null);
        }

        if (legsSlotDesequipButton != null)
        {
            legsSlotDesequipButton.onClick.RemoveAllListeners();
            legsSlotDesequipButton.onClick.AddListener(() => DesequipEquipment(EquipmentType.Legs));
            legsSlotDesequipButton.gameObject.SetActive(equipLegs != null);
        }

        if (feetSlotDesequipButton != null)
        {
            feetSlotDesequipButton.onClick.RemoveAllListeners();
            feetSlotDesequipButton.onClick.AddListener(() => DesequipEquipment(EquipmentType.Feet));
            feetSlotDesequipButton.gameObject.SetActive(equipFeet != null);
        }

        if (handsSlotDesequipButton != null)
        {
            handsSlotDesequipButton.onClick.RemoveAllListeners();
            handsSlotDesequipButton.onClick.AddListener(() => DesequipEquipment(EquipmentType.Hands));
            handsSlotDesequipButton.gameObject.SetActive(equipHands != null);
        }
        if (weaponSlotDesequipButton != null)
        {
            weaponSlotDesequipButton.onClick.RemoveAllListeners();
            weaponSlotDesequipButton.onClick.AddListener(() => DesequipEquipment(EquipmentType.Weapon));
            weaponSlotDesequipButton.gameObject.SetActive(equipWeapon != null);
        }
    }
        public void EquipAction(ItemData equipment=null)
    {
        ItemData itemToEquip = equipment ? equipment : itemsActionSystem.selectedItem;
        print("Equipping " + itemToEquip.name);
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(Element => Element.itemData == itemToEquip).First();
        if (equipmentLibraryItem != null)
        {

            switch (itemToEquip.equipmentType)
            {
                case EquipmentType.Head:
                DisablePreviousEquipedEquipment(equipHead);
                    HeadSlotImage.sprite = itemToEquip.visual;
                    equipHead = itemToEquip;
                    break;
                case EquipmentType.Chest:
                    DisablePreviousEquipedEquipment(equipChest);
                    ChestSlotImage.sprite = itemToEquip.visual;
                    equipChest = itemToEquip;
                    break;
                case EquipmentType.Legs:
                    DisablePreviousEquipedEquipment(equipLegs);
                    LegsSlotImage.sprite = itemToEquip.visual;
                    equipLegs = itemToEquip;
                    break;
                case EquipmentType.Feet:
                    DisablePreviousEquipedEquipment(equipFeet);
                    FeetSlotImage.sprite = itemToEquip.visual;
                    equipFeet = itemToEquip;
                    break;
                case EquipmentType.Hands:
                    DisablePreviousEquipedEquipment(equipHands);
                    HandsSlotImage.sprite = itemToEquip.visual;
                    equipHands = itemToEquip;
                    break;
                case EquipmentType.Weapon:
                    DisablePreviousEquipedEquipment(equipWeapon);
                    WeaponSlotImage.sprite = itemToEquip.visual;
                    equipWeapon =  itemToEquip;
                    break;
            } 
            equipmentLibraryItem.itemPrefab.SetActive(true);
            for (int i = 0; i < equipmentLibraryItem.elementsToDisable.Length; i++)
            {
                equipmentLibraryItem.elementsToDisable[i].SetActive(false);
            }

            playerStats.currentArmorPoints += itemToEquip.armorPoints;
            Inventory.instance.RemoveItem(itemToEquip);
            audioSource.PlayOneShot(equipSound);


        }
        else
        {
            Debug.LogError("No prefab found for this item in the Equipment Library: " + itemToEquip.name);
        }
        itemsActionSystem.CloseActionPanel();
    }
    public void LoadEquipments(ItemData[] savedEquipments)
    {
        Inventory.instance.ClearInventory();
        foreach(EquipmentType equipmentType in System.Enum.GetValues(typeof(EquipmentType)))
        {
            DesequipEquipment(equipmentType);
        }
        foreach(ItemData itemData in savedEquipments)
        {
            if(itemData != null)
            {
                EquipAction(itemData);
            }
        }
    }

}
