using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    public Animator armAnimator;
    public float attackDuration;
    public float colliderDuration;

    public bool isAttacking = false;

    public float damage;

    public Collider staffCollider;

    void Start() {
        Transform cameraTransform = GameObject.Find("PlayerCamera").transform;
        Transform armTransform = cameraTransform.Find("Arms");

        armAnimator = armTransform.GetComponent<Animator>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && !isAttacking) {
            Attack();
        }
    }

    private void Attack() {
        armAnimator.SetBool("isAttacking", true);
        isAttacking = true;

        StartCoroutine(EnableCollider(staffCollider, colliderDuration));
        StartCoroutine(EndAttack());
    }

    private IEnumerator EnableCollider(Collider col, float duration) {
        col.enabled = true;
        yield return new WaitForSeconds(duration);
        col.enabled = false;
    }

    private IEnumerator EndAttack() {
        yield return new WaitForSeconds(attackDuration);
        armAnimator.SetBool("isAttacking", false);
        isAttacking = false;
    }
}
