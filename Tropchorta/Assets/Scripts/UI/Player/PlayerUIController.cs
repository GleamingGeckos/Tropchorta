using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] GameObject cluesPanel;
    [SerializeField] BestiaryUIController bestiaryUIController;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if(cluesPanel.activeSelf)
            {
                cluesPanel.SetActive(false);
            }
            else
            {
                cluesPanel.SetActive(true);
                bestiaryUIController.ResetBestiary();
            }
        }
    }
}
