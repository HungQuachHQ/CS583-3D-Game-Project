using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    public GameObject inventoryPanel;   //The panel to show/hide
    public Transform itemsParent;       //The grid or content transform
    public GameObject itemButtonPrefab; //Prefab for each item slot
    public PlayerStats playerStats;     //Reference to player stats

    private bool isOpen = false;
    private List<GameObject> spawnedButtons = new List<GameObject>();



    // Start is called before the first frame update
    private void Start()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }

        if (Inventory.Instance != null)
        {
            Inventory.Instance.OnInventoryChanged += RefreshUI;
            Debug.Log("[InventoryUI] Subscribed to Inventory.OnInventoryChanged.");

        }
        else
        {
            Debug.LogError("[InventoryUI] No Inventory. Instance at Start.");
        }
    }

    private void OnDestory()
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.OnInventoryChanged -= RefreshUI;
        }
    }



    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(isOpen);
        }

        Debug.Log("[InventoryUI] Inventory panel " + (isOpen ? "opened" : "closed"));

        if (isOpen)
        {
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        Debug.Log("[InventoryUI] RefreshUI called.");
        
        //Clear old buttons
        foreach (var btn in spawnedButtons)
        {
            Destroy(btn);
        }
        spawnedButtons.Clear();

        if (Inventory.Instance == null)
        {
            Debug.LogError("[InventoryUI] No Inventory.Istance in RefreshUI.");
            return;
        }

        if(itemsParent == null || itemButtonPrefab == null)
        {
            Debug.LogError("[InventoryUI] itemsParent or itemButtonPrefab is not set!");
            return;
        }

        foreach(var item in Inventory.Instance.items)
        {
            Debug.Log("[InventoryUI] Create button for: " + item.displayName);
            
            GameObject newButtonObj = Instantiate(itemButtonPrefab, itemsParent);
            spawnedButtons.Add(newButtonObj);

            Button button = newButtonObj.GetComponent<Button>();
            Image iconImage = newButtonObj.GetComponentInChildren<Image>();

            if (iconImage != null && item.icon != null) 
            {
                iconImage.sprite = item.icon;
            }

            //Capture local variable for closure
            ItemData capturedItem = item;

            button.onClick.AddListener(() => OnItemClicked(capturedItem));
        }
    }

    private void OnItemClicked(ItemData item)
    {
        //For now: If it's a weapon, equp or unequip
        if(item.itemType == ItemType.Weapon)
        {
            ItemData currentlyEquipped = playerStats.GetEquippedWeapon();
            if(currentlyEquipped == item)
            {
                playerStats.UnequipWeapon();
            }
            else
            {
                playerStats.EquipWeapon(item);
            }
        }

        //Later: can add logic for puzzle items, consumables
        Debug.Log("Clicked on item: " + item.displayName);
    }
}
