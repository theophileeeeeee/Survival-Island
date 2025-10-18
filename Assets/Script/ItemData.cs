using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "Item/NewItem")]
public class ItemData : ScriptableObject
{
    [Header("Data")]
    public string name;
    public Sprite visual;
    public GameObject prefab;
    public string description;
    public bool stackable;
    public int maxStack;
    [Header("Effects")]
    public float healthEffect;
    public float hungerEffect;
    public float thirstEffect;
    [Header("Equipment Stats")]
    public float armorPoints;
    
    [Header("Types")]
    public ItemType type;
    public EquipmentType equipmentType;
    }
public enum ItemType
{
    Ressource,
    Equipment,
    Consumable
}
public enum EquipmentType
{
    Head,
    Chest,
    Legs,
    Feet,
    Hands
}