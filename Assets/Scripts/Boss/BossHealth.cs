using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour {
    AudioSource source;
    [SerializeField] AudioClip hurtSFX;
    [SerializeField] AudioClip deathSFX;

    public float health;
    public float currentHealth;

    public bool isHurt;
    public bool isDead;
    
    void Start() {
        source = GetComponent<AudioSource>();

        currentHealth = health;
        isHurt = false;
        isDead = false;

        EnemyManager.Instance.RegisterEnemy();
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

        if (currentHealth <= 0 && !isDead) {
            Debug.Log(gameObject.name + " is dead");
            isDead = true;
            PlayDeathSFX();

            gameObject.layer = LayerMask.NameToLayer("Dead Enemies");
            EnemyManager.Instance.UnregisterEnemy();
        }
    }
    public void TakeDamage(float damage) {
        currentHealth -= damage;

        if (currentHealth > 0 && !isDead) {
            PlayHurtSFX();
        }
    }

    public void PlayHurtSFX() {
        SoundManager.instance.PlayAudio(hurtSFX, source);
    }

    public void PlayDeathSFX() {
        SoundManager.instance.PlayAudio(deathSFX, source);
    }
}
