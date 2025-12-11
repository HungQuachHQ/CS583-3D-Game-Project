using UnityEngine;
using UnityEngine.EventSystems;

public class SlotTooltipHook : MonoBehaviour,
                               IPointerEnterHandler,
                               IPointerExitHandler,
                               IPointerClickHandler    //  new
{
    [HideInInspector]
    public int slotIndex;   // set by InventoryUI.BuildSlots

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (TooltipManager.Instance != null)
        {
            TooltipManager.Instance.ShowTooltipForSlot(slotIndex);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (TooltipManager.Instance != null)
        {
            TooltipManager.Instance.HideTooltip();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Inventory.Instance != null)
            {
                Inventory.Instance.UseItem(slotIndex);
            }
        }
    }
}
