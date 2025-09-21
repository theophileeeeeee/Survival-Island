using UnityEngine;

public class ItemsActionSystem : MonoBehaviour
{
    [Header("Other scripts references")]
     [SerializeField] private Equipment equipment;
    [Header("Items Action System Variables")]

    [HideInInspector] public ItemData selectedItem;
   
    public GameObject actionPanel;
    [SerializeField] private GameObject useItemButton;
    [SerializeField] private GameObject dropItemButton;
    [SerializeField] private GameObject equipItemButton;
    [SerializeField] private GameObject destroyItemButton;
    [SerializeField] private GameObject DropPoint;
        public void OpenActionPanel(ItemData item, Vector3 slotPosition)
    {
        selectedItem = item;
        if (item == null)
        {
            actionPanel.SetActive(false);
            return;
        }
        switch (item.type)
        {
            case ItemType.Ressource:
                useItemButton.SetActive(false);
                equipItemButton.SetActive(false);
                break;
            case ItemType.Equipment:
                useItemButton.SetActive(false);
                equipItemButton.SetActive(true);
                break;
            case ItemType.Consumable:
                useItemButton.SetActive(true);
                equipItemButton.SetActive(false);
                break;
        }
        actionPanel.transform.position = slotPosition;
        actionPanel.SetActive(true);

    }
    public void CloseActionPanel()
    {
        actionPanel.SetActive(false);
        selectedItem = null;
    }
    public void UseActionButton()
    {
        print("Using " + selectedItem.name);
        CloseActionPanel();
    }
    public void EquipActionButton()
    {
        equipment.EquipAction();
    }

    public void DropActionButton()
    {
        GameObject instantiatedItem = Instantiate(selectedItem.prefab);
        instantiatedItem.transform.position = DropPoint.transform.position;
        Inventory.instance.RemoveItem(selectedItem);
        Inventory.instance.RefreshContent();
        CloseActionPanel();
    }
    public void DestroyActionButton()
    {
        Inventory.instance.RemoveItem(selectedItem);
        Inventory.instance.RefreshContent();
        CloseActionPanel();
    }
}
