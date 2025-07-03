using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public InputReader input;
    [SerializeField] private string initSceneName;
    [SerializeField] private string tutSceneName;
    [SerializeField] private bool startWithVisible;
    [SerializeField] private bool allowExit = true;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerUI;
    [SerializeField] private bool destroyPersistentObjects = false;
    private Canvas canvas;

    public bool IsOpend { get; private set; }

    void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player");
        playerUI = GameObject.FindGameObjectWithTag("PlayerUI");
    }

    void Start()
    {
        //input.OnEscapeEvent += TogglePauseGame;
        canvas = GetComponent<Canvas>();    
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        canvas.enabled = startWithVisible;
        IsOpend = startWithVisible;
    }

    private void Update()
    {
/*        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }*/
    }

    public void ShowMenu()
    {
        canvas.enabled = true;
        IsOpend = true;
        PauseController.SetPause(true);
    }

    public void HideMenu()
    {
        canvas.enabled = false;
        IsOpend = false;
        PauseController.SetPause(false);
    }

    public void NewGame()
    {
        if (destroyPersistentObjects)
        {
            if(mainCamera != null)
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
        SceneManager.LoadScene(initSceneName);
    }

    public void Tutorial()
    {
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
        SceneManager.LoadScene(tutSceneName);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    void TogglePauseGame()
    {
        if (!IsOpend)
            ShowMenu();
        else
            HideMenu();
    }
}
