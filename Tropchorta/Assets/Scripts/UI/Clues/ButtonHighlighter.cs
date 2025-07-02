using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Odwo³anie do UI Image, które chcemy w³¹czaæ/wy³¹czaæ
    public Image targetImage;

    // W momencie najechania kursorem na przycisk (button)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.gameObject.SetActive(true);
        }
    }

    // W momencie opuszczenia przycisku przez kursor
    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.gameObject.SetActive(false);
        }
    }
}
