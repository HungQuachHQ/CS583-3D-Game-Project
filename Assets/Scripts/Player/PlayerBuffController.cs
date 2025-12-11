using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerBuffController : MonoBehaviour
{
    PlayerStats stats;
    PlayerHealth health;
    PlayerMovement movement;

    float baseMoveSpeed;

    // ----------- BUFF TIMERS -----------
    float staminaBuffRemaining = 0f;
    float attackBuffRemaining = 0f;
    float defenseBuffRemaining = 0f;

    Coroutine staminaRoutine;
    Coroutine attackRoutine;
    Coroutine defenseRoutine;

    // Expose to HUD
    public float StaminaBuffRemaining => staminaBuffRemaining;
    public float AttackBuffRemaining => attackBuffRemaining;
    public float DefenseBuffRemaining => defenseBuffRemaining;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        health = GetComponent<PlayerHealth>();
        movement = GetComponent<PlayerMovement>();

        baseMoveSpeed = movement.moveSpeed;
    }

    // ---------- HEALTH POTION ----------
    public void ApplyHealthPotion(float healFraction)
    {
        if (health == null) return;
        if (healFraction <= 0f) return;

        float restore = health.maxHealth * healFraction;
        health.currentHealth = Mathf.Min(health.currentHealth + restore, health.maxHealth);
        // PlayerHealth.Update() already syncs the slider
    }

    // ---------- STAMINA / SPEED POTION ----------
    public void ApplyStaminaPotion(float speedMultiplier, float duration)
    {
        if (movement == null) return;
        if (speedMultiplier <= 1f || duration <= 0f) return;

        // Reset / extend buff timer
        staminaBuffRemaining = duration;

        if (staminaRoutine != null)
            StopCoroutine(staminaRoutine);

        staminaRoutine = StartCoroutine(StaminaBuffCoroutine(speedMultiplier));
    }

    private IEnumerator StaminaBuffCoroutine(float speedMultiplier)
    {
        // Apply boosted speed
        movement.moveSpeed = baseMoveSpeed * speedMultiplier;

        while (staminaBuffRemaining > 0f)
        {
            // Time.deltaTime respects Time.timeScale, so buff pauses when game is paused.
            staminaBuffRemaining -= Time.deltaTime;
            yield return null;
        }

        // Restore base speed
        movement.moveSpeed = baseMoveSpeed;
        staminaBuffRemaining = 0f;
        staminaRoutine = null;
    }

    // ---------- ATTACK POTION ----------
    public void ApplyAttackPotion(int bonusPerPotion, int maxTotalBonus, float duration)
    {
        if (bonusPerPotion <= 0 || duration <= 0f) return;

        int current = stats.AttackBuffFromPotions;
        int canAdd = Mathf.Clamp(maxTotalBonus - current, 0, bonusPerPotion);
        if (canAdd <= 0) return; // already at cap

        // Add the new buff amount (up to cap)
        stats.AddAttackBuff(canAdd);

        // Reset / extend shared timer
        attackBuffRemaining = duration;

        if (attackRoutine != null)
            StopCoroutine(attackRoutine);

        attackRoutine = StartCoroutine(AttackBuffCoroutine());
    }

    private IEnumerator AttackBuffCoroutine()
    {
        while (attackBuffRemaining > 0f)
        {
            attackBuffRemaining -= Time.deltaTime;
            yield return null;
        }

        // When timer ends, clear whatever attack buff is still active
        int remainingBonus = stats.AttackBuffFromPotions;
        if (remainingBonus > 0)
            stats.RemoveAttackBuff(remainingBonus);

        attackBuffRemaining = 0f;
        attackRoutine = null;
    }

    // ---------- DEFENSE POTION ----------
    public void ApplyDefensePotion(int bonusPerPotion, int maxTotalBonus, float duration)
    {
        if (bonusPerPotion <= 0 || duration <= 0f) return;

        int current = stats.DefenseBuffFromPotions;
        int canAdd = Mathf.Clamp(maxTotalBonus - current, 0, bonusPerPotion);
        if (canAdd <= 0) return; // already at cap

        stats.AddDefenseBuff(canAdd);

        defenseBuffRemaining = duration;

        if (defenseRoutine != null)
            StopCoroutine(defenseRoutine);

        defenseRoutine = StartCoroutine(DefenseBuffCoroutine());
    }

    private IEnumerator DefenseBuffCoroutine()
    {
        while (defenseBuffRemaining > 0f)
        {
            defenseBuffRemaining -= Time.deltaTime;
            yield return null;
        }

        int remainingBonus = stats.DefenseBuffFromPotions;
        if (remainingBonus > 0)
            stats.RemoveDefenseBuff(remainingBonus);

        defenseBuffRemaining = 0f;
        defenseRoutine = null;
    }
}
