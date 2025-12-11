using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerProgression : MonoBehaviour
{
    public static PlayerProgression Instance { get; private set; }

    [Header("Current Progress")]
    public int currentLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;

    [Header("Scaling Settings")]
    public int baseXPRequirement = 100;
    public float xpScalingFactor = 1.5f;  // Each level requires more XP

    [Header("Stats Per Level")]
    public int physicalDamagePerLevel = 2;
    public int magicDamagePerLevel = 1;
    public int defensePerLevel = 1;
    public float maxHealthPerLevel = 10f;

    // Track total bonuses from leveling
    public int BonusPhysicalDamage { get; private set; }
    public int BonusMagicDamage { get; private set; }
    public int BonusDefense { get; private set; }
    public float BonusMaxHealth { get; private set; }

    // Event for UI updates
    public event Action<int, int, int> OnXPChanged;  // currentXP, xpToNext, level
    public event Action<int> OnLevelUp;               // newLevel

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        CalculateXPToNextLevel();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Apply level bonuses to the new scene's player
        ApplyBonusesToPlayer();
    }

    public void GainXP(int amount)
    {
        if (amount <= 0) return;

        currentXP += amount;
        Debug.Log($"[PlayerProgression] Gained {amount} XP. Total: {currentXP}/{xpToNextLevel}");

        // Check for level up (can level multiple times at once)
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }

        OnXPChanged?.Invoke(currentXP, xpToNextLevel, currentLevel);
    }

    private void LevelUp()
    {
        currentLevel++;

        // Add stat bonuses
        BonusPhysicalDamage += physicalDamagePerLevel;
        BonusMagicDamage += magicDamagePerLevel;
        BonusDefense += defensePerLevel;
        BonusMaxHealth += maxHealthPerLevel;

        // Recalculate XP needed for next level
        CalculateXPToNextLevel();

        // Apply to current player
        ApplyBonusesToPlayer();

        Debug.Log($"[PlayerProgression] LEVEL UP! Now level {currentLevel}");
        OnLevelUp?.Invoke(currentLevel);
    }

    private void CalculateXPToNextLevel()
    {
        xpToNextLevel = Mathf.RoundToInt(baseXPRequirement * Mathf.Pow(currentLevel, xpScalingFactor));
    }

    private void ApplyBonusesToPlayer()
    {
        // Find PlayerStats in scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        PlayerStats stats = player.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.ApplyLevelBonuses(BonusPhysicalDamage, BonusMagicDamage, BonusDefense);
            Debug.Log($"[PlayerProgression] Applied level bonuses to PlayerStats");
        }

        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.ApplyMaxHealthBonus(BonusMaxHealth);
            Debug.Log($"[PlayerProgression] Applied max health bonus to PlayerHealth");
        }
    }
}