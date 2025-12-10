using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    [SerializeField] private AudioClip hurtSFX;
    AudioSource source;

    public float maxHealth;
    public float playerHealth;
    public float currentHealth;
    public Slider slider;

    public bool isHurt = false;
    public bool isDead = false;

    void Start() {
        source = GetComponent<AudioSource>();

        playerHealth = maxHealth;
        currentHealth = maxHealth;

        slider.maxValue = maxHealth;
        slider.value = currentHealth;
    }

    void Update() {
        HandleHealth();
    }

    private void HandleHealth() {
        if (currentHealth < playerHealth && !isDead) {
            playerHealth = currentHealth;
            slider.value = currentHealth;

            isHurt = true;
        }
        else {
            isHurt = false;
        }

        if (currentHealth <= 0) {
            isDead = true;
        }
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        PlayHurtSFX();
    }

    private void PlayHurtSFX() {
        SoundManager.instance.PlayAudio(hurtSFX, source);
    }
}
