using UnityEngine;

public class LockedWall : MonoBehaviour
{
    [Header("Required Item")]
    public ItemData requiredItem;

    [Header("Interaction")]
    public float interactionRange = 3f;
    public KeyCode interactKey = KeyCode.F;

    [Header("UI Prompt")]
    public GameObject promptUI;           // Optional: "Press F to use Ancient Stone"
    public string promptMessage = "Press F to use";

    [Header("Effects (Optional)")]
    public GameObject disappearEffect;    // Particle effect when wall vanishes
    public AudioClip disappearSound;

    private bool playerInRange = false;
    private Transform playerTransform;
    private bool isUnlocked = false;

    private void Start()
    {
        if (promptUI != null)
        {
            promptUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (isUnlocked) return;

        // Check if player is in range
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            playerInRange = distance <= interactionRange;
            playerTransform = player.transform;
        }

        // Show/hide prompt based on whether player has the item
        if (playerInRange && HasRequiredItem())
        {
            ShowPrompt();

            if (Input.GetKeyDown(interactKey))
            {
                UnlockWall();
            }
        }
        else
        {
            HidePrompt();
        }
    }

    private bool HasRequiredItem()
    {
        if (Inventory.Instance == null || requiredItem == null)
            return false;

        return Inventory.Instance.items.Contains(requiredItem);
    }

    private void UnlockWall()
    {
        isUnlocked = true;

        // Remove item from inventory
        if (Inventory.Instance != null && requiredItem != null)
        {
            Inventory.Instance.RemoveItem(requiredItem);
            Debug.Log($"[LockedWall] Used {requiredItem.displayName} to unlock wall");
        }

        // Play effects
        if (disappearEffect != null)
        {
            Instantiate(disappearEffect, transform.position, Quaternion.identity);
        }

        if (disappearSound != null)
        {
            AudioSource.PlayClipAtPoint(disappearSound, transform.position);
        }

        // Hide prompt
        HidePrompt();

        // Destroy the wall
        Destroy(gameObject);
    }

    private void ShowPrompt()
    {
        if (promptUI != null && !promptUI.activeSelf)
        {
            promptUI.SetActive(true);
        }
    }

    private void HidePrompt()
    {
        if (promptUI != null && promptUI.activeSelf)
        {
            promptUI.SetActive(false);
        }
    }

    // Visualize interaction range in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}