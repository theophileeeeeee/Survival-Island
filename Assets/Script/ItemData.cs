using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "Item/NewItem")]
public class ItemData : ScriptableObject
{
    public string name;
    public Sprite visual;
    public GameObject prefab;
    public string description;
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