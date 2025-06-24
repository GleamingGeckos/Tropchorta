using UnityEngine;
using UnityEngine.EventSystems;
public class DragDropItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] Canvas canvas;
    [SerializeField] CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    [SerializeField] int slotId;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalPosition = rectTransform.anchoredPosition;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true; 
        rectTransform.anchoredPosition = originalPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("on pointer down");
    }

    public int GetSlotId()
    {
        return slotId;
    }

    public void SetCanvas(Canvas toSetCanvas)
    {
        canvas = toSetCanvas;
    }
}
