using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] GameObject bestiaryPanel;
    [SerializeField] GameObject notesPanel;
    [SerializeField] GameObject mapPanel;
    [SerializeField] BestiaryUIController bestiaryUIController;
    [SerializeField] PlayerMovement playerMovement;
    public HealthBar healthBar;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (PauseController.IsPaused) return;
        if (Input.GetKeyDown(KeyCode.B))
        {
            if(bestiaryPanel.activeSelf)
            {
                bestiaryPanel.SetActive(false);
                playerMovement.playerState.state = PlayerState.Normal;
            }
            else
            {
                bestiaryPanel.SetActive(true);
                playerMovement.playerState.state = PlayerState.DisableInput;
                bestiaryUIController.ResetBestiary();
            }
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (notesPanel.activeSelf)
            {
                notesPanel.SetActive(false);
                playerMovement.playerState.state = PlayerState.Normal;
            }
            else
            {
                notesPanel.SetActive(true);
                playerMovement.playerState.state = PlayerState.DisableInput;
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mapPanel.activeSelf)
            {
                mapPanel.SetActive(false);
                playerMovement.playerState.state = PlayerState.Normal;
            }
            else
            {
                mapPanel.SetActive(true);
                playerMovement.playerState.state = PlayerState.DisableInput;
            }
        }
    }
}
