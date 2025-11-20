using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StaffDamage : MonoBehaviour {
    public GameObject player;
    private PlayerAttack playerDamage;

    void Start() {
        playerDamage = player.GetComponent<PlayerAttack>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            Debug.Log("Enemy hit: " + other.gameObject.name);
            other.GetComponent<EnemyHealth>().currentHealth -= playerDamage.damage;
        }
    }
}
