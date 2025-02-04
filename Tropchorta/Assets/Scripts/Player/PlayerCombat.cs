using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] Animator staffAnimator;
    [SerializeField] HealthBar healthBar;
    HealthComponent health;

    void Start()
    {
        var playerInput = GetComponent<PlayerInput>();
        playerInput.actions["LeftMouse"].performed += OnAttack;
        health = GetComponent<HealthComponent>();
        health.onDamageTaken.AddListener(onDamageTaken);
    }


    public void onDamageTaken(float damage, float currentHealth)
    {
        healthBar.SetHealth(currentHealth / health.MaxHealth);
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
        health.Damage(10);
        // TODO : no more attack logic at this point. Will wait for enemies to be implemented
    }
}
