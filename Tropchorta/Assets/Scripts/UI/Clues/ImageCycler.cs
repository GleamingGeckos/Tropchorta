using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class ImageCycler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("G³ówna konfiguracja")]
    [SerializeField] private Image targetImage;
    [SerializeField] private List<Sprite> sprites;

    public int currentIndex { get; private set; }
    private Vector3 originalScale;

    // Event informuj¹cy o zmianie indeksu
    public event Action<ImageCycler, int> OnIndexChanged;

    private void Awake()
    {
        if (targetImage != null)
            originalScale = targetImage.rectTransform.localScale;

        currentIndex = 0;
    }

    public void CycleImage()
    {
        if (sprites == null || sprites.Count == 0 || targetImage == null)
            return;

        currentIndex = (currentIndex + 1) % sprites.Count;
        targetImage.sprite = sprites[currentIndex];

        // Wywo³aj event
        OnIndexChanged?.Invoke(this, currentIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.rectTransform.localScale = originalScale * 1.1f;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.rectTransform.localScale = originalScale;
        }
    }
}
