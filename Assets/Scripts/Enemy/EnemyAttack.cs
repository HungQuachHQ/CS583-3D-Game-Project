using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    Animator animator;
    public GameObject player;

    public GameObject attackPoint;
    public float radius;
    public LayerMask players;
    public float damage;

    public float attackRange;
    public float attackDelay = 0.3f;

    public bool isAttacking;
    private bool attackBlocked;

    private EnemyHealth healthStatus;

    private void Start() {
       healthStatus = GetComponent<EnemyHealth>();
       animator = GetComponent<Animator>();
       player = GameObject.Find("Player");
    }

    private void FixedUpdate() {
        AttackRange();
    }

    private void AttackRange() {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= attackRange && !isAttacking && !healthStatus.isDead) {
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
    }

    private IEnumerator DelayAttack() { 
        yield return new WaitForSeconds(attackDelay);
        attackBlocked = false;
        isAttacking = false;
    }

    public void AttackDetection() {
        Collider[] player = Physics.OverlapSphere(attackPoint.transform.position, radius, players);

        foreach (Collider playerGameObject in player) {
            playerGameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(attackPoint.transform.position, radius);
    }
}
