using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    // References
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsPlayer;
    public Animator animator;

    // Patrol variables
    private Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    // Attack variables
    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    // Detection ranges
    public float sightRange;
    public float attackRange;
    private bool playerInSightRange;
    private bool playerInAttackRange;

    public string gameOverSceneName = "GameOverScene";

    private void Awake()
    {
        player = GameObject.Find("PlayerCapsule").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Pause enemy movement when menus are open
        if (UIController.IsBookOpen || UIController.IsHelpOpen)
        {
            agent.SetDestination(transform.position);
            ResetAnimations();
            return;
        }

        // Check if player is within sight or attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // Decide behavior based on player's position
        if (!playerInSightRange && !playerInAttackRange)
            Patrol();

        if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();

        if (playerInAttackRange && playerInSightRange)
            AttackPlayer();
    }

    private void Patrol()
    {
        // Search for a walk point if not set
        if (!walkPointSet)
            SearchWalkPoint();

        // Move to the walk point
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            ResetAnimations();
            animator.SetBool("isWalking", true);
        }

        // Reset walk point if reached
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            walkPointSet = false;
            animator.SetBool("isWalking", false);
        }
    }

    private void SearchWalkPoint()
    {
        // Generate a random point within the walk point range
        Vector3 randomPoint = transform.position +
            new Vector3(Random.Range(-walkPointRange, walkPointRange), 0f,
                        Random.Range(-walkPointRange, walkPointRange));

        // Validate the point on the NavMesh
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, walkPointRange, NavMesh.AllAreas))
        {
            walkPoint = hit.position;
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        // Chase the player when detected
        agent.SetDestination(player.position);
        ResetAnimations();
        animator.SetBool("isRunning", true);
    }

    private void AttackPlayer()
    {
        // Stop moving and face the player
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        ResetAnimations();

        // Prevent attack spamming
        if (alreadyAttacked)
            return;

        animator.SetTrigger("Attack");
        alreadyAttacked = true;

        // Trigger game over after attack animation
        Invoke(nameof(LoadGameOver), 0.4f);
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void ResetAnimations()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }

    private void LoadGameOver()
    {
        SceneManager.LoadScene(gameOverSceneName);
    }
}
