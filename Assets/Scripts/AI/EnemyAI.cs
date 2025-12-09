using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public Animator animator;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public string gameOverSceneName = "GameOverScene";

    private void Awake()
    {
        player = GameObject.Find("PlayerCapsule").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (UIController.IsBookOpen || UIController.IsHelpOpen)
        {
            agent.SetDestination(transform.position);
            ResetStates();

            return;
        }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    void ResetStates()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            ResetStates();
            animator.SetBool("isWalking", true);
        }

        if (walkPointSet && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            walkPointSet = false;
            ResetStates();
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (NavMesh.SamplePosition(walkPoint, out NavMeshHit hit, walkPointRange, NavMesh.AllAreas))
        {
            walkPoint = hit.position;
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);

        ResetStates();
        animator.SetBool("isRunning", true);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        ResetStates();

        if (!alreadyAttacked)
        {
            animator.SetTrigger("Attack");
            alreadyAttacked = true;

            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            float attackAnimLength = 0f;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == "BearAttack")
                {
                    attackAnimLength = clip.length;
                    break;
                }
            }

            Invoke(nameof(LoadGameOver), attackAnimLength * 0.4f);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void LoadGameOver()
    {
        SceneManager.LoadScene(gameOverSceneName);
    }
}
