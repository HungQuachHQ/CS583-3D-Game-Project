using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    [SerializeField] private AudioClip hurtSFX;
    [SerializeField] private AudioClip deathSFX;

    [SerializeField] private EnemyAI enemyAI;

    Animator animator;
    AudioSource source;

    public GameObject deathEffect;

    public float health;
    public float currentHealth;

    public bool isHurt;
    public bool isDead;

    void Start() {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        enemyAI = GetComponent<EnemyAI>();

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
            //Debug.Log(gameObject.name + " is dead");
            isDead = true;
            animator.SetTrigger("isDead");

            StartCoroutine(DestroyEnemy());

            gameObject.layer = LayerMask.NameToLayer("Dead Enemies");
            EnemyManager.Instance.UnregisterEnemy();
        }
    }

    private IEnumerator DestroyEnemy() {
        yield return new WaitForSeconds(3);
        EnableDeathEffect();

        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    public void TakeDamage(float damage, Transform attacker) {
        currentHealth -= damage;
        PlayHurtSFX();

        if (currentHealth > 0 && enemyAI != null) {
            enemyAI.OnHitByPlayer(attacker);
        }
    }

    private void PlayHurtSFX() {
        SoundManager.instance.PlayAudio(hurtSFX, source);
    }

    public void PlayDeathSFX() { 
        SoundManager.instance.PlayAudio(deathSFX, source);
    }

    public void EnableDeathEffect() {
        deathEffect.SetActive(true);
    }

    public void DisableDeathEffect() { 
        deathEffect.SetActive(false);
    }
}
