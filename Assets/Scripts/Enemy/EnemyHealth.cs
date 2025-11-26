using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    Rigidbody rb;
    Animator animator;

    public float health;
    public float currentHealth;

    public bool isHurt;
    public bool isDead;

    void Start() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

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
            Debug.Log(gameObject.name + " is dead");
            isDead = true;
            animator.SetTrigger("isDead");

            gameObject.layer = LayerMask.NameToLayer("Dead Enemies");
        }
        else {
            isDead = false;
        }
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
    }
}
