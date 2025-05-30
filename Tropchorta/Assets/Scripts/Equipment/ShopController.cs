using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [SerializeField] private EquipmentController equipmentController;

    [Header("Items Display")]
    [SerializeField] private ShopUIController shopUIController;

    [Header("Items")]
    [SerializeField] private List<Item> items;

    private bool isInitialized = false;


    private void Start()
    {
        InicializeItemsDisplay();
        isInitialized = true;
    }

    void InicializeItemsDisplay()
    {
        foreach (Item item in items)
        {
            shopUIController.AddItemPanel(item);
        }
    }

    public bool BuyItem(int itemId)
    {
        if(items[itemId].itemPrice <= equipmentController.GetGold())
        {
            equipmentController.RemoveGold(items[itemId].itemPrice);
            Item tmpItem = equipmentController.SwitchItem(items[itemId]);
            if(tmpItem != null)
            {
                items[itemId] = tmpItem;
                shopUIController.AddItemPanel(tmpItem);
            }
            else
            {
                items.Remove(items[itemId]);
            }
            return true;//if you can buy item
        }
        else
        {
            Debug.Log("Not enough gold for this item");
            return false;//if you cant buy this item
        }

    }
}
