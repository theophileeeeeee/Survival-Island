using Unity.VisualScripting;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public float interactionRange = 2.6f;
    [SerializeField]
    public LayerMask itemLayer;

    public InteractBehaviour playerInteractBehaviour;
    [SerializeField]
    private GameObject interactText;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, itemLayer))
        {
            interactText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.collider.CompareTag("Item"))
                {
                    playerInteractBehaviour.DoPickup(hit.transform.gameObject.GetComponent<Item>());
                }
                if (hit.collider.CompareTag("Harvestable"))
                {
                    playerInteractBehaviour.DoHarvest(hit.transform.gameObject.GetComponent<Harvestable>());
                }
            }
        }
        else
        {
            interactText.SetActive(false);
        }
    }
    void OnDrawGizmosSelected()
{
    Gizmos.color = Color.yellow;
    Gizmos.DrawLine(transform.position, transform.position + transform.forward * interactionRange);

    // Petite sphère à la fin du ray
    Gizmos.DrawSphere(transform.position + transform.forward * interactionRange, 0.05f);
}

}
