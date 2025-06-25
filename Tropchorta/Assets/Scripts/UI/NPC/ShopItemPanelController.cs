using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopItemPanelController : MonoBehaviour
{
    [SerializeField] private Item displayedItem;
    [SerializeField] private Image itemImage;
    //[SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI priceText;


    public void LoadItemData(Item item)
    {
        displayedItem = item;
        itemImage.sprite = item.shopPanel;
        //itemImage.sprite = item.icon;
        //nameText.text = item.itemName;
        descriptionText.text = item.description;
        priceText.text = item.itemPrice.ToString();
    }
}
