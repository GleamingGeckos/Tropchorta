using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [Header("Other Controllers")]
    [SerializeField] public EquipmentController equipmentController;
    [SerializeField] private PlayerUIController playerUIController;

    [Header("Items Display")]
    [SerializeField] private ShopUIController shopUIController;

    [Header("Items")]
    [SerializeField] private List<Item> items;

    //private bool isInitialized = false;

    [SerializeField] private bool isShopSoldOut = false;
    [SerializeField] private bool isPlayerClose = false;
    [SerializeField] private bool isShopOpened = false;
    private void Start()
    {
        if(equipmentController == null)
        {
            equipmentController = GameObject.FindGameObjectWithTag("Equipment").GetComponent<EquipmentController>();
        }
        if (playerUIController == null)
        {
            playerUIController = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<PlayerUIController>();
        }
        InicializeItemsDisplay();
        //isInitialized = true;
    }

    void Update()
    {
        if (isPlayerClose && !isShopSoldOut)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(isShopOpened)
                {
                    StopShop();
                }
                else
                {
                    StartShop();
                }
            }
        }
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
            if (items[itemId].dropOnBuy)
            {
                Instantiate(items[itemId].itemPrefab, transform.position + Vector3.up + transform.forward, Quaternion.identity);
                return true;
            }
            Item tmpItem = equipmentController.SwitchItem(items[itemId]);
            if(tmpItem != null)
            {
                items[itemId] = tmpItem;
                shopUIController.AddItemPanel(tmpItem);
            }
            else
            {
                items.Remove(items[itemId]);
                if(items.Count <= 0)
                {
                    SoldOutShop();
                }
            }

            return true;//if you can buy item
        }
        else
        {
            Debug.Log("Not enough gold for this item");
            return false;//if you cant buy this item
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerClose = true;
            if(!isShopSoldOut)
            {
                shopUIController.ShowPressDisplay();
            }
            else
            {
                shopUIController.ShowSoldOutDisplay();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerClose = false;
            shopUIController.HidePressDisplay();
            shopUIController.HideSoldOutDisplay();
            StopShop(); // just in case he somehow leaves without pressing E?
        }
    }

    void StartShop()
    {
        if(!isShopOpened)
        {
            isShopOpened = true;
            shopUIController.ShowShopPanel();
            playerUIController.TurnOffPlayerUI();
            PauseController.SetPause(true);
            PauseController.DisableInput(true);
        }
    }

    void StopShop()
    {
        if (isShopOpened)
        {
            isShopOpened = false; 
            shopUIController.HideShopPanel();
            playerUIController.TurnOnPlayerUI();
            PauseController.SetPause(false);
            PauseController.DisableInput(false);
        }
    }

    public void SoldOutShop() // called when all shop items are sold out and player cant interact with the shop anymore
    {
        Debug.Log("All items in the store are sold out");
        isShopSoldOut = true;
        shopUIController.HidePressDisplay();
        shopUIController.ShowSoldOutDisplay();
        StopShop();
    }
}
