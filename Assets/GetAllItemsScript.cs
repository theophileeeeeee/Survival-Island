using System.Collections.Generic;
using UnityEngine;

public class GetAllItemsScript : MonoBehaviour
{
    [Header("Liste des items à ajouter")]
    public List<ItemData> itemsToGive = new List<ItemData>();

    [Header("Touche pour ajouter les items")]
    public KeyCode giveItemsKey = KeyCode.G;

    void Update()
    {
        if (Input.GetKeyDown(giveItemsKey))
        {
            foreach (ItemData item in itemsToGive)
            {
                Inventory.instance.AddItem(item);
            }
            Debug.Log("Items ajoutés !");
        }
    }
}
