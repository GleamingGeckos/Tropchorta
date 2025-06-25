using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIController : MonoBehaviour
{
    [SerializeField] private ShopController shopController;
    [SerializeField] private GameObject itemPanelPrefab;
    [Header("Items Display")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private List<GameObject> itemPanels;
    [SerializeField] int activePanelId = 0;
    [SerializeField] private GameObject pressDisplayPanel;

    public void ShowPressDisplay()
    {
        pressDisplayPanel.SetActive(true);
    }
    public void HidePressDisplay()
    {
        pressDisplayPanel.SetActive(false);
    }
    public void ShowShopPanel()
    {
        shopPanel.SetActive(true);
        //PauseController.SetPause(true);
        //shopController.equipmentController.GetComponentInParent<PlayerMovement>().playerState.state = PlayerState.;
        foreach (GameObject panel in itemPanels)
        {
            panel.SetActive(false);
        }
        activePanelId = 0;
        if(itemPanels.Count > 0)
        {
            itemPanels[activePanelId].SetActive(true);
        }
    }
    public void HideShopPanel()
    {
        //PauseController.SetPause(false);
        shopPanel.SetActive(false);
    }
    public void NextItemDisplay()// moves on to the next beast display
    {
        itemPanels[activePanelId].SetActive(false);
        if (activePanelId + 1 < itemPanels.Count)
        {
            activePanelId++;
        }
        else
        {
            activePanelId = 0;
        }
        itemPanels[activePanelId].SetActive(true);
    }

    public void LastItemDisplay()// moves on to the last beast display
    {
        itemPanels[activePanelId].SetActive(false);
        if (activePanelId == 0)
        {
            activePanelId = itemPanels.Count - 1;
        }
        else
        {
            activePanelId--;
        }
        itemPanels[activePanelId].SetActive(true);
    }

    public void AddItemPanel(Item item)
    {
        GameObject newItemPanel = Instantiate(itemPanelPrefab,shopPanel.transform);
        itemPanels.Add(newItemPanel);
        newItemPanel.GetComponent<ShopItemPanelController>().LoadItemData(item);
        newItemPanel.transform.SetAsFirstSibling();
        newItemPanel.SetActive(false);
    }

    public void BuySelectedItem()
    {
        if(shopController.BuyItem(activePanelId))
        {
            GameObject itemPanel = itemPanels[activePanelId];
            itemPanels.Remove(itemPanel);
            Destroy(itemPanel);
            activePanelId = 0;
            if (itemPanels.Count > 0)
            {
                itemPanels[activePanelId].SetActive(true);
            }
        }
        else
        {
            Debug.Log("Cant buy this item");
        }
        //set active panel id here i guess
    }
}
