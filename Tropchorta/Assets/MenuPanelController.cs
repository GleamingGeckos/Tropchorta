using System.Collections.Generic;
using UnityEngine;

public class MenuPanelController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject authorsPanel;
    [SerializeField] private GameObject optionsPanel;

    [SerializeField] private BestiaryUIController bestiaryUIController;

    public void ShowAuthors()
    {
        menuPanel.SetActive(false);
        authorsPanel.SetActive(true);
        bestiaryUIController.goToMenuButton.SetActive(true);
    }

    public void ShowOptions()
    {
        menuPanel.SetActive(false);
        optionsPanel.SetActive(true);
        bestiaryUIController.goToMenuButton.SetActive(true);
    }
}
