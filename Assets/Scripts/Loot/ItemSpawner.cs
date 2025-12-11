using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableItem
    {
        public GameObject itemPrefab;
        [Range(0f, 100f)]
        public float spawnWeight = 25f;  // Higher = more likely
    }

    [Header("Items to Spawn")]
    public List<SpawnableItem> possibleItems = new List<SpawnableItem>();

    [Header("Spawn Settings")]
    public bool spawnOnStart = true;
    public float spawnDelay = 0f;              // Delay before first spawn
    public float spawnHeightOffset = 0.5f;     // Height above spawner position

    [Header("Respawn Settings")]
    public bool respawnAfterPickup = false;
    public float respawnTime = 30f;            // Seconds until respawn

    [Header("Visual (Optional)")]
    public GameObject spawnEffect;             // Particle effect on spawn
    public bool showSpawnPoint = true;         // Show gizmo in editor

    private GameObject currentSpawnedItem;
    private bool hasSpawned = false;

    private void Start()
    {
        if (spawnOnStart)
        {
            if (spawnDelay > 0)
            {
                Invoke(nameof(SpawnRandomItem), spawnDelay);
            }
            else
            {
                SpawnRandomItem();
            }
        }
    }

    private void Update()
    {
        // Check if item was picked up and we should respawn
        if (respawnAfterPickup && hasSpawned && currentSpawnedItem == null)
        {
            hasSpawned = false;
            Invoke(nameof(SpawnRandomItem), respawnTime);
        }
    }

    public void SpawnRandomItem()
    {
        if (possibleItems.Count == 0)
        {
            Debug.LogWarning($"[ItemSpawner] No items configured on {gameObject.name}");
            return;
        }

        // Calculate total weight
        float totalWeight = 0f;
        foreach (var item in possibleItems)
        {
            if (item.itemPrefab != null)
            {
                totalWeight += item.spawnWeight;
            }
        }

        if (totalWeight <= 0)
        {
            Debug.LogWarning($"[ItemSpawner] Total weight is 0 on {gameObject.name}");
            return;
        }

        // Pick random item based on weight
        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;
        GameObject selectedPrefab = null;

        foreach (var item in possibleItems)
        {
            if (item.itemPrefab == null) continue;

            currentWeight += item.spawnWeight;
            if (randomValue <= currentWeight)
            {
                selectedPrefab = item.itemPrefab;
                break;
            }
        }

        // Spawn the item
        if (selectedPrefab != null)
        {
            Vector3 spawnPosition = transform.position + Vector3.up * spawnHeightOffset;
            currentSpawnedItem = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
            hasSpawned = true;

            Debug.Log($"[ItemSpawner] Spawned {selectedPrefab.name} at {gameObject.name}");

            // Play spawn effect
            if (spawnEffect != null)
            {
                Instantiate(spawnEffect, spawnPosition, Quaternion.identity);
            }
        }
    }

    // Manual spawn trigger (can be called by other scripts or events)
    public void TriggerSpawn()
    {
        SpawnRandomItem();
    }

    // Visualize spawn point in editor
    private void OnDrawGizmos()
    {
        if (!showSpawnPoint) return;

        Gizmos.color = Color.cyan;
        Vector3 spawnPos = transform.position + Vector3.up * spawnHeightOffset;
        Gizmos.DrawWireSphere(spawnPos, 0.3f);
        Gizmos.DrawLine(transform.position, spawnPos);

        // Draw icon
        Gizmos.DrawIcon(spawnPos, "d_Prefab Icon", true);
    }
}