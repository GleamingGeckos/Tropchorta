using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] Animator staffAnimator;

    void Start()
    {
        var playerInput = GetComponent<PlayerInput>();
        playerInput.actions["LeftMouse"].performed += OnAttack;
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // check if the clip is already playing, if it is simply reset it
            if (staffAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                // reset the clip
                staffAnimator.Play("Attack", -1, 0);
            }
            else
            {
                // play the clip, if it's not already playing
                staffAnimator.SetTrigger("Attack");
            }
        }

        // TODO : no more attack logic at this point. Will wait for enemies to be implemented
    }
}
