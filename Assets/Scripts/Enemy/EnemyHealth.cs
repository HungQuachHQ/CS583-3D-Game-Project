using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    [SerializeField] private AudioClip hurtSFX;
    [SerializeField] private AudioClip deathSFX;

    [SerializeField] private EnemyAI enemyAI;

    Animator animator;

    public float health;
    public float currentHealth;

    public bool isHurt;
    public bool isDead;

    void Start() {
        animator = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();

        currentHealth = health;
    }

    void Update() {
        HandleHealth();
    }

    private void HandleHealth() {
        if (health < currentHealth && !isDead) {
            currentHealth = health;
            isHurt = true;
        }
        else {
            isHurt = false;
        }

        if (currentHealth <= 0) {
            //Debug.Log(gameObject.name + " is dead");
            isDead = true;
            animator.SetTrigger("isDead");

            gameObject.layer = LayerMask.NameToLayer("Dead Enemies");
        }
        else {
            isDead = false;
        }
    }

    public void TakeDamage(float damage, Transform attacker) {
        currentHealth -= damage;
        PlayHurtSFX();

        if (currentHealth > 0 && enemyAI != null) {
            enemyAI.OnHitByPlayer(attacker);
        }
    }

    private void PlayHurtSFX() {
        SoundManager.instance.PlaySound(hurtSFX);
    }

    public void PlayDeathSFX() { 
        SoundManager.instance.PlaySound(deathSFX);
    }
}
