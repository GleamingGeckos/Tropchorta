using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ImageCycler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("G³ówna konfiguracja")]
    [SerializeField] private Image targetImage;            // Obrazek, którego sprite zmieniamy
    [SerializeField] private List<Sprite> sprites;         // Lista sprite'ów     


    public int currentIndex;
    private Vector3 originalScale;

    private void Awake()
    {
        if (targetImage != null)
            originalScale = targetImage.rectTransform.localScale;
        currentIndex = 0;
    }

    // Wywo³ywane z OnClick() przycisku
    public void CycleImage()
    {
        if (sprites == null || sprites.Count == 0 || targetImage == null)
            return;

        currentIndex = (currentIndex + 1) % sprites.Count;
        targetImage.sprite = sprites[currentIndex];
    }

    // Najechanie kursorem
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.rectTransform.localScale = originalScale * 1.1f;
        }
    }

    // Zjechanie kursora
    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.rectTransform.localScale = originalScale;
        }
    }
}
