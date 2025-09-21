using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Equipment : MonoBehaviour
{
    [Header("Other scripts references")]
    [SerializeField] private ItemsActionSystem itemsActionSystem;

    [Header("Equipment System Variables")]
    [SerializeField] private Button headSlotDesequipButton;
    [SerializeField] private Button chestSlotDesequipButton;
    [SerializeField] private Button legsSlotDesequipButton;
    [SerializeField] private Button feetSlotDesequipButton;
    [SerializeField] private Button handsSlotDesequipButton;

    [SerializeField] private EquipmentLibrary equipmentLibrary;
    [SerializeField] Image HeadSlotImage;
    [SerializeField] Image ChestSlotImage;
    [SerializeField] Image LegsSlotImage;
    [SerializeField] Image FeetSlotImage;
    [SerializeField] Image HandsSlotImage;
    private ItemData equipHead;
    private ItemData equipChest;
    private ItemData equipLegs;
    private ItemData equipFeet;
    private ItemData equipHands;
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
        }
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(Element => Element.itemData == curentlyEquippedItem).First();
        if (equipmentLibraryItem != null)
        {
            equipmentLibraryItem.itemPrefab.SetActive(false);
            for (int i = 0; i < equipmentLibraryItem.elementsToDisable.Length; i++)
            {
                equipmentLibraryItem.elementsToDisable[i].SetActive(true);
            }
            Inventory.instance.AddItem(curentlyEquippedItem);
            Inventory.instance.RefreshContent();
        }
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
    }
        public void EquipAction()
    {
        print("Equipping " + itemsActionSystem.selectedItem.name);
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(Element => Element.itemData == itemsActionSystem.selectedItem).First();
        if (equipmentLibraryItem != null)
        {

            switch (itemsActionSystem.selectedItem.equipmentType)
            {
                case EquipmentType.Head:
                DisablePreviousEquipedEquipment(equipHead);
                    HeadSlotImage.sprite = itemsActionSystem.selectedItem.visual;
                    equipHead = itemsActionSystem.selectedItem;
                    break;
                case EquipmentType.Chest:
                    DisablePreviousEquipedEquipment(equipChest);
                    ChestSlotImage.sprite = itemsActionSystem.selectedItem.visual;
                    equipChest = itemsActionSystem.selectedItem;
                    break;
                case EquipmentType.Legs:
                    DisablePreviousEquipedEquipment(equipLegs);
                    LegsSlotImage.sprite = itemsActionSystem.selectedItem.visual;
                    equipLegs = itemsActionSystem.selectedItem;
                    break;
                case EquipmentType.Feet:
                    DisablePreviousEquipedEquipment(equipFeet);
                    FeetSlotImage.sprite = itemsActionSystem.selectedItem.visual;
                    equipFeet = itemsActionSystem.selectedItem;
                    break;
                case EquipmentType.Hands:
                    DisablePreviousEquipedEquipment(equipHands);
                    HandsSlotImage.sprite = itemsActionSystem.selectedItem.visual;
                    equipHands = itemsActionSystem.selectedItem;
                    break;
            } 
            equipmentLibraryItem.itemPrefab.SetActive(true);
            for (int i = 0; i < equipmentLibraryItem.elementsToDisable.Length; i++)
            {
                equipmentLibraryItem.elementsToDisable[i].SetActive(false);
            }


            Inventory.instance.RemoveItem(itemsActionSystem.selectedItem);


        }
        else
        {
            Debug.LogError("No prefab found for this item in the Equipment Library: " + itemsActionSystem.selectedItem.name);
        }
        itemsActionSystem.CloseActionPanel();
    }


}
