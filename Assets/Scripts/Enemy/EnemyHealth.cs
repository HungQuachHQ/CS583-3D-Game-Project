using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    public float health;
    public float currentHealth;

    public bool isHurt;
    public bool isDead;

    void Start() {
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
            Destroy(gameObject);
        }
        else {
            isDead = false;
        }
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
    }
}
