using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapIcon : MonoBehaviour, IPointerClickHandler
{
    private Image iconImage;
    private RectTransform iconRectTransform;
    private Vector2 iconPositionNormalized;
    private int id;
    public UnityEvent<int> onDestroyed = new UnityEvent<int>();
    [SerializeField] private bool destroyable = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        iconImage = GetComponent<Image>();
        iconRectTransform = GetComponent<RectTransform>();
    }

    public void SetSprite(Sprite sprite)
    {
        iconImage.sprite = sprite;
    }

    public void SetPosition(Vector2 positionNormalized)
    {
        iconPositionNormalized = positionNormalized;
        iconRectTransform.anchorMin = positionNormalized;
        iconRectTransform.anchorMax = positionNormalized;
    }

    public void SetIndex(int index)
    {
        id = index;
    }

    public void SetSize(float size)
    {
        iconRectTransform.sizeDelta = new Vector2(size, size);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!destroyable) return;
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            onDestroyed.Invoke(id);
            Destroy(gameObject);
        }
    }
}
