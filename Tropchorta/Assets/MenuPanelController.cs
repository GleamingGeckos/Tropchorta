using System.Collections.Generic;
using UnityEngine;

public class MenuPanelController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject authorsPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] PlayerMovement playerMovement;

    [SerializeField] PlayerUIController playerUIController;

    [SerializeField] private BestiaryUIController bestiaryUIController;

    public void ContinueButton ()
    {
        playerUIController.CloseAllUI();
    }

    public void ShowAuthors()
    {
        playerUIController.CloseAllPanels();
        authorsPanel.SetActive(true);
        bestiaryUIController.goToMenuButton.SetActive(true);
    }

    public void ShowOptions()
    {
        playerUIController.CloseAllPanels();
        optionsPanel.SetActive(true);
        bestiaryUIController.goToMenuButton.SetActive(true);
    }
}
