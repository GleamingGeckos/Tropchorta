using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] GameObject bestiaryPanel;
    [SerializeField] GameObject notesPanel;
    [SerializeField] GameObject mapPanel;
    [SerializeField] BestiaryUIController bestiaryUIController;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] GameObject equipmentBackpackPanel;
    private EquipmentBackpackUIController equipmentBackpackUIController;
    public HealthBar healthBar;

    [SerializeField] bool canUseUI = true; // when can be changed to any other state
    [SerializeField] bool isBaseState = true; // when can be changed to any other state

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        equipmentBackpackUIController = equipmentBackpackPanel.GetComponent<EquipmentBackpackUIController>();
    }

    void Update()
    {
        if (canUseUI)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (bestiaryPanel.activeSelf && !isBaseState)
                {
                    bestiaryPanel.SetActive(false);
                    PauseController.SetPause(false);
                    playerMovement.playerState.state = PlayerState.Normal;
                    isBaseState = true;
                }
                else if (isBaseState)
                {
                    bestiaryPanel.SetActive(true);
                    PauseController.SetPause(true);
                    playerMovement.playerState.state = PlayerState.DisableInput;
                    bestiaryUIController.ResetBestiary();
                    isBaseState = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                if (notesPanel.activeSelf && !isBaseState)
                {
                    notesPanel.SetActive(false);
                    PauseController.SetPause(false);
                    playerMovement.playerState.state = PlayerState.Normal;
                    isBaseState = true;
                }
                else if (isBaseState)
                {
                    notesPanel.SetActive(true);
                    PauseController.SetPause(true);
                    playerMovement.playerState.state = PlayerState.DisableInput;
                    isBaseState = false;
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
        bestiaryPanel.SetActive(false);
        notesPanel.SetActive(false);
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
}
