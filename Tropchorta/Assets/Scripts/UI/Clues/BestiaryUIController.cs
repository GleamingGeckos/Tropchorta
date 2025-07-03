using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestiaryUIController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject cluesPanel;
    [SerializeField] GameObject mapPanel;
    [Header("Buttons")]
    [SerializeField] public GameObject goToBestiaryButton;
    [SerializeField] public GameObject goToMenuButton;
    [SerializeField] public GameObject goToCluesButton;
    [SerializeField] public GameObject goToMapButton;
    [SerializeField] public GameObject buttonsRL;

    [SerializeField] List<GameObject> beastPanels;
    [SerializeField] int activePanelId = -1;
    [SerializeField] PlayerUIController playerUIController;


    public void DisplayBeastPanel(int beastId)
    {
/*        mainPanel.SetActive(false);
        cluesPanel.SetActive(false);
        menuPanel.SetActive(false);
        mapPanel.SetActive(false);*/
        playerUIController.CloseAllPanels();

        buttonsRL.SetActive(true);
        goToBestiaryButton.SetActive(true);   
        goToMenuButton.SetActive(true);
        goToCluesButton.SetActive(true);
        goToMapButton.SetActive(true);

        beastPanels[beastId].SetActive(true);

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

    public void PreviousBeastDisplay()// moves on to the last beast display
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
    public void ShowBestiary()// moves back to the all beasts display
    {
        if (activePanelId >= 0)
        {
            beastPanels[activePanelId].SetActive(false);
        }
        /*        cluesPanel.SetActive(false);
                menuPanel.SetActive(false);
                mapPanel.SetActive(false);*/
        playerUIController.CloseAllPanels();

        buttonsRL.SetActive(false);
        goToBestiaryButton.SetActive(false);
        goToMenuButton.SetActive(true);
        goToCluesButton.SetActive(true);
        goToMapButton.SetActive(true);

        mainPanel.SetActive(true);

        activePanelId = -1;
    }

    public void ShowMenuPanel()
    {
        if (activePanelId >= 0)
        {
            beastPanels[activePanelId].SetActive(false);
        }
        /*        mainPanel.SetActive(false);
                cluesPanel.SetActive(false);
                mapPanel.SetActive(false);*/
        playerUIController.CloseAllPanels();

        buttonsRL.SetActive(false);
        goToBestiaryButton.SetActive(true);
        goToMenuButton.SetActive(false);
        goToCluesButton.SetActive(true);
        goToMapButton.SetActive(true);

        menuPanel.SetActive(true);
    }

    public void ShowCluesPanel()
    {
        if (activePanelId >= 0)
        {
            beastPanels[activePanelId].SetActive(false);
        }
        /*        mainPanel.SetActive(false);
                menuPanel.SetActive(false);
                mapPanel.SetActive(false);*/
        playerUIController.CloseAllPanels();

        buttonsRL.SetActive(false);
        goToBestiaryButton.SetActive(true);
        goToMenuButton.SetActive(true);
        goToCluesButton.SetActive(false);
        goToMapButton.SetActive(true);

        cluesPanel.SetActive(true);
    }

    public void ShowMapPanel()
    {
        if (activePanelId >= 0)
        {
            beastPanels[activePanelId].SetActive(false);
        }
        /*        mainPanel.SetActive(false);
                menuPanel.SetActive(false);
                cluesPanel.SetActive(false);*/
        playerUIController.CloseAllPanels();

        buttonsRL.SetActive(false);
        goToBestiaryButton.SetActive(true);
        goToMenuButton.SetActive(true);
        goToCluesButton.SetActive(true);
        goToMapButton.SetActive(false);

        mapPanel.SetActive(true);
    }

}
