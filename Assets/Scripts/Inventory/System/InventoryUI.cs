using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("Visuals")]
    public Color emptySlotColor = new Color(0.25f, 0.25f, 0.25f, 0.9f);  // dark gray
    public Color filledSlotColor = new Color(0.8f, 0.8f, 0.8f, 0.9f);     // lighter gray


    [Header("References")]
    public GameObject inventoryPanel;   // The panel to show/hide
    public Transform itemsParent;       // The grid or content transform
    public GameObject itemButtonPrefab; // Prefab for each item slot

    [Header("Player")]
    public PlayerStats playerStats;     // Will be auto-bound

    private bool isOpen = false;
    public bool IsOpen => isOpen;

    public void CloseInventory()
    {
        if (isOpen)
        {
            ToggleInventory();
        }
    }

    // One runtime record per slot
    private class SlotUI
    {
        public GameObject obj;
        public Button button;
        public Image background;
        public Image icon;
    }

    private List<SlotUI> slots = new List<SlotUI>();

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        //Safety: hide tooltip whenever inventory UI is disabled
        if(TooltipManager.Instance != null)
        {
            TooltipManager.Instance.HideTooltip();
        }
    }

    private void Start()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        if (Inventory.Instance != null)
        {
            Inventory.Instance.OnInventoryChanged += RefreshUI;
            Debug.Log("[InventoryUI] Subscribed to Inventory.OnInventoryChanged.");
        }
        else
        {
            Debug.LogError("[InventoryUI] No Inventory.Instance at Start.");
        }

        // Build fixed slots once
        BuildSlots();

        // Bind to player
        if (playerStats == null)
        {
            FindPlayerStats();
        }
    }

    private void OnDestroy()
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.OnInventoryChanged -= RefreshUI;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("[InventoryUI] Scene loaded: " + scene.name + ". Rebinding PlayerStats.");
        FindPlayerStats();
    }

    private void FindPlayerStats()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("[InventoryUI] No GameObject with tag 'Player' found in scene.");
            playerStats = null;
            return;
        }

        // Root or children
        playerStats = player.GetComponent<PlayerStats>() ?? player.GetComponentInChildren<PlayerStats>();

        if (playerStats == null)
            Debug.LogWarning("[InventoryUI] Found Player but no PlayerStats component in hierarchy.");
        else
            Debug.Log("[InventoryUI] Bound PlayerStats to: " + playerStats.gameObject.name);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // If we’re trying to OPEN inventory while the game is already paused,
            // just ignore the input. (When inventory is open, isOpen is true,
            // so this guard only applies to the "open" edge.)
            if (!isOpen)
            {
                if (PauseMenuController.GameIsPaused)
                    return;

                // Optional: also block if Bonfire UI is open
                BonfireController bonfire = FindObjectOfType<BonfireController>();
                if (bonfire != null && bonfire.IsBonfireOpen)
                    return;
            }

            ToggleInventory();
        }
    }


    public void ToggleInventory()
    {
        isOpen = !isOpen;
        if (inventoryPanel != null)
            inventoryPanel.SetActive(isOpen);

        Debug.Log("[InventoryUI] Inventory panel " + (isOpen ? "opened" : "closed"));

        if (isOpen)
        {
            //If we're opening inventory, hide pickup prompt so it does not overlap
            if (PickupPromptUI.Instance != null)
            {
                PickupPromptUI.Instance.HideImmediate();
            }
            
            
            // PAUSE GAME when inventory is open
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            RefreshUI();
        }
        else
        {
            // UNPAUSE GAME when inventory is closed
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            //Make sure any tooltip is hidden when inventory closes
            if (TooltipManager.Instance != null)
            {
                TooltipManager.Instance.HideTooltip();
            }

            //Also hide pickup prompt on close just to be safe
            if (PickupPromptUI.Instance != null)
            {
                PickupPromptUI.Instance.HideImmediate();
            }
        }
    }


    /// <summary>
    /// Build a fixed number of slots (maxSlots) once.
    /// </summary>
    private void BuildSlots()
    {
        if (itemsParent == null || itemButtonPrefab == null)
        {
            Debug.LogError("[InventoryUI] itemsParent or itemButtonPrefab is not set!");
            return;
        }

        // Clear any old children
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }
        slots.Clear();

        int slotCount = (Inventory.Instance != null) ? Inventory.Instance.maxSlots : 20;

        for (int i = 0; i < slotCount; i++)
        {
            GameObject slotObj = Instantiate(itemButtonPrefab, itemsParent);
            Button button = slotObj.GetComponent<Button>();
            Image background = slotObj.GetComponent<Image>();

            // 🔹 Tooltip hook: give this slot its index
            SlotTooltipHook hook = slotObj.GetComponentInChildren<SlotTooltipHook>();
            if (hook != null)
            {
                hook.slotIndex = i;
            }
            else
            {
                Debug.LogWarning($"[InventoryUI] No SlotTooltipHook found on slot {i}");
            }

                // Icon = child named "Icon" if it exists
                Transform iconTransform = slotObj.transform.Find("Icon");
            Image iconImage = null;
            if (iconTransform != null)
            {
                iconImage = iconTransform.GetComponent<Image>();
            }
            else
            {
                iconImage = slotObj.GetComponentInChildren<Image>();
            }

            SlotUI slot = new SlotUI
            {
                obj = slotObj,
                button = button,
                background = background,
                icon = iconImage
            };

            slots.Add(slot);
        }

        Debug.Log($"[InventoryUI] Built {slots.Count} inventory slots.");
    }


    private void RefreshUI()
    {
        Debug.Log("[InventoryUI] RefreshUI called.");

        if (Inventory.Instance == null)
        {
            Debug.LogError("[InventoryUI] No Inventory.Instance in RefreshUI.");
            return;
        }

        if (slots.Count == 0)
        {
            BuildSlots();
            if (slots.Count == 0) return;
        }

        List<ItemData> items = Inventory.Instance.items;

        // 1) Clear all slots to "empty" visuals
        for (int i = 0; i < slots.Count; i++)
        {
            SlotUI slot = slots[i];

            slot.button.onClick.RemoveAllListeners();
            slot.button.interactable = false;

            if (slot.icon != null)
            {
                slot.icon.sprite = null;                // no icon when empty
                slot.icon.enabled = false;
            }


            if (slot.background != null)
                slot.background.color = emptySlotColor; // dark box for empty
        }

        // 2) Fill first N slots with items
        for (int i = 0; i < items.Count && i < slots.Count; i++)
        {
            ItemData item = items[i];
            SlotUI slot = slots[i];

            if (slot.icon != null && item.icon != null)
            {
                slot.icon.sprite = item.icon;           // show icon
                slot.icon.enabled = true;
            }


            if (slot.background != null)
                slot.background.color = filledSlotColor; // lighter to show "filled"

            slot.button.interactable = true;

            // capture for closure
            ItemData capturedItem = item;
            slot.button.onClick.AddListener(() => OnItemClicked(capturedItem));
        }
    }


    private void OnItemClicked(ItemData item)
    {
        if (playerStats == null)
        {
            Debug.LogWarning("[InventoryUI] No PlayerStats bound. Cannot equip.");
            return;
        }

        if (item.itemType == ItemType.Weapon)
        {
            ItemData currentlyEquipped = playerStats.GetEquippedWeapon();
            if (currentlyEquipped == item)
                playerStats.UnequipWeapon();
            else
                playerStats.EquipWeapon(item);
        }

        Debug.Log("Clicked on item: " + item.displayName);
    }
}
