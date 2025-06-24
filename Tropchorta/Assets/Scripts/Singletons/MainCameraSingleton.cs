using UnityEngine;

public class MainCameraSingleton : MonoBehaviour
{
    public static MainCameraSingleton Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
