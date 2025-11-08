using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Equipment equipment;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)&& equipment.equipWeapon != null)
        {
            animator.SetTrigger("Attack");
        }
    }
}
