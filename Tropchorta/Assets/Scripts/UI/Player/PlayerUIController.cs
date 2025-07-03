using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUIController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject bookPanel;
    [SerializeField] GameObject mapPanel;
    [SerializeField] GameObject menuPanel;
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
                if (bookPanel.activeSelf && !isBaseState)
                {
                    CloseBook();
                }
                else if (isBaseState)
                {
                    OpenBook();
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
                }
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                if (mapPanel.activeSelf && !isBaseState)
                {
                    mapPanel.SetActive(false);
                    PauseController.SetPause(false);
                    playerMovement.playerState.state = PlayerState.Normal;
                    isBaseState = true;
                }
                else if (isBaseState)
                {
                    mapPanel.SetActive(true);
                    PauseController.SetPause(true);
                    playerMovement.playerState.state = PlayerState.DisableInput;
                    isBaseState = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (equipmentBackpackPanel.activeSelf && !isBaseState)
                {
                    equipmentBackpackPanel.SetActive(false);
                    PauseController.SetPause(false);
                    playerMovement.playerState.state = PlayerState.Normal;
                    isBaseState = true;
                }
                else if (isBaseState)
                {
                    equipmentBackpackPanel.SetActive(true);
                    PauseController.SetPause(true);
                    playerMovement.playerState.state = PlayerState.DisableInput;
                    isBaseState = false;
                }
            }
        }
    }

    public void OpenMenu()
    {
        bookPanel.SetActive(true);
        bestiaryUIController.ShowMenuPanel();
        PauseController.SetPause(true);
        isBaseState = false;
        playerMovement.playerState.state = PlayerState.DisableInput;
    }


    public void OpenBook()
    {
        bookPanel.SetActive(true);
        PauseController.SetPause(true);
        playerMovement.playerState.state = PlayerState.DisableInput;
        bestiaryUIController.MainPanelDisplay();
        isBaseState = false;
    }

    public void CloseBook()
    {
        bookPanel.SetActive(false);
        PauseController.SetPause(false);
        playerMovement.playerState.state = PlayerState.Normal;
        isBaseState = true;
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
        bookPanel.SetActive(false);
        menuPanel.SetActive(false);
        mapPanel.SetActive(false);
        equipmentBackpackPanel.SetActive(false);
        PauseController.SetPause(false);
        playerMovement.playerState.state = PlayerState.Normal;
        isBaseState = true;
    }

    public void ShowBackpackPanel()
    {
        equipmentBackpackPanel.SetActive(true);
        PauseController.SetPause(true);
        playerMovement.playerState.state = PlayerState.DisableInput;
        isBaseState = false;
    }

    public void HideBackpackPanel()
    {
        equipmentBackpackPanel.SetActive(false);
        PauseController.SetPause(false);
        playerMovement.playerState.state = PlayerState.Normal;
        isBaseState = true;
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
}
