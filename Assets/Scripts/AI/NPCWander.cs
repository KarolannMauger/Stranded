using UnityEngine;
using UnityEngine.AI;

public class NPCWander : MonoBehaviour
{
    // References
    public NavMeshAgent agent;
    public LayerMask whatIsGround;
    public Animator animator;

    // Wander variables
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 10f;

    // Wait variables
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
        // Handle idle waiting
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            animator.SetBool("isRunning", false);

            if (waitTimer <= 0)
                isWaiting = false;

            return;
        }
        
        Wander();
    }

    private void Wander()
    {
        // Search for a new walk point if not set
        if (!walkPointSet)
            SearchWalkPoint();

        // Move to the walk point
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            animator.SetBool("isRunning", true);
        }

        // Reset walk point if reached
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            walkPointSet = false;
            isWaiting = true;
            waitTimer = Random.Range(minWaitTime, maxWaitTime);
        }
    }

    private void SearchWalkPoint()
    {
        // Generate a random point within the walk point range
        Vector3 randomPoint = transform.position +
            new Vector3(Random.Range(-walkPointRange, walkPointRange), 10f, 
                        Random.Range(-walkPointRange, walkPointRange));

        // Validate the point is on the ground
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, walkPointRange, NavMesh.AllAreas))
        {
            walkPoint = hit.position;
            walkPointSet = true;
        }
    }
}