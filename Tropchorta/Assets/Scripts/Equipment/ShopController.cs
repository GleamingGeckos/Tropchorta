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

    public bool isPlayerClose = false;
    public bool isShopOpened = false;
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
        if (isPlayerClose)
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
            if (items[itemId].justDrop)
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
            shopUIController.ShowPressDisplay();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerClose = false;
            shopUIController.HidePressDisplay();
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
}
