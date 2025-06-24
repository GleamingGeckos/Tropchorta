using UnityEngine;

public class PauseTrigger : MonoBehaviour
{
    [SerializeField] private GameObject uiPanel;

    public Animator PlayerAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TutorialController.disableInput = true;
            PlayerAnimator.SetBool("isSprinting", false);
            PlayerAnimator.SetBool("isMoving", false);
            if (uiPanel != null)
            {
                uiPanel.SetActive(true);     // Pokazuje UI
            }
        }
    }
}
