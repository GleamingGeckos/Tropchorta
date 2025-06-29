using UnityEngine;

public class PlayerUISingleton : MonoBehaviour
{
    public static PlayerUISingleton Instance;

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

    public void ResetSingleton()
    {
        Instance = null;
        Destroy(gameObject);
    }
}
