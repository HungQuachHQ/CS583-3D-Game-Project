using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {
    public NavMeshAgent navMeshAgent;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float moveSpeed = 1f;

    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;

    public Transform[] waypoints;
    int m_CurrentWaypointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    float m_WaitTime;
    float m_TimeToRotate;
    bool m_PlayerInRange;
    bool m_PlayerNear;
    bool m_IsPatrol;
    bool m_CaughtPlayer;

    [SerializeField] private AudioClip walkSFX;
    Animator animator;
    private EnemyHealth healthStatus;

    void Start() {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_PlayerInRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;

        m_CurrentWaypointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);

        animator = GetComponent<Animator>();
        healthStatus = GetComponent<EnemyHealth>();
    }

    void Update() {
        EnvironmentView();

        if (!healthStatus.isDead) {
            if (m_IsPatrol) {
                Patrolling();
            }
            else {
                Chasing();
            }
        }
    }

    public void PlayWalkSFX() {
        SoundManager.instance.PlaySound(walkSFX);
    }

    private void Chasing() {
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;

        if (!m_CaughtPlayer) {
            Move(moveSpeed);
            navMeshAgent.SetDestination(m_PlayerPosition);
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
            if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f) {
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(moveSpeed);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
            else {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f) {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    private void Patrolling() { 
        if (m_PlayerNear) {
            if (m_TimeToRotate <= 0) {
                Move(moveSpeed);
                LookingPlayer(playerLastPosition);
            }
            else {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else {
            m_PlayerNear = false;
            playerLastPosition = Vector3.zero;
            
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
                Stop();
                m_WaitTime -= Time.deltaTime;

                if (m_WaitTime <= 0) {
                    NextPoint();
                    m_WaitTime = startWaitTime;

                    Move(moveSpeed);
                }
            }
            else {
                Move(moveSpeed);
            }
        }
    }

    void Move(float speed) {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;

        animator.SetBool("isWalking", true);
    }

    void Stop() {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;

        animator.SetBool("isWalking", false);
    }

    public void NextPoint() {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    void CaughtPlayer() {
        m_CaughtPlayer = true;
    }

    void LookingPlayer(Vector3 player) {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3) {
            if (m_WaitTime <= 0) {
                m_PlayerNear = false;
                Move(moveSpeed);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnvironmentView() {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < playerInRange.Length; i++) {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2) {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask)) {
                    m_PlayerInRange = true;
                    m_IsPatrol = false;
                }
                else {
                    m_PlayerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius) {
                m_PlayerInRange = false;
            }
            if (m_PlayerInRange)
            {
                m_PlayerPosition = player.transform.position;
            }
        }
    }

    public void OnHitByPlayer(Transform player) {
        Debug.Log("OnHitByPlayer called from: " + player.name);

        m_PlayerInRange = true;
        m_IsPatrol = false;
        m_PlayerNear = false;
        m_CaughtPlayer = false;

        m_WaitTime = 0f;
        m_TimeToRotate = 0f;    

        m_PlayerPosition = player.position;
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(m_PlayerPosition);
        Move(moveSpeed);
    }
}
