using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Odwo�anie do UI Image, kt�re chcemy w��cza�/wy��cza�
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
