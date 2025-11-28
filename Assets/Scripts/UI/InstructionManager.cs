using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstructionManager : MonoBehaviour {
    public TextMeshProUGUI tutorialText;

    private Dictionary<string, string> instructions = new() {
        { "MovementCollider", "Use WASD to move." },
        { "JumpCollider", "Press the Space Bar to jump." },
        { "AttackCollider", "Click the Left Mouse Button to attack." }
    };

    public void ShowInstruction(string colliderName) {
        if (instructions.ContainsKey(colliderName)) { 
            tutorialText.text = instructions[colliderName];
            tutorialText.gameObject.SetActive(true);
        }
    }

    public void HideInstruction() {
        tutorialText.gameObject.SetActive(false);
    }
}
