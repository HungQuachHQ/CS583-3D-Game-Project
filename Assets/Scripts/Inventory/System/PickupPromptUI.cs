using UnityEngine;
using TMPro;

public class PickupPromptUI : MonoBehaviour
{
    public static PickupPromptUI Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject root;              // Panel object
    [SerializeField] private TextMeshProUGUI promptText;   // Text component

    // Which pickup currently "owns" the prompt
    private ItemPickup currentSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        HideImmediate();
    }

    public void Show(ItemPickup source, string text)
    {
        currentSource = source;

        if (promptText != null)
            promptText.text = text;

        if (root != null && !root.activeSelf)
            root.SetActive(true);
    }

    /// <summary>
    /// Hide the prompt if it belongs to this source,
    /// or always if source is null.
    /// </summary>
    public void Hide(ItemPickup source = null)
    {
        if (source != null && currentSource != null && source != currentSource)
        {
            // Some other pickup is currently showing the prompt
            return;
        }

        HideImmediate();
    }

    public void HideImmediate()
    {
        currentSource = null;

        if (root != null && root.activeSelf)
            root.SetActive(false);
    }
}
