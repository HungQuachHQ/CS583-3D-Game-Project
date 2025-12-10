using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    [SerializeField] private AudioClip attackSFX;
    public Animator armAnimator;
    private AudioSource source;
    
    public float attackDuration;
    public float colliderDuration;

    public bool isAttacking = false;

    public float damage;

    void Start() {
        Transform cameraTransform = GameObject.Find("PlayerCamera").transform;
        Transform armTransform = cameraTransform.Find("Arms");

        armAnimator = armTransform.GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && !isAttacking && Time.timeScale != 0f) {
            Attack();
        }
    }

    private void Attack() {
        PlayAttackSFX();
        armAnimator.SetBool("isAttacking", true);
        isAttacking = true;
        StartCoroutine(EndAttack());
    }

    private IEnumerator EndAttack() {
        yield return new WaitForSeconds(attackDuration);
        armAnimator.SetBool("isAttacking", false);
        isAttacking = false;
    }

    private void PlayAttackSFX() {
        SoundManager.instance.PlayAudio(attackSFX, source);
    }
}
