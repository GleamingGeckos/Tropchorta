using UnityEngine;

public class Resete : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerSingleton.Instance != null)
        {
            PlayerSingleton.Instance.ResetSingleton();
        }
    }

}
