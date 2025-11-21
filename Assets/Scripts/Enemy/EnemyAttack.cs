using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    public float damage;
    public float damageInterval = 1f;
    private float lastDamageTime = 0f;

    private void OnCollisionStay(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
            if (Time.time - lastDamageTime > damageInterval && player.currentHealth > 0) {
                player.TakeDamage(damage);
                lastDamageTime = Time.time;
            }
        }
    }
}
