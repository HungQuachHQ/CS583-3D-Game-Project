//Attach to empty GameObject in scene / GameManager

using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [Tooltip("Max number of item slots in the inventory.")]
    public int maxSlots = 20;

    public List<ItemData> items = new List<ItemData>();

    public event Action OnInventoryChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[Invenotry] Multiple Inventory instances found. Destorying duplicate.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Debug.Log("[Inventory] Inventory singelton initialized.");
    }

    public bool AddItem(ItemData item)
    {
        if(item == null)
        {
            Debug.LogError("[Inventory] Tried to add null item.");
            return false;
        }
        
        if(items.Count >= maxSlots)
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
}
