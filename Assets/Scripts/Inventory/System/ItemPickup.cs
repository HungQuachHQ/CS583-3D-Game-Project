using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;

    [Tooltip("If true, item is picked up automatically on trigger enter. Otherwise, require a key press.")]
    public bool autoPickup = false;

    [Tooltip("Key to press to pick up if autoPickup is false.")]
    public KeyCode pickupKey = KeyCode.E;

    [Header("Sound Effects")]
    public AudioClip pickupSFX;
    [Range(0f, 1f)]
    public float pickupVolume = 1f;

    private bool playerInRange = false;

    private void Reset()
    {
        // Ensure collider is a trigger
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[ItemPickup] OnTriggerEnter with {other.name}");

        if (IsPlayerCollider(other))
        {
            playerInRange = true;
            Debug.Log($"Player entered pickup range for {itemData?.displayName}");

            // Only show prompt if manual pickup
            if (!autoPickup && PickupPromptUI.Instance != null)
            {
                string label = itemData != null ? itemData.displayName : "item";
                string keyName = pickupKey.ToString();   // e.g. "E"

                PickupPromptUI.Instance.Show(this, $"Press {keyName} to pick up {label}");
            }

            if (autoPickup)
            {
                TryPickup();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"[ItemPickup] OnTriggerExit with {other.name}");

        if (IsPlayerCollider(other))
        {
            playerInRange = false;
            Debug.Log($"[ItemPickup] Player left range for {itemData?.displayName}");

            if (PickupPromptUI.Instance != null)
            {
                // Only hide if this pickup owns the prompt
                PickupPromptUI.Instance.Hide(this);
            }
        }
    }

    private bool IsPlayerCollider(Collider col)
    {
        if (col.CompareTag("Player")) return true;
        if (col.transform.root != null && col.transform.root.CompareTag("Player")) return true;
        if (col.GetComponentInParent<PlayerStats>() != null) return true;

        return false;
    }

    private void Update()
    {
        if (!playerInRange) return;
        if (autoPickup) return;

        if (Input.GetKeyDown(pickupKey))
        {
            Debug.Log("[ItemPickup] Pickup key pressed.");
            TryPickup();
        }
    }

    private void TryPickup()
    {
        if (itemData == null)
        {
            Debug.LogError("[ItemPickup] itemData is NULL on the pickup!", this);
            return;
        }

        if (Inventory.Instance == null)
        {
            Debug.LogError("[ItemPickup] No Inventory.Instance found in scene!");
            return;
        }

        Debug.Log($"[ItemPickup] Trying to add item: {itemData.displayName}");
        bool added = Inventory.Instance.AddItem(itemData);

        if (added)
        {
            Debug.Log($"[ItemPickup] Successfully picked up: {itemData.displayName}");

            // Play pickup sound
            PlayPickupSound();

            // Hide prompt if we were showing one
            if (PickupPromptUI.Instance != null)
            {
                PickupPromptUI.Instance.Hide(this);
            }

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("[ItemPickup] Could not pick up item. Inventory might be full.");
        }
    }

    private void PlayPickupSound()
    {
        if (pickupSFX != null)
        {
            // PlayClipAtPoint survives this object's destruction
            AudioSource.PlayClipAtPoint(pickupSFX, transform.position, pickupVolume);
        }
    }
}