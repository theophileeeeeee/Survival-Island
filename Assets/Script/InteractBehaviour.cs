using UnityEngine;
using System.Collections;
using NUnit.Framework;

public class InteractBehaviour : MonoBehaviour
{
    private Vector3 spawnOffset = new Vector3(0, 0.5f, 0);
    [SerializeField]
    private MoveBehaviour playerMovement;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private Inventory inventory;
    private bool isBusy = false;
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
        if (isBusy)
        {
            return;
        }
        isBusy = true;
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
        if (isBusy)
        {
            return;
        }
        isBusy = true;
        currentTool = harvestable.Tool;
        EnableToolGameObjectFromEnum(currentTool);
        currentHarvestable = harvestable;
        playerAnimator.SetTrigger("Harvest");
        playerMovement.canMove = false;
    }
    IEnumerator BreakHarvestable()
    {
        Harvestable harvestable = currentHarvestable;
        // permet de désactiver la possibilité d'interagir avec ce Harvestable + d'une fois (passage du layer Harvestable a Default)
        harvestable.gameObject.layer = LayerMask.NameToLayer("Default");
        if (harvestable.disableKinematicsOnHarvest)
        {
            Rigidbody rb = harvestable.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(transform.forward * 800, ForceMode.Impulse);
        }
        yield return new WaitForSeconds(harvestable.destroyDelay);

        for (int i = 0; i < harvestable.harvestableItems.Length; i++)
        {
            Ressource ressource = harvestable.harvestableItems[i];
            if (Random.Range(0, 100) <= ressource.dropChance)
            {
                GameObject instanciatedRessource = Instantiate(ressource.Item.prefab);
                instanciatedRessource.transform.position = harvestable.transform.position + spawnOffset;
            }
        }


        Destroy(harvestable.gameObject);
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
        isBusy = false;
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
