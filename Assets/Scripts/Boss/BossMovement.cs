using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMovement : MonoBehaviour {
    AudioSource source;
    [SerializeField] AudioClip footstepsSFX1;
    [SerializeField] AudioClip footstepsSFX2;

    GameObject player;
    NavMeshAgent agent;
    Animator animator;

    private Vector3 startPosition;

    public float viewRadius = 20;
    public float viewAngle = 180;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;

    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float moveSpeed = 1f;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    float m_WaitTime;
    float m_TimeToRotate;
    bool m_PlayerInRange;
    bool m_PlayerNear;
    bool m_CaughtPlayer;

    void Start() {
        m_PlayerPosition = Vector3.zero;
        m_CaughtPlayer = false;
        m_PlayerInRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;

        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        agent.speed = moveSpeed;

        source = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();

        startPosition = transform.position;
    }

    void Update() {
        EnvironmentalView();

        Chasing();
    }

    private void Chasing() {
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;

        if (!m_CaughtPlayer) {
            Move(moveSpeed);
            agent.SetDestination(m_PlayerPosition);
        }
        if (agent.remainingDistance <= agent.stoppingDistance) {
            if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, player.transform.position) >= 6f) {
                m_PlayerNear = false;
                Move(moveSpeed);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                agent.SetDestination(startPosition);
            }
            else {
                if (Vector3.Distance(transform.position, player.transform.position) >= 2.5f) {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    void Move(float speed) {
        agent.isStopped = false;
        agent.speed = speed;

        animator.SetBool("isWalking", true);
    }

    void Stop() { 
        agent.isStopped = true;
        agent.speed = 0;

        animator.SetBool("isWalking", false);
    }

    void EnvironmentalView() {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < playerInRange.Length; i++) { 
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask)) {
                    m_PlayerInRange = true;
                }
                else {
                    m_PlayerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius) {
                m_PlayerInRange = false;
            }
            if (m_PlayerInRange) {
                m_PlayerPosition = player.transform.position;
            }
        }
    }

    public void PlayWalkSFX1() {
        SoundManager.instance.PlayAudio(footstepsSFX1, source);
    }

    public void PlayWalkSFX2() {
        SoundManager.instance.PlayAudio(footstepsSFX2, source);
    }
}
