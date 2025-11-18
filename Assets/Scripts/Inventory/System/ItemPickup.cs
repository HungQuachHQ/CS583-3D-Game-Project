using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;

    [Tooltip("If true, item is picked up automatically on trigger enter. Otherwise, require a key press.")]
    public bool autoPickup = false;

    [Tooltip("Key to press to pick up if autoPickup is false.")]
    public KeyCode pickupKey = KeyCode.E;

    private bool playerInRange = false;

    private void Reset()
    {
        //Ensure collider is a trigger
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[ItemPickup] OnTriggerEnter with {other.name}");
        
        if (IsPlayerCollider(other))
        {
            playerInRange = true;
            Debug.Log($"Player entered pickup range for {itemData.displayName}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"[ItemPickup] OnTriggerExit with {other.name}");

        if (IsPlayerCollider(other))
        {
            playerInRange = false;
            Debug.Log($"[ItemPickup] Player left range for {itemData?.displayName}");

        }
    }

    private bool IsPlayerCollider(Collider col)
    {
        //Directly tagged?
        if (col.CompareTag("Player")) return true;

        //Root object tagged player?
        if (col.transform.root != null && col.transform.root.CompareTag("Player")) return true;


        //Parent chain has a playerStats or another 'player' script
        if (col.GetComponentInParent<PlayerStats>() != null)  return true;

        return false;
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (autoPickup)
        {
            TryPickup();
        }
        else
        {
            if (Input.GetKeyDown(pickupKey))
            {
                Debug.Log("[ItemPickup] Pickup key pressed.");
                TryPickup();
            }
        }
    }

    private void TryPickup()
    {
        if (Inventory.Instance == null)
        {
            Debug.LogError("[ItemPickup] itemData is NULL on the pickup!", this);
            return;
        }

        if(Inventory.Instance == null)
        {
            Debug.LogError("[ItemPickup] No Inventory.Instance found in scene!");
        }

        Debug.Log($"[ItemPickup] Trying to add item: {itemData.displayName}");
        bool added = Inventory.Instance.AddItem(itemData);
        if (added)
        {
            Debug.Log($"[ItemPickup] Successfully picked up: {itemData.displayName}");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("[ItemPickup] Could not pick up item. Inventory might be full.");
        }
    }
}
