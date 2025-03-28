using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] Animator staffAnimator;
    [SerializeField] HealthBar healthBar;
    [SerializeField] Transform rotatingRootTransform;
    [SerializeField] PlayerStateSO playerState;
    [SerializeField] EquipmentController equipmentController;
    private PlayerHealthComponent health;
    private bool isBlocking = false;
    private float blockingStartTime = 0f;
    private float blockPower = 0.0f;

    // array of colliders so that SphereCast doesn't allocate everytime it's called
    Collider[] colliders = new Collider[16];

    float attackCooldown = 0.4f; // TODO : move this to weapon behavior
    bool canAttack = true;

    void Start()
    {
        var pm = GetComponent<PlayerMovement>();
        pm.input.OnLeftMouseClickEvent += OnAttackStart;
        pm.input.OnLeftMouseReleaseEvent += OnAttackEnd;
        pm.input.OnRightMouseClickEvent += AltUseStart;
        pm.input.OnRightMouseReleaseEvent += AltUseEnd;
        playerState = pm.playerState;
        health = GetComponent<PlayerHealthComponent>();
        health.onDamageTaken.AddListener(onDamageTaken);
        health.onAttacked.AddListener(OnAttacked);
        equipmentController.Initialize(transform);
    }

    public void onDamageTaken(float damage, float currentHealth)
    {
        healthBar.SetHealth(currentHealth / health.MaxHealth);
    }

    void OnAttackStart()
    {
        if (playerState.state == PlayerState.DisableInput) return;
        if (!canAttack) return;
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
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void OnAttackEnd()
    {
        equipmentController.UseWeaponEnd(transform);
    }

    void AltUseStart()
    {
        equipmentController.UseWeaponAltStart(transform);
    }

    void AltUseEnd()
    {
        equipmentController.UseWeaponAltEnd(transform);
    }

    public Vector3 GetRotatingRootForward()
    {
        return rotatingRootTransform.forward;
    }

    public void StartBlocking(float blockPower)
    {
        if (playerState.state == PlayerState.DisableInput) return;
        isBlocking = true;
        blockingStartTime = Time.time;
        this.blockPower = blockPower;
        if (staffAnimator.GetCurrentAnimatorStateInfo(0).IsName("Block"))
        {
            // reset the clip
            staffAnimator.Play("Block", -1, 0);
        }
        else
        {
            // play the clip, if it's not already playing
            staffAnimator.SetBool("Blocking", true);
        }
    }

    public void StopBlocking()
    {
        if (playerState.state == PlayerState.DisableInput) return;
        isBlocking = false;
        blockingStartTime = 0f;

        staffAnimator.SetBool("Blocking", false);
    }

    private void OnAttacked(AttackData ad)
    {
        if (isBlocking)
        {
            if (Time.time - blockingStartTime < ad.blockTime)
            {
                Debug.Log("Sucessful block");
                health.SimpleDamage(ad.damage - (ad.damage * blockPower));
            }
            else
            {
                Debug.Log("Failed block");
                health.SimpleDamage(ad.damage - (ad.damage * blockPower / 2f));
            }
        }
        else
        {
            Debug.Log("Normal damage");
            health.SimpleDamage(ad.damage);
        }
    }
}
