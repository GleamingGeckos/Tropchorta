using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestiaryUIController : MonoBehaviour
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject cluesPanel;

    [SerializeField] GameObject goToBestiaryButton;
    [SerializeField] public GameObject goToMenuButton;
    [SerializeField] GameObject goToCluesButton;
    [SerializeField] GameObject buttonsRL;

    [SerializeField] List<GameObject> beastPanels;
    [SerializeField] int activePanelId = -1;
    public void DisplayBeastPanel(int beastId)
    {
        mainPanel.SetActive(false);
        cluesPanel.SetActive(false);
        menuPanel.SetActive(false);
        buttonsRL.SetActive(true);
        beastPanels[beastId].SetActive(true);
        goToBestiaryButton.SetActive(true);   
        goToMenuButton.SetActive(true);
        goToCluesButton.SetActive(true);
        activePanelId = beastId;
    }
    public void NextBeastDisplay()// moves on to the next beast display
    {
        beastPanels[activePanelId].SetActive(false);
        if (activePanelId + 1 < beastPanels.Count)
        {
            activePanelId++;
        }
        else
        {
            activePanelId = 0;
        }
        beastPanels[activePanelId].SetActive(true);
    }

    public void LastBeastDisplay()// moves on to the last beast display
    {
        beastPanels[activePanelId].SetActive(false);
        if (activePanelId == 0)
        {
            activePanelId = beastPanels.Count - 1;
        }
        else
        {
            activePanelId--;
        }
        beastPanels[activePanelId].SetActive(true);
    }
    public void MainPanelDisplay()// moves back to the all beasts display
    {
        if (activePanelId >= 0)
        {
            beastPanels[activePanelId].SetActive(false);
        }
        buttonsRL.SetActive(false);
        goToBestiaryButton.SetActive(false);
        goToMenuButton.SetActive(true);
        goToCluesButton.SetActive(true);
        mainPanel.SetActive(true);
        cluesPanel.SetActive(false);
        menuPanel.SetActive(false);
        activePanelId = -1;
    }

    public void ShowMenuPanel()
    {
        if (activePanelId >= 0)
        {
            beastPanels[activePanelId].SetActive(false);
        }
        mainPanel.SetActive(false);
        cluesPanel.SetActive(false);
        goToBestiaryButton.SetActive(true);
        goToMenuButton.SetActive(false);
        goToCluesButton.SetActive(true);
        menuPanel.SetActive(true);
    }

    public void ShowCluesPanel()
    {
        if (activePanelId >= 0)
        {
            beastPanels[activePanelId].SetActive(false);
        }
        mainPanel.SetActive(false);
        menuPanel.SetActive(false);
        goToBestiaryButton.SetActive(true);
        goToMenuButton.SetActive(true);
        goToCluesButton.SetActive(false);
        cluesPanel.SetActive(true);
    }

}
