using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] GameObject cluesPanel;
    [SerializeField] BestiaryUIController bestiaryUIController;

 
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
