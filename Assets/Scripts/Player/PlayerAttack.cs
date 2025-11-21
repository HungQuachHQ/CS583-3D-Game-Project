using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    public Animator armAnimator;
    public float attackDuration;
    public float colliderDuration;

    public bool isAttacking = false;

    public float damage;

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
        StartCoroutine(EndAttack());
    }

    private IEnumerator EndAttack() {
        yield return new WaitForSeconds(attackDuration);
        armAnimator.SetBool("isAttacking", false);
        isAttacking = false;
    }
}
