using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameManager GM;

    public NavMeshAgent agent;

    public Transform player;
    public Transform attackPoint;

    public LayerMask whatIsGround, whatIsPlayer;

    public Animator spiderAnim;

    public int health = 3;
    public int dmgAmt = 1;
    public bool finalBoss = false;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, dead;

    Collider enemCol;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        enemCol = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (dead)
        {
            playerInSightRange = false;
            playerInAttackRange = false;
            agent.SetDestination(transform.position);
        }
        if (!playerInSightRange && !playerInAttackRange && !dead) Patrolling();
        if (playerInSightRange && !playerInAttackRange && !dead) ChasePlayer();
        if (playerInAttackRange && playerInSightRange && !dead) Attacking();
    }

    void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            spiderAnim.SetBool("Walking", true);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    void Attacking()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        
        if (!alreadyAttacked)
        {
            Collider[] playerCol = Physics.OverlapSphere(attackPoint.position, attackRange, whatIsPlayer);

            foreach (Collider playerCollision in playerCol)
            {
                GM.DamagePlayer(dmgAmt);
            }
            spiderAnim.SetBool("Attack", true);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
        spiderAnim.SetBool("Attack", false);
    }

    public void TakeDamage()
    {
        health--;
        if(health <= 0)
        {
            dead = true;
            enemCol.enabled = false;
            StartCoroutine("Die");
        }
    }

    IEnumerator Die() 
    {
        spiderAnim.SetBool("Die", true);
        yield return new WaitForSeconds(2.0f);
        if (finalBoss)
            GM.GameOver();
        Destroy(this.gameObject);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
