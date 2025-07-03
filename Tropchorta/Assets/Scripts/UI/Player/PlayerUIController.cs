using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUIController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject bestiaryPanel;
    [SerializeField] GameObject mapPanel;
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject cluesPanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject authorsPanel;
    [SerializeField] GameObject mapImagePanel;

    [SerializeField] GameObject equipmentBackpackPanel;

    [SerializeField] BestiaryUIController bestiaryUIController;
    [SerializeField] PlayerMovement playerMovement;
    
    private EquipmentBackpackUIController equipmentBackpackUIController;
    public HealthBar healthBar;

    [SerializeField] bool canUseUI = true; // when can be changed to any other state
    [SerializeField] bool isBaseState = true; // when can be changed to any other state

    [Header("MainMenu")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerUI;
    [SerializeField] private bool destroyPersistentObjects = true;
    //[SerializeField] private string sceneName;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        equipmentBackpackUIController = equipmentBackpackPanel.GetComponent<EquipmentBackpackUIController>();
    }

    private void Start()
    {
        PauseController.SetPause(false);
        PauseController.DisableInput(false);
    }
    void Update()
    {
        if (canUseUI)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (bestiaryPanel.activeSelf && !isBaseState)
                {
                    CloseBestiary();
                    mapImagePanel.SetActive(false);
                }
                else if (isBaseState)
                {
                    OpenBestiary();
                    mapImagePanel.SetActive(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isBaseState)
                {
                    CloseAllPanels();
                }
                else if (isBaseState)
                {
                    OpenMenu();
                    mapImagePanel.SetActive(true);
                }
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                if (mapPanel.activeSelf && !isBaseState)
                {
                    mapPanel.SetActive(false);
                    CloseAllButtons();
                    Reasume();
                }
                else if (isBaseState)
                {
                    OpenMap();
                    mapImagePanel.SetActive(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (equipmentBackpackPanel.activeSelf && !isBaseState)
                {
                    equipmentBackpackPanel.SetActive(false);
                    Reasume();
                }
                else if (isBaseState)
                {
                    equipmentBackpackPanel.SetActive(true);
                    Pause(); 
                }
            }
        }
    }

    public void OpenMap()
    {
        bestiaryUIController.ShowMapPanel();
        Pause();
    }

    public void OpenMenu()
    {
        bestiaryUIController.ShowMenuPanel();
        Pause();
    }


    public void OpenBestiary()
    {
        bestiaryUIController.ShowBestiary();
        Pause();
    }

    public void CloseBestiary()
    {
        bestiaryPanel.SetActive(false);
        CloseAllButtons();
        Reasume();
    }

    public void TurnOnPlayerUI()
    {
        CloseAllPanels();
        canUseUI = true;
    }

    public void TurnOffPlayerUI()
    {
        CloseAllPanels();
        canUseUI = false;
    }

    public void TurnOnBackpackDroppingCapability()
    {
        equipmentBackpackUIController.TurnOnDropZone();
    }
    public void TurnOffBackpackDroppingCapability()
    {
        equipmentBackpackUIController.TurnOffDropZone();
    }

    public void CloseAllPanels()
    {
        //Panels
        bestiaryPanel.SetActive(false);
        menuPanel.SetActive(false);
        mapPanel.SetActive(false);
        cluesPanel.SetActive(false);
        equipmentBackpackPanel.SetActive(false);
        optionsPanel.SetActive(false);
        authorsPanel.SetActive(false);
        mapImagePanel.SetActive(false);

        CloseAllButtons();

        Reasume(); 
    }

    public void CloseAllButtons()
    {
        //Buttons
        bestiaryUIController.goToMenuButton.SetActive(false);
        bestiaryUIController.goToMapButton.SetActive(false);
        bestiaryUIController.goToCluesButton.SetActive(false);
        bestiaryUIController.goToBestiaryButton.SetActive(false);
        bestiaryUIController.buttonsRL.SetActive(false);
    }

    public void ShowBackpackPanel()
    {
        equipmentBackpackPanel.SetActive(true);
        Pause();
    }

    public void HideBackpackPanel()
    {
        equipmentBackpackPanel.SetActive(false);
        Reasume();
    }

    public void ChangeScene(string sceneName)
    {
        CloseAllPanels();
        if (destroyPersistentObjects)
        {
            if (mainCamera != null)
            {
                Destroy(mainCamera);
            }
            if (player != null)
            {
                Destroy(player);
            }
            if (playerUI != null)
            {
                Destroy(playerUI);
            }
            //Destroy(GameObject.FindGameObjectWithTag("MainCamera")); 
            //Destroy(GameObject.FindGameObjectWithTag("Player"));
            //Destroy(GameObject.FindGameObjectWithTag("PlayerUI"));
            //PlayerUISingleton.Instance.ResetSingleton();
        }
        //PauseController.SetPause(false);
        SceneManager.LoadScene(sceneName);
    }

    public void Pause()
    {
        PauseController.SetPause(true);
        isBaseState = false;
        playerMovement.playerState.state = PlayerState.DisableInput;
    }

    public void Reasume()
    {
        PauseController.SetPause(false);
        isBaseState = true;
        playerMovement.playerState.state = PlayerState.Normal;
    }
}
