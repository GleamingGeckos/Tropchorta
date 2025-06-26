using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public InputReader input;
    [SerializeField] private string initSceneName;
    [SerializeField] private bool startWithVisible;
    [SerializeField] private bool allowExit = true;
    private Canvas canvas;

    public bool IsOpend { get; private set; }

    void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }
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
        SceneManager.LoadScene(initSceneName);
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
