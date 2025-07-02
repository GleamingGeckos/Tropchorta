using System.Collections.Generic;
using UnityEngine;

public class MenuPanelController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject authorsPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] PlayerMovement playerMovement;

    [SerializeField] private BestiaryUIController bestiaryUIController;
    [SerializeField] private GameObject go_bestiaryUIController;

    public void ContinueButton ()
    {
/*        go_bestiaryUIController.SetActive(false);
        PauseController.SetPause(false);
        playerMovement.playerState.state = PlayerState.Normal;*/
    }

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
