using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] public InputReader input;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input.OnEscapeEvent += ExitGame;
    }

    // Update is called once per frame
    void ExitGame()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
