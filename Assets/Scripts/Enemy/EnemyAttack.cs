using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    public GameObject player;

    public float damage;
    public float damageInterval = 1f;
    private float lastDamageTime = 0f;

    void Start() {
        player = GameObject.Find("Player");
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (Time.time - lastDamageTime > damageInterval && player.GetComponent<PlayerHealth>().currentHealth > 0) {
                player.GetComponent<PlayerHealth>().currentHealth -= damage;
                lastDamageTime = Time.time;
            }
        }
    }
}
