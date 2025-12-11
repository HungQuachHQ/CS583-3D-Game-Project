using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLootTable", menuName = "DungeonCrawler/LootTable")]
public class LootTable : ScriptableObject
{
    [System.Serializable]
    public class LootEntry
    {
        public GameObject itemPrefab;      // The world pickup prefab (e.g., HealthPotion_World)
        [Range(0f, 100f)]
        public float dropChance = 10f;     // Percentage chance to drop
    }

    [Header("Loot Settings")]
    [Range(0f, 100f)]
    public float chanceToDropAnything = 50f;  // Overall chance something drops

    public List<LootEntry> possibleDrops = new List<LootEntry>();

    [Header("Drop Positioning")]
    public float dropHeightOffset = 0.5f;     // How high above death point to spawn

    /// <summary>
    /// Rolls for a drop and returns the prefab to spawn (or null if nothing drops)
    /// </summary>
    public GameObject GetDrop()
    {
        // First check if anything drops at all
        if (Random.Range(0f, 100f) > chanceToDropAnything)
            return null;

        // Build weighted list of items that passed their individual rolls
        List<GameObject> possibleItems = new List<GameObject>();

        foreach (var entry in possibleDrops)
        {
            if (entry.itemPrefab != null && Random.Range(0f, 100f) <= entry.dropChance)
            {
                possibleItems.Add(entry.itemPrefab);
            }
        }

        // Pick one randomly from those that passed
        if (possibleItems.Count > 0)
        {
            return possibleItems[Random.Range(0, possibleItems.Count)];
        }

        return null;
    }
}