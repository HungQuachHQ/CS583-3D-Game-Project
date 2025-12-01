using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireController : MonoBehaviour {
    public GameObject bonfireMessage;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && EnemyManager.Instance.AllEnemiesDefeated()) {
            bonfireMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            bonfireMessage.SetActive(false);
        }
    }
}
