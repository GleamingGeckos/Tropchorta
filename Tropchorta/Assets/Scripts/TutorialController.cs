using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public Animator PlayerAnimator;

    public static bool disableInput = false;

    private void Update()
    {
        if (disableInput)
            PauseController.DisableInput(true);
        else
            PauseController.DisableInput(false);

    }


}
