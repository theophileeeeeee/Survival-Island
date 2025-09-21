using UnityEngine;
using System.Collections;

public class InteractBehaviour : MonoBehaviour
{
    private Vector3 spawnOffset = new Vector3(0, 0.5f, 0);
    [SerializeField]
    private MoveBehaviour playerMovement;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private Inventory inventory;
    private Item currentItem;
    private Harvestable currentHarvestable;
    [Header("Tools Visuals")]
    private Tool currentTool;
    [SerializeField]
    private GameObject pickAxeVisual;
    [SerializeField]
    private GameObject axeVisual;

    public void DoPickup(Item item)
    {
        if (inventory.IsFull())
        {
            Debug.Log("Inventory is full, can't pick up" + item.name);
            return;
        }
        currentItem = item;
        playerAnimator.SetTrigger("PickUp");
        playerMovement.canMove = false;
    }
    public void DoHarvest(Harvestable harvestable)
    {
        currentTool = harvestable.Tool;
        EnableToolGameObjectFromEnum(currentTool);
        currentHarvestable = harvestable;
        playerAnimator.SetTrigger("Harvest");
        playerMovement.canMove = false;
    }
    IEnumerator BreakHarvestable()
    {
        if (currentHarvestable.disableKinematicsOnHarvest)
        {
            Rigidbody rb = currentHarvestable.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(new Vector3(750, 750, 0), ForceMode.Impulse);
        }
        yield return new WaitForSeconds(currentHarvestable.destroyDelay);

        for (int i = 0; i < currentHarvestable.harvestableItems.Length; i++)
        {
            Ressource ressource = currentHarvestable.harvestableItems[i];
            if (Random.Range(0, 100) <= ressource.dropChance)
            {
                GameObject instanciatedRessource = Instantiate(ressource.Item.prefab);
                instanciatedRessource.transform.position = currentHarvestable.transform.position + spawnOffset;
            }
        }


        Destroy(currentHarvestable.gameObject);
    }
    public void AddItemToInventory()
    {
        inventory.AddItem(currentItem.itemData);
        Destroy(currentItem.gameObject);
    }
    public void ReEnableMovement()
    {
        EnableToolGameObjectFromEnum(currentTool, false);
        playerMovement.canMove = true;
    }
    public void EnableToolGameObjectFromEnum(Tool tool, bool enable = true)
    {
        switch (tool)
        {
            case Tool.Axe:
                axeVisual.SetActive(enable);
                break;
            case Tool.Pickaxe:
                pickAxeVisual.SetActive(enable);
                break;
        }
    }


}
