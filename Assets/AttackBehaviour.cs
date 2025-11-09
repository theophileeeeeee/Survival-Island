using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Equipment equipment;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private InteractBehaviour interactBehaviour;

    [Header("Attack parameters")]
    [SerializeField] private float attackRange;
    [SerializeField] LayerMask attackableLayer;
    [SerializeField] private Vector3 attackOffset;
    private bool isAttacking;
    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position + attackOffset, transform.forward * attackRange, Color.red);
        if (Input.GetMouseButtonDown(0) && CanAttack())
        {
            animator.SetTrigger("Attack");
            SendAttack();
            isAttacking = true;
        }
    }
    public void AttackFinished()
    {
        isAttacking = false;
    }
    bool CanAttack()
    {
        return equipment.equipWeapon != null && !isAttacking && !uiManager.AtLeastOnePanelActive && !interactBehaviour.isBusy;
    }
    void SendAttack()
    {
        Debug.Log("Attack sent!");
        RaycastHit hit;
        if (Physics.Raycast(transform.position + attackOffset, transform.forward, out hit, attackRange, attackableLayer))
        {
            if (hit.transform.CompareTag("AI"))
            {
               EnnemyAI enemy = hit.transform.GetComponent<EnnemyAI>();
               enemy.TakeDamage(equipment.equipWeapon.attackPoints);
           }
       }  
   }
}