using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject tooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }
}
