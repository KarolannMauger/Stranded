using UnityEngine;
using UnityEngine.AI;

public class NPCWander : MonoBehaviour
{
    public NavMeshAgent agent;
    public LayerMask whatIsGround;
    public Animator animator;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 10f;

    public float minWaitTime = 2f;
    public float maxWaitTime = 5f;
    private float waitTimer;
    private bool isWaiting;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            animator.SetBool("isRunning", false);

            if (waitTimer <= 0)
                isWaiting = false;
        }
        else
        {
            Wander();
        }
    }

    private void Wander()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            animator.SetBool("isRunning", true);
        }

        if (walkPointSet && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            walkPointSet = false;
            isWaiting = true;
            waitTimer = Random.Range(minWaitTime, maxWaitTime);
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y + 10f, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, Vector3.down, out RaycastHit hit, 20f, whatIsGround))
        {
            walkPoint = hit.point;
            walkPointSet = true;
        }
    }
}