using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    
    public enum EnemyState { Patrolling, Shooting }
    public EnemyState currentState = EnemyState.Patrolling;
    Animator enemyAnimator;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    private int currentPatrolIndex = 0;
    public float enemyMoveSpeed = 10;
    private Transform player;
    public int enemyHealth = 3;
    public float rotationSpeed;
    private NavMeshAgent agent;
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    public LayerMask obstructionMask;
    public Transform shootingPoint;
    public float aimTime = 0.5f;           
    public float shootCooldown = 1.2f;     
    public float aimError = 7f;            
    public float viewRange = 20f;          

    private bool canShoot = true;
    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void Update()
    {
        switch (currentState) {
            case EnemyState.Patrolling:
                Patrol();
                break;
            case EnemyState.Shooting:
                if (canShoot && IsPlayerVisible())
                {
                    StartCoroutine(AimAndShoot());
                }
                break;
        }
    }

    bool IsPlayerVisible()
    {
        // Raycast to check line of sight
        Vector3 directionToPlayer = player.position - transform.position;
        float distance = directionToPlayer.magnitude;

        if (distance > viewRange) return false; // Too far

        if (!Physics.Raycast(transform.position, directionToPlayer.normalized, distance, obstructionMask))
            return true; // No obstruction

        return false;
    }

    IEnumerator AimAndShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(aimTime);

        Vector3 direction = player.position - transform.position;
        direction = Quaternion.Euler(Random.Range(-aimError, aimError), Random.Range(-aimError, aimError), 0) * direction;

        Shoot(direction);

        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    

    void Patrol()
    {
        //Debug.Log("agent pathpending : " + !agent.pathPending);
        agent.speed = enemyMoveSpeed;
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (currentWaypointIndex < waypoints.Count() - 1)
            {
                currentWaypointIndex = Random.Range(0, waypoints.Count() - 1);
            }
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
        enemyAnimator.SetBool("Walk", true);
        enemyAnimator.SetBool("Shoot", false);
    }

    void Shoot(Vector3 direction)
    {
        
        Debug.DrawRay(transform.position, direction.normalized * 10, Color.red, 1f);
        //ObjectPool.Instance.SpawnFromPool("Bullet", shootingPoint.position, Quaternion.identity);
        if (Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, Mathf.Infinity))
        {
            if (hitInfo.transform.GetComponent<PlayerHealth>() != null)
            {
                hitInfo.transform.GetComponent<PlayerHealth>().SetHealth();    
            }
            Debug.Log(hitInfo.transform.name);
        }
        
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        agent.speed = 0;

        enemyAnimator.SetBool("Walk", false);
        enemyAnimator.SetBool("Shoot", true);
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            currentState = EnemyState.Shooting;
            player = other.gameObject.transform;
        }
        
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            currentState = EnemyState.Patrolling;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
