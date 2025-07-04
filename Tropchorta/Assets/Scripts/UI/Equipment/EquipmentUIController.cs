using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EquipmentUIController : MonoBehaviour
{
    [SerializeField] EquipmentController equipmentController;
    [SerializeField] Image headImage;
    [SerializeField] Image torsoImage;
    [SerializeField] Sprite basicTorsoImage;
    [SerializeField] Image pantsImage;
    [SerializeField] Image shoesImage;
    [SerializeField] Image charmImage;
    [SerializeField] Sprite basicCharmImage;
    [SerializeField] Image weapon1Image;
    [SerializeField] Image weapon2Image;
    [SerializeField] GameObject lastinteractedItemPanel;
    [SerializeField] Image interactedItemImage;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;

    private void Awake()
    {
        if (equipmentController == null)
        {
            equipmentController = GameObject.FindGameObjectWithTag("Equipment").GetComponent<EquipmentController>();
        }
    }

    public void ChangeHeadImage(Item item)
    {
        if (headImage != null && item != null && item.icon != null)
        {
            headImage.sprite = item.icon;
        }
        else if (item == null)
        {
            headImage.sprite = null;
        }
    }

    public void ChangeTorsoImage(Item item)
    {
        if (torsoImage != null && item != null && item.icon != null)
        {
            torsoImage.sprite = item.icon;
        }
        else if (item == null)
        {
            torsoImage.sprite = basicTorsoImage;
        }
    }

    public void ChangePantsImage(Item item)
    {
        if (pantsImage != null && item != null && item.icon != null)
        {
            pantsImage.sprite = item.icon;
        }
        else if (item == null)
        {
            pantsImage.sprite = null;
        }
    }

    public void ChangeShoesImage(Item item)
    {
        if (shoesImage != null && item != null && item.icon != null)
        {
            shoesImage.sprite = item.icon;
        }
        else if (item == null)
        {
            shoesImage.sprite = null;
        }
    }

    public void ChangeWeapon1Image(Item item)
    {
        if (weapon1Image != null && item != null && item.icon != null)
        {
            weapon1Image.sprite = item.icon;
        }
        else if (item == null)
        {
            weapon1Image.sprite = null;
        }
    }

    public void ChangeWeapon2Image(Item item)
    {
        if (weapon2Image != null && item != null && item.icon != null)
        {
            weapon2Image.sprite = item.icon;
        }
        else if (item == null)
        {
            weapon2Image.sprite = null;
        }
    }

    public void ChangeCharmImage(Item item)
    {
        if (charmImage != null && item != null && item.icon != null)
        {
            charmImage.sprite = item.icon;
        }
        else if (item == null)
        {
            charmImage.sprite = basicCharmImage;
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