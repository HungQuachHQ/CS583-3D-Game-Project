using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StaffDamage : MonoBehaviour {
    public GameObject player;
    private PlayerAttack playerDamage;

    [SerializeField] private Collider staffCollider;

    void Start() {
        playerDamage = player.GetComponent<PlayerAttack>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
            Debug.Log("Enemy hit: " + other.gameObject.name);
            enemy.TakeDamage(playerDamage.damage);
        }
    }

    public void EnableStaffCollider() {
        staffCollider.enabled = true;
    }

    public void DisableStaffCollider() {
        staffCollider.enabled = false;
    }
}
