using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] Animator staffAnimator;
    [SerializeField] HealthBar healthBar;
    [SerializeField] Transform rotatingRootTransform;
    [SerializeField] PlayerStateSO playerState;
    [SerializeField] EquipmentController equipmentController;
    HealthComponent health;

    // array of colliders so that SphereCast doesn't allocate everytime it's called
    Collider[] colliders = new Collider[16];

    void Start()
    {
        var pm = GetComponent<PlayerMovement>();
        pm.input.OnLeftMouseClickEvent += OnAttackStart;
        pm.input.OnLeftMouseReleaseEvent += OnAttackEnd;
        playerState = pm.playerState;
        health = GetComponent<HealthComponent>();
        health.onDamageTaken.AddListener(onDamageTaken);
    }

    public void onDamageTaken(float damage, float currentHealth)
    {
        healthBar.SetHealth(currentHealth / health.MaxHealth);
    }

    void OnAttackStart()
    {
        if (playerState.state == PlayerState.DisableInput) return;
        // TODO : move this to a weapon behavior somehow
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

        equipmentController.UseWeaponStart(transform);
    }

    void OnAttackEnd()
    {
        equipmentController.UseWeaponEnd(transform);
    }

    public Vector3 GetRotatingRootForward()
    {
        return rotatingRootTransform.forward;
    }
}
