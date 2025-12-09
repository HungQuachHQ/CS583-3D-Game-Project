using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour {
    AudioSource source;
    [SerializeField] AudioClip hurtSFX;
    [SerializeField] AudioClip deathSFX;

    public GameObject deathEffect;

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
            EnableDeathParicles();

            StartCoroutine(DestroyBoss());

            gameObject.layer = LayerMask.NameToLayer("Dead Enemies");
            EnemyManager.Instance.UnregisterEnemy();
        }
    }

    private IEnumerator DestroyBoss() {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
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

    public void EnableDeathParicles() {
        deathEffect.SetActive(true);
    }

    public void DisableDeathParicles() { 
        deathEffect.SetActive(false);
    }
}
