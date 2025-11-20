using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    public Animator armAnimator;
    public float attackTime;

    void Start() {
        Transform cameraTransform = GameObject.Find("PlayerCamera").transform;
        Transform armTransform = cameraTransform.Find("Arms");

        armAnimator = armTransform.GetComponent<Animator>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Attack();
        }
    }

    private void Attack() {
        armAnimator.SetBool("isAttacking", true);

        StartCoroutine(EndAttack());
    }

    private IEnumerator EndAttack() {
        yield return new WaitForSeconds(attackTime);
        armAnimator.SetBool("isAttacking", false);
    }
}
