using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StaffDamage : MonoBehaviour {
    public GameObject player;
    public Transform playerTransform;
    private PlayerAttack playerDamage;

    [SerializeField] private Collider staffCollider;

    void Start() {
        playerDamage = player.GetComponent<PlayerAttack>();
        player = GameObject.Find("Player");
        playerTransform = player.transform;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
            Debug.Log("Enemy hit: " + other.gameObject.name);
            enemy.TakeDamage(playerDamage.damage, playerTransform);
        }
        if (other.CompareTag("Boss")) {
            BossHealth boss = other.gameObject.GetComponent<BossHealth>();
            Debug.Log("Enemy hit: " + other.gameObject.name);
            boss.TakeDamage(playerDamage.damage);
        }
    }

    public void EnableStaffCollider() {
        staffCollider.enabled = true;
    }

    public void DisableStaffCollider() {
        staffCollider.enabled = false;
    }
}
