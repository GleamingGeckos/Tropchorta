using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EquipmentUIController : MonoBehaviour
{
    public EquipmentController equipmentController;
    public Image headImage;
    public Image torsoImage;
    public Image pantsImage;
    public Image shoesImage;
    public Image weapon1Image;
    public Image weapon2Image;

    public GameObject lastinteractedItemPanel;
    public Image interactedItemImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public void ChangeHeadImage(Item item)
    {
        if (headImage != null && item != null && item.icon != null)
        {
            headImage.sprite = item.icon;
        }
    }

    public void ChangeTorsoImage(Item item)
    {
        if (torsoImage != null && item != null && item.icon != null)
        {
            torsoImage.sprite = item.icon;
        }
    }

    public void ChangePantsImage(Item item)
    {
        if (pantsImage != null && item != null && item.icon != null)
        {
            pantsImage.sprite = item.icon;
        }
    }

    public void ChangeShoesImage(Item item)
    {
        if (shoesImage != null && item != null && item.icon != null)
        {
            shoesImage.sprite = item.icon;
        }
    }

    public void ChangeWeapon1Image(Item item)
    {
        if (weapon1Image != null && item != null && item.icon != null)
        {
            weapon1Image.sprite = item.icon;
        }
    }

    public void ChangeWeapon2Image(Item item)
    {
        if (weapon2Image != null && item != null && item.icon != null)
        {
            weapon2Image.sprite = item.icon;
        }
    }

    public void ChangeLastInteractedItemDisplay(Item item)
    {
        if (lastinteractedItemPanel != null && item != null)
        {
            lastinteractedItemPanel.SetActive(true);
            if (interactedItemImage != null && item.icon != null)
            {
                interactedItemImage.sprite = item.icon;
            }
            if (nameText != null && item.name != null)
            {
                nameText.text = item.name;
            }
            if (descriptionText != null && item.description != null)
            {
                descriptionText.text = item.description;
            }
        }
    }
    public void HideLastInteractedItemDisplay()
    {
        lastinteractedItemPanel.SetActive(false);
    }
}