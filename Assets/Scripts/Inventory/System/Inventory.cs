using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [Tooltip("Max number of item slots in the inventory.")]
    public int maxSlots = 20;

    // Each entry is one item (we'll add stacking later if needed)
    public List<ItemData> items = new List<ItemData>();

    public event Action OnInventoryChanged;

    private void Awake()
    {
        Debug.Log($"[Inventory] Awake on {gameObject.name}. Current Instance = {Instance}");

        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"[Inventory] Duplicate Inventory on {gameObject.name}, destroying this one. Existing Instance is on {Instance.gameObject.name}.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log($"[Inventory] Inventory singleton initialized on {gameObject.name} (persistent).");
    }

    public bool AddItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogError("[Inventory] Tried to add null item.");
            return false;
        }

        if (items.Count >= maxSlots)
        {
            Debug.Log("[Inventory] Inventory full! Cannot add: " + item.displayName);
            return false;
        }

        items.Add(item);
        Debug.Log($"[Inventory] Added item: {item.displayName}. Total items: {items.Count}");
        OnInventoryChanged?.Invoke();
        return true;
    }

    public void RemoveItem(ItemData item)
    {
        if (items.Remove(item))
        {
            Debug.Log($"[Inventory] Removed item: {item.displayName}. Total items: {items.Count}");
            OnInventoryChanged?.Invoke();
        }
    }

    // --------------------------------------------------------
    // 👇 NEW: Use/consume an item by slot index (for potions)
    // --------------------------------------------------------
    public void UseItem(int index)
    {
        if (index < 0 || index >= items.Count)
        {
            Debug.LogWarning($"[Inventory] UseItem called with invalid index {index}");
            return;
        }

        ItemData item = items[index];
        if (item == null)
        {
            Debug.LogWarning($"[Inventory] UseItem index {index} is null.");
            return;
        }

        // Right now we only "use" consumables (potions)
        if (item.itemType != ItemType.Consumable)
        {
            Debug.Log($"[Inventory] UseItem called on non-consumable: {item.displayName}");
            return;
        }

        Debug.Log($"[Inventory] Using item in slot {index}: {item.displayName}");
        ApplyConsumable(item);

        // For now: each slot = one potion → remove slot after use
        items.RemoveAt(index);
        OnInventoryChanged?.Invoke();
    }

    // Handles applying the effect of the consumable to the player
    private void ApplyConsumable(ItemData item)
    {
        // Find the Player and its buff controller
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("[Inventory] No Player found to apply consumable.");
            return;
        }

        PlayerBuffController buffs = player.GetComponent<PlayerBuffController>();
        if (buffs == null)
        {
            Debug.LogWarning("[Inventory] Player has no PlayerBuffController component.");
            return;
        }

        switch (item.consumableType)
        {
            case ConsumableType.HealthPotion:
                buffs.ApplyHealthPotion(item.healFraction);
                break;

            case ConsumableType.StaminaPotion:
                buffs.ApplyStaminaPotion(
                    item.speedMultiplier,
                    item.buffDurationSeconds);
                break;

            case ConsumableType.AttackPotion:
                buffs.ApplyAttackPotion(
                    item.attackBonusPerPotion,
                    item.maxAttackBonusFromPotions,
                    item.buffDurationSeconds);
                break;

            case ConsumableType.DefensePotion:
                buffs.ApplyDefensePotion(
                    item.defenseBonusPerPotion,
                    item.maxDefenseBonusFromPotions,
                    item.buffDurationSeconds);
                break;

            default:
                Debug.LogWarning($"[Inventory] Consumable type {item.consumableType} not handled.");
                break;
        }
    }
}
