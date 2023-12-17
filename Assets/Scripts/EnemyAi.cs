
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi: MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;
    private Transform player;

    //Patroling
    [SerializeField] private Transform centrePoint;
    [SerializeField] private float range;

    //Attacking
    [SerializeField] private GameObject projectile;
    [SerializeField] private float timeBetweenAttacks;
    private bool alreadyAttacked;


    //States
    [SerializeField] private float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;

    private bool enemyHit = false;

    // for debugging
    private bool chasing = false;
    private bool patrolling = false;
    private bool attacking = false;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        player = PlayerManager.instance.player.gameObject.transform;
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange && !enemyHit) Patroling();
        if ((playerInSightRange && !playerInAttackRange && !AbilityUI.instance.dashAbility.invisible) || enemyHit ) ChasePlayer();
        if (playerInAttackRange && playerInSightRange && !AbilityUI.instance.dashAbility.invisible) AttackPlayer();
    }

    private void Patroling()
    {
        chasing = false;
        attacking = false;
        patrolling = true;
        if (agent.remainingDistance <= agent.stoppingDistance) //done with path
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                agent.SetDestination(point);
            }

        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
    public void Chase()
    {
        enemyHit = true;
        Invoke(nameof(ResetEnemyStatus), 5f);
    }
    private void ResetEnemyStatus()
    {
        enemyHit = false;
    }
    private void ChasePlayer()
    {
        chasing = true;
        attacking = false;
        patrolling = false;
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        chasing = false;
        attacking = true;
        patrolling = false;
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            GameObject projectile = ObjectPool.instance.GetPooledObject();
            projectile.transform.position = transform.position;
            if (projectile != null)
            {
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 24f, ForceMode.Impulse);
                rb.AddForce(transform.up * 6f, ForceMode.Impulse);
            }
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
