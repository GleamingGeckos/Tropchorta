using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ImageCycler : MonoBehaviour
{
    [SerializeField] private Image targetImage;          // Obrazek, kt�ry zmieniamy
    [SerializeField] private List<Sprite> sprites;       // Lista obrazk�w do prze��czania
    private int currentIndex = 0;

    public void CycleImage()
    {
        if (sprites == null || sprites.Count == 0 || targetImage == null)
            return;

        // Ustaw sprite z listy
        targetImage.sprite = sprites[currentIndex];

        // Przesu� indeks do przodu
        currentIndex = (currentIndex + 1) % sprites.Count; // zap�tla si�
    }
}
