using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAttack : MonoBehaviour {
    [SerializeField] private GameObject attackParticles;
    NavMeshAgent agent;
    Animator animator;
    AudioSource source;
    [SerializeField] AudioClip attackSFX;
    public GameObject player;

    public GameObject attackPoint;
    public Vector3 aoeHalfExtents = new Vector3(5f, 0.5f, 5f); // x, z = radius, y = height
    public LayerMask players;
    public float damage;
    public float turnSpeed = 8f;

    public float attackRange;
    public float attackCooldown;
    public float animationDuration;

    public bool isAttacking = false;
    private bool attackBlocked = false;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        player = GameObject.Find("Player");
    }

    void FixedUpdate() {
        AttackRange();
    }

    private void AttackRange() {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (isAttacking) {
            agent.updateRotation = false;
        }
        else {
            agent.updateRotation = true;
        }

        if (distance <= attackRange && !isAttacking) {
            Attack();
        }
    }

    private void Attack() {
        if (attackBlocked) {
            return;
        }

        isAttacking = true;
        animator.SetTrigger("isAttacking");
        attackBlocked = true;

        StartCoroutine(DelayAttack());
        StartCoroutine(AnimationDuration());
    }

    public void AttackDetection() {
        Collider[] player = Physics.OverlapBox(attackPoint.transform.position, aoeHalfExtents, Quaternion.identity, players);

        foreach (Collider playerGameObject in player) {
            playerGameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.transform.position, aoeHalfExtents * 2f);
    }

    private IEnumerator DelayAttack() {
        yield return new WaitForSeconds(attackCooldown);
        attackBlocked = false;
    }

    private IEnumerator AnimationDuration() {
        yield return new WaitForSeconds(animationDuration);
        isAttacking = false;
    }

    public void EnableAttackParticles() {
        attackParticles.SetActive(true);
    }

    public void DisableAttackParticles() {
        attackParticles.SetActive(false);
    }

    public void PlayAttackSFX() {
        SoundManager.instance.PlayAudio(attackSFX, source);
    }
}
