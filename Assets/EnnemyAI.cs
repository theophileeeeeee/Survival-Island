using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class EnnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;

    [Header("Stats")]
    [SerializeField] private float detectionRadius;
    [SerializeField] private float attackRadius;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float attackDelay;
    [SerializeField] private int attackDamage;
    [SerializeField] private float rotationSpeed;

    [Header("Wandering Parameters")]
    [SerializeField] private float wanderWaitTimeMin;
    [SerializeField] private float wanderWaitTimeMax;
    [SerializeField] private float wanderingDistanceMin;
    [SerializeField] private float wanderingDistanceMax;
    private bool hasDestination;
    private bool isAttacking;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < detectionRadius)
        {
            agent.speed = runSpeed;
            Quaternion rot = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
            if (!isAttacking)
            {
                if (Vector3.Distance(transform.position, player.position) < attackRadius)
                {
                    StartCoroutine(attackPlayer());
                }
                else
                {
                    agent.SetDestination(player.position);
                }
            }

        }
        else
        {
            agent.speed = walkSpeed;
            if (agent.remainingDistance < 0.75f && !hasDestination)
            {
                StartCoroutine(SetRandomDestination());

            }
        }
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
    IEnumerator SetRandomDestination()
    {
        hasDestination = true;
        yield return new WaitForSeconds(Random.Range(wanderWaitTimeMin, wanderWaitTimeMax));
        Vector3 nextDestination = transform.position;
        nextDestination += Random.Range(wanderingDistanceMin, wanderingDistanceMax) * new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(nextDestination, out hit, wanderingDistanceMax, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        hasDestination = false;
    }
    IEnumerator attackPlayer()
    {
        isAttacking = true;
        agent.isStopped = true;
        animator.SetTrigger("Attack");
        playerStats.TakeDamage(attackDamage);
        yield return new WaitForSeconds(attackDelay);
        agent.isStopped = false;
        isAttacking = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}

