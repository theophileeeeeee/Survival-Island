using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class Recipe : MonoBehaviour
{
    public Color missingColor;
    public Color availableColor;
    private RecipeData currentRecipe;
    [SerializeField] private Image CraftableItemImage;
    [SerializeField] private GameObject requiredItemPrefab;
    [SerializeField] private Transform requiredItemsParent;


    [SerializeField] private Button craftButton;
    [SerializeField] private Sprite canBuildSprite;
    [SerializeField] private Sprite cannotBuildSprite;
 
    


    public void Configure(RecipeData recipeData)
    {

        currentRecipe = recipeData;
        CraftableItemImage.sprite = recipeData.craftableItem.visual;
        CraftableItemImage.transform.parent.GetComponent<Slot>().item = recipeData.craftableItem;

        bool canCraft = true;
        for (int i = 0; i < recipeData.requiredItems.Length; i++)
        { 
            GameObject reqItemObj = Instantiate(requiredItemPrefab, requiredItemsParent);
            Image requiredItemImage = reqItemObj.GetComponent<Image>();
            ItemData reqItem = recipeData.requiredItems[i].itemData;
            ElementsRequired elementsRequired = reqItemObj.GetComponent<ElementsRequired>();


            reqItemObj.GetComponent<Slot>().item = reqItem;
            ItemInInventory itemInInventoryCopy = Inventory.instance.GetContent().Where(elem => elem.itemData == reqItem).FirstOrDefault();
             if (itemInInventoryCopy!=null && itemInInventoryCopy.count >= recipeData.requiredItems[i].count)
            {
                requiredItemImage.color = availableColor;
            }
            else
            {
                canCraft = false;
                requiredItemImage.color = missingColor;
            }
            // configure le visuel de l'élément requis
            elementsRequired.elementImage.sprite = recipeData.requiredItems[i].itemData.visual;
            elementsRequired.elementCountText.text = recipeData.requiredItems[i].count.ToString();
        }

        craftButton.image.sprite = canCraft ? canBuildSprite : cannotBuildSprite;
        craftButton.enabled = canCraft;
        ResizeElementsRequiredParents();
    }
    private void ResizeElementsRequiredParents()
    {
        Canvas.ForceUpdateCanvases();
        requiredItemsParent.GetComponent<ContentSizeFitter>().enabled = false;
        requiredItemsParent.GetComponent<ContentSizeFitter>().enabled = true;
    }
    public void CraftItem()
    {
        for (int i = 0; i < currentRecipe.requiredItems.Length; i++)
        {
            Inventory.instance.RemoveItem(currentRecipe.requiredItems[i].itemData, currentRecipe.requiredItems[i].count);
        }
        Inventory.instance.AddItem(currentRecipe.craftableItem);
    }
}
