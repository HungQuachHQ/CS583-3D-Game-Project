using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAttack : MonoBehaviour {
    NavMeshAgent agent;
    Animator animator;
    public GameObject player;

    public GameObject attackPoint;
    public float radius;
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

    private IEnumerator DelayAttack() {
        yield return new WaitForSeconds(attackCooldown);
        attackBlocked = false;
    }

    private IEnumerator AnimationDuration() {
        yield return new WaitForSeconds(animationDuration);
        isAttacking = false;
    }
}
