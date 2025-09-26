using UnityEngine;

public class CraftingSystem : MonoBehaviour

{
    [SerializeField] private RecipeData[] availableRecipes;
    [SerializeField] private GameObject recipeUiPrefab;
    [SerializeField] private Transform recipeParent;
    [SerializeField] KeyCode  openCraftPanelInput;
    [SerializeField] GameObject craftPanel;
    void Start()
    {
        UpdateDisplayRecipes();
    }
    private void Update()
    {
        if (Input.GetKeyDown(openCraftPanelInput))
        {
            craftPanel.SetActive(!craftPanel.activeSelf);
            UpdateDisplayRecipes();
        }
    }
    public void UpdateDisplayRecipes()
    {
        foreach (Transform child in recipeParent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < availableRecipes.Length; i++)
        {
            GameObject recipe = Instantiate(recipeUiPrefab, recipeParent);
            recipe.GetComponent<Recipe>().Configure(availableRecipes[i]);
        }
    }
}
