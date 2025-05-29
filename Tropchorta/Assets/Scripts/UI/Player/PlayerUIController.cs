using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] GameObject bestiaryPanel;
    [SerializeField] GameObject notesPanel;
    [SerializeField] BestiaryUIController bestiaryUIController;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if(bestiaryPanel.activeSelf)
            {
                bestiaryPanel.SetActive(false);
            }
            else
            {
                bestiaryPanel.SetActive(true);
                bestiaryUIController.ResetBestiary();
            }
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (notesPanel.activeSelf)
            {
                notesPanel.SetActive(false);
            }
            else
            {
                notesPanel.SetActive(true);
            }
        }
    }
}
