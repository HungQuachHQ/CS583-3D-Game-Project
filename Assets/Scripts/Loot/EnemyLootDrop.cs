using UnityEngine;

public class EnemyLootDrop : MonoBehaviour
{
    [Header("Loot Configuration")]
    public LootTable lootTable;

    private EnemyHealth enemyHealth;
    private bool hasDropped = false;

    private void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();

        if (enemyHealth == null)
        {
            Debug.LogWarning($"[EnemyLootDrop] No EnemyHealth found on {gameObject.name}");
        }
    }

    private void Update()
    {
        // Check if enemy just died and hasn't dropped loot yet
        if (enemyHealth != null && enemyHealth.isDead && !hasDropped)
        {
            DropLoot();
            hasDropped = true;
        }
    }

    private void DropLoot()
    {
        if (lootTable == null)
        {
            Debug.Log($"[EnemyLootDrop] No loot table assigned to {gameObject.name}");
            return;
        }

        GameObject dropPrefab = lootTable.GetDrop();

        if (dropPrefab != null)
        {
            Vector3 dropPosition = transform.position + Vector3.up * lootTable.dropHeightOffset;
            Instantiate(dropPrefab, dropPosition, Quaternion.identity);
            Debug.Log($"[EnemyLootDrop] {gameObject.name} dropped {dropPrefab.name}");
        }
        else
        {
            Debug.Log($"[EnemyLootDrop] {gameObject.name} dropped nothing");
        }
    }
}