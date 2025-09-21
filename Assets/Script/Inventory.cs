using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Other scripts references")]
    [SerializeField] private Equipment equipment;
    [SerializeField] private ItemsActionSystem itemsActionSystem;
    [SerializeField] private CraftingSystem craftingSystem;

    [Header("Inventory system variables")]

    [SerializeField]
    private List<ItemData> content = new List<ItemData>();

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
        content.Add(item);
        RefreshContent();
    }
    public void RemoveItem(ItemData item)
    {
        content.Remove(item);
        RefreshContent();
    }
    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        isOpen = true;
    }
    public List<ItemData> GetContent()
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
        }
        for (int i = 0; i < content.Count; i++)
        {
            Slot slot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            slot.item = content[i];
            slot.itemVisual.sprite = content[i].visual;
        }
        equipment.UpdateEquipmentsDesequipButtons();
        craftingSystem.UpdateDisplayRecipes();

    }
    public bool IsFull()
    {
        return content.Count == InventorySize;
    }

 
}
