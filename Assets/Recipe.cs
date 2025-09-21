using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
        List<ItemData> inventoryCopy = new List<ItemData>(Inventory.instance.GetContent());
        for (int i = 0; i < recipeData.requiredItems.Length; i++)
        {
            ItemData reqItem = recipeData.requiredItems[i];
            GameObject reqItemObj = Instantiate(requiredItemPrefab, requiredItemsParent);
            reqItemObj.GetComponent<Slot>().item = reqItem;
            Image requiredItemImage = reqItemObj.GetComponent<Image>();
            if (inventoryCopy.Contains(reqItem))
            {
                inventoryCopy.Remove(reqItem); // Remove one instance to handle duplicates
                requiredItemImage.color = availableColor;
            }
            else
            {
                canCraft = false;
                requiredItemImage.color = missingColor;
            }
            reqItemObj.transform.GetChild(0).GetComponent<Image>().sprite = recipeData.requiredItems[i].visual;

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
            Inventory.instance.RemoveItem(currentRecipe.requiredItems[i]);
        }
        Inventory.instance.AddItem(currentRecipe.craftableItem);
    }
}
