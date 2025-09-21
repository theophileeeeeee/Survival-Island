using UnityEngine;

public class Harvestable : MonoBehaviour
{
    [SerializeField] public Ressource[] harvestableItems;
    public bool disableKinematicsOnHarvest;
    public float destroyDelay;
    [Header("Options")]
    public Tool Tool;
}

[System.Serializable]
public class Ressource
{
    public ItemData Item;
    [Range(0, 100)]
    public int dropChance;
}
public enum Tool
{
    Axe,
    Pickaxe
}
