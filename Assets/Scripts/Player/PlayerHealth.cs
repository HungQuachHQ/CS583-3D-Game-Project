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

    private float levelBonusMaxHealth = 0f;


    private PlayerStats playerStats;

    public void ApplyMaxHealthBonus(float bonus)
    {
        //Remove old bonus, apply new
        maxHealth = maxHealth - levelBonusMaxHealth + bonus;
        levelBonusMaxHealth = bonus;

        //Update slider
        slider.maxValue = maxHealth;

        //Heal the full on level up (optional -remove if you don't want this)
        currentHealth = maxHealth;
        playerHealth = maxHealth;
        slider.value = currentHealth;
    }

    void Start() {
        source = GetComponent<AudioSource>();

        playerStats = GetComponent<PlayerStats>();

        playerHealth = maxHealth;
        currentHealth = maxHealth;

        if (slider != null)
        {
            slider.maxValue = maxHealth;
            slider.value = currentHealth;
        }
    }

    void Update() {
        HandleHealth();
    }

    private void HandleHealth() {
        // Always keep the slider in sync with current health 
        if(slider != null)
        {
            slider.value = currentHealth;
        }
        
        if (currentHealth < playerHealth && !isDead) {
            playerHealth = currentHealth;
            slider.value = currentHealth;

            isHurt = true;
        }
        else {
            isHurt = false;
        }

        if (currentHealth <= 0 && !isDead) {
            isDead = true;
            //Todo: trigger death logic if desired?
        }
    }

    public void TakeDamage(float damage) {
        float finalDamage = damage;

        if(playerStats != null)
        {
            finalDamage -= playerStats.currentDefense;
            if (finalDamage < 1f) finalDamage = 1f;
        }

        currentHealth -= finalDamage;
        currentHealth = Mathf.Max(currentHealth, 0f);
        PlayHurtSFX();
    }

    private void PlayHurtSFX() {
        SoundManager.instance.PlayAudio(hurtSFX, source);
    }
}