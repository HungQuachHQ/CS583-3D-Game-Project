using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    public static EnemyManager Instance { get; private set; }

    public int EnemiesAlive { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public void RegisterEnemy() {
        EnemiesAlive++; 
    }

    public void UnregisterEnemy() { 
        EnemiesAlive--;
    }

    public bool AllEnemiesDefeated() {
        return EnemiesAlive == 0;
    }
}
