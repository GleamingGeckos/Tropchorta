using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ImageCycler : MonoBehaviour
{
    [SerializeField] private Image targetImage;          // Obrazek, który zmieniamy
    [SerializeField] private List<Sprite> sprites;       // Lista obrazków do prze³¹czania
    private int currentIndex = 0;

    public void CycleImage()
    {
        if (sprites == null || sprites.Count == 0 || targetImage == null)
            return;

        // Ustaw sprite z listy
        targetImage.sprite = sprites[currentIndex];

        // Przesuñ indeks do przodu
        currentIndex = (currentIndex + 1) % sprites.Count; // zapêtla siê
    }
}
