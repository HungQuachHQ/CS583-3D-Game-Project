using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour {
    Animator animator;
    //AudioSource source;

    public float health;
    public float currentHealth;

    public bool isHurt;
    public bool isDead;
    
    void Start() {
        animator = GetComponent<Animator>();

        currentHealth = health;
        isHurt = false;
        isDead = false;

        EnemyManager.Instance.RegisterEnemy();
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

        if (currentHealth <= 0 && !isDead) {
            Debug.Log(gameObject.name + " is dead");
            isDead = true;
            //animator.SetTrigger("isDead");

            gameObject.layer = LayerMask.NameToLayer("Dead Enemies");
            EnemyManager.Instance.UnregisterEnemy();
        }
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
    }
}
