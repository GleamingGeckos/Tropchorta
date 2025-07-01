using UnityEngine;

public static class PauseController
{
    public static InputReader InputReaderRef;
    public static bool IsPaused { get; private set; }

    public static void SetPause(bool pause)
    {
        IsPaused = pause;
        if (IsPaused)
        {
            Time.timeScale = 0f;    
            InputReaderRef.DisableInput();
        }
        else
        {
            Time.timeScale = 1f;
            InputReaderRef.EnableInput();
        }
    }

    public static void DisableInput(bool disable)// kto to kurwa pisal bo to jest kontrintuitywne i walczy z IsPause mimo ze nie wylacza czasu tylko input?????
    {
        IsPaused = disable;
        if (IsPaused)
        {
            InputReaderRef.DisableInput();
        }
        else
        {
            InputReaderRef.EnableInput();
        }
    }
}
