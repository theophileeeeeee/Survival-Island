using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [Header("Other scripts references")]
    [SerializeField] private Equipment equipment;
    [SerializeField] private ItemsActionSystem itemsActionSystem;
    [SerializeField] private CraftingSystem craftingSystem;

    [Header("Inventory system variables")]

    [SerializeField]
    private List<ItemInInventory> content = new List<ItemInInventory>();

    [SerializeField]
    private GameObject inventoryPanel;
    [SerializeField]
    private Transform inventorySlotParent;

    public Sprite transparent;
    private bool isOpen = false;
    const int InventorySize = 24;
    public static Inventory instance;



    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        RefreshContent();
        CloseInventory();
    }
    public void AddItem(ItemData item)
    {
        ItemInInventory itemInInventory = content.Where(elem => elem.itemData == item).FirstOrDefault();
        if (itemInInventory != null && item.stackable)
        {
            itemInInventory.count++;
        }
        else
        {
            content.Add
            (new ItemInInventory
            {
                itemData = item,
                count = 1
            }
            );
        }

        RefreshContent();
    }
    public void RemoveItem(ItemData item, int count=1)
    {
        ItemInInventory itemInInventory = content.Where(elem => elem.itemData == item).FirstOrDefault();
        if (itemInInventory.count > count && item.stackable)
        {
            itemInInventory.count-=count;

        }
        else
        {
            content.Remove(itemInInventory);
        }
        RefreshContent();
    }
    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        isOpen = true;
    }
    public List<ItemInInventory> GetContent()
    {
        return content;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (isOpen)
            {
                CloseInventory();
                isOpen = false;
            }
            else
            {
                OpenInventory();
                isOpen = true;
            }
        }
    }
    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        itemsActionSystem.actionPanel.SetActive(false);
        ToolTipSystem.instance.Hide();
        isOpen = false;
    }
    public void RefreshContent()
    {
        for (int i = 0; i < inventorySlotParent.childCount; i++)
        {
            Slot slot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            slot.item = null;
            slot.itemVisual.sprite = transparent;
            slot.countText.enabled = false;
        }
        for (int i = 0; i < content.Count; i++)
        {
            Slot slot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            slot.item = content[i].itemData;
            slot.itemVisual.sprite = content[i].itemData.visual;
            if (slot.item.stackable)
            {
                slot.countText.enabled = true;
                slot.countText.text =content[i].count.ToString();
            }
        }
        equipment.UpdateEquipmentsDesequipButtons();
        craftingSystem.UpdateDisplayRecipes();

    }
    public bool IsFull()
    {
        return content.Count == InventorySize;
    }



}

[System.Serializable]
public class ItemInInventory
{
    public ItemData itemData;
    public int count;
}
