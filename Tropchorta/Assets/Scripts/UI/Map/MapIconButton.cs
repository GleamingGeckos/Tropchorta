using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// this *could* be later generalized to a more generic button class
public class MapIconButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image borderImage;
    [SerializeField] private Color backgroundDefaultColor;
    [SerializeField] private Color backgroundSelectedColor;
    [SerializeField] private Color borderColor;
    private bool selected = false;

    public UnityEvent onClick;

    private FullscreenMapController fullscreenMapController;

    private void Start()
    {
        fullscreenMapController = FindFirstObjectByType<FullscreenMapController>();
        backgroundImage.color = backgroundDefaultColor;
        borderImage.color = borderColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        fullscreenMapController.OnIconButtonClicked(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        backgroundImage.DOColor(backgroundSelectedColor, 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backgroundImage.DOColor(backgroundDefaultColor, 0.2f);
    }

    public void SetSelected(bool selected)
    {
        this.selected = selected;
        if (!selected)
        {
            backgroundImage.color = backgroundDefaultColor;
            borderImage.DOFade(0f, 0f);
        }
        else
        {
            backgroundImage.color = backgroundSelectedColor;
            borderImage.DOFade(1f, 0f);
        }
    }

    public Sprite GetIconSprite()
    {
        return iconImage.sprite;
    }

    public bool IsSelected()
    {
        return selected;
    }
}
