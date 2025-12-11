using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }

    public RectTransform tooltipPanel;
    public TextMeshProUGUI tooltipText;

    private Canvas canvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Debug.Log("[TooltipManager] Awake. Instance set. ");
    }

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        Debug.Log("[TooltipManager] Start. Canvas = " + (canvas != null ? canvas.name : "null"));
        HideTooltip();
    }

    private void Update()
    {
        if (tooltipPanel != null && tooltipPanel.gameObject.activeSelf)
        {
            FollowMouse();
        }
    }

    public void ShowTooltipForSlot(int index)
    {
        if (Inventory.Instance == null)
        {
            HideTooltip();
            return;
        }

        var items = Inventory.Instance.items;
        Debug.Log($"[TooltipManager] ShowTooltipForSlot index={index}, items ={items.Count}");

        if (index < 0 || index >= items.Count)
        {
            Debug.Log("[TooltipManager] Slot is empty, hiding tooltip.");
            // Empty slot -> hide tooltip (or show "(Empty Slot)" if you prefer)
            HideTooltip();
            return;
        }

        ItemData item = items[index];
        string text = ItemTooltipFormatter.Format(item);
        ShowTooltip(text);
    }

    public void ShowTooltip(string text)
    {
        if (tooltipPanel == null || tooltipText == null) 
        {
            Debug.LogWarning("[TooltipManager] ShowTooltip called but tooltipPanel or tooltipText is null!");
            return;
        }
        tooltipText.text = text;

        Debug.Log("[TooltipManager] Before SetActive(ture), activeSelf = " + tooltipPanel.gameObject.activeSelf);
        tooltipPanel.gameObject.SetActive(true);
        Debug.Log("[TooltipManager] After SetActive(true), activeSelf = " + tooltipPanel.gameObject.activeSelf);
        FollowMouse();
    }

    public void HideTooltip()
    {
        if (tooltipPanel == null) return;

        Debug.Log("[TooltipManager] Before SetActive(false), activeSelf = " + tooltipPanel.gameObject.activeSelf);
        tooltipPanel.gameObject.SetActive(false);
        Debug.Log("[TooltipManager] After SetActive(false), activeSelf =" + tooltipPanel.gameObject.activeSelf);
    }

    private void FollowMouse()
    {
        if (canvas == null || tooltipPanel == null) return;

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera, out localPos);

        //PLace tooltip a bit to the right and below the cursor
        tooltipPanel.anchoredPosition = localPos + new Vector2(24f, -24f);
    }
}
