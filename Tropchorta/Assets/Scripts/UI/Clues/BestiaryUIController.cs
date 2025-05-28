using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestiaryUIController : MonoBehaviour
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject buttonsPanel;
    [SerializeField] List<GameObject> beastPanels;
    [SerializeField] int activePanelId = -1;
    public void DisplayBeastPanel(int beastId)
    {
        //mainPanel.SetActive(false);
        //buttonsPanel.SetActive(true);
        beastPanels[beastId].SetActive(true);
        activePanelId = beastId;
    }
    public void NextBeastDisplay()// moves on to the next beast display
    {
        beastPanels[activePanelId].SetActive(false);
        if (activePanelId+1 < beastPanels.Count)
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
            activePanelId = beastPanels.Count-1;
        }
        else
        {
            activePanelId--;
        }
        beastPanels[activePanelId].SetActive(true);
    }
/*    public void MainPanelDisplay()// moves back to the all beasts display
    {
        beastPanels[activePanelId].SetActive(false);
        buttonsPanel.SetActive(false);
        mainPanel.SetActive(true);
        activePanelId = -1;
    }*/

    public void ResetBestiary()
    {
        if(activePanelId>=0)
        {
            beastPanels[activePanelId].SetActive(false);
        }
        buttonsPanel.SetActive(true);
        DisplayBeastPanel(0);
        //mainPanel.SetActive(true);
        activePanelId = 0;
    }
}
