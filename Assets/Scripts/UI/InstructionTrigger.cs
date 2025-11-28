using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionTrigger : MonoBehaviour {
    private InstructionManager manager;

    void Start() {
        manager = FindObjectOfType<InstructionManager>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            manager.ShowInstruction(gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            manager.HideInstruction();
            gameObject.SetActive(false);
        }
    }
}
