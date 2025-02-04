using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] RectTransform fill;
    float initialWidth;
    Vector2 initialPosition;

    void Start()
    {
        initialWidth = fill.sizeDelta.x;
        initialPosition = fill.anchoredPosition;
    }

    // this expects normalized value (0-1)
    public void SetHealth(float health)
    {
        fill.sizeDelta = new Vector2(initialWidth * health, fill.sizeDelta.y);
        fill.anchoredPosition = new Vector2(initialPosition.x - (initialWidth - fill.sizeDelta.x) / 2, initialPosition.y);
    }
}
