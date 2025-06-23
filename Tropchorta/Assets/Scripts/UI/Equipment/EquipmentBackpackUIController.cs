using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class EquipmentBackpackUIController : MonoBehaviour
{
    [SerializeField] EquipmentController equipmentController;
    [SerializeField] Image headImage;
    [SerializeField] Image torsoImage;
    [SerializeField] Image pantsImage;
    [SerializeField] Image shoesImage;
    [SerializeField] Image weapon1Image;
    [SerializeField] Image weapon2Image;
    [SerializeField] Image charmImage;
    [SerializeField] List<Image> backpackImages;

    [SerializeField] GameObject itemHighlightPanel;
    public Image highlightedItemImage;
    public TextMeshProUGUI highlightedItemNameText;
    public TextMeshProUGUI highlightedItemDescriptionText;

    public void RefreshAllImages(Item weapon1, Item weapon2, Item head, Item torso, Item pants, Item shoes, List<Item> addditional)
    {
        ChangeWeapon1Image(weapon1);
        ChangeWeapon2Image(weapon2);
        ChangeHeadImage(head);
        ChangeTorsoImage(torso);
        ChangePantsImage(pants);
        ChangeShoesImage(shoes);
        ChangeBackpackImages(addditional);
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
            torsoImage.sprite = null;
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
        else if (item == null && charmImage != null)
        {
            charmImage.sprite = null;
        }
    }

    public void ChangeBackpackImages(List<Item> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i] != null)
            {
                backpackImages[i].sprite = items[i].icon;
            }
            else
            {
                backpackImages[i].sprite = null;
            }
        }
    }

    public void HighlightItem(Item item)
    {
        if (itemHighlightPanel != null && item != null)
        {
            itemHighlightPanel.SetActive(true);
            if (highlightedItemImage != null && item.icon != null)
            {
                highlightedItemImage.sprite = item.icon;
            }
            if (highlightedItemNameText != null && item.name != null)
            {
                highlightedItemNameText.text = item.name;
            }
            if (highlightedItemDescriptionText != null && item.description != null)
            {
                highlightedItemDescriptionText.text = item.description;
            }
        }
    }
    public void HideHighlightItem()
    {
        itemHighlightPanel.SetActive(false);
    }

    public void SwitchItems(int firstSlotId, int secondSlotId)
    {
        equipmentController.SwitchAdditionalItem(firstSlotId,secondSlotId);
    }
}
