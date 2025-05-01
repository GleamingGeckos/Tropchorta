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
    private PlayerMovement movement;
    private bool isBlocking = false;
    private float blockingStartTime = 0f;
    private float blockPower = 0.0f;

    // array of colliders so that SphereCast doesn't allocate everytime it's called
    Collider[] colliders = new Collider[16];

    [Header("Temporary stuff")]
    [SerializeField] Vector2Int animationAttackTime = new Vector2Int(0, 30); // TODO : move this to weapon behavior
    [SerializeField] Vector2Int totalAnimationTime = new Vector2Int(1, 20); // TODO : move this to weapon behavior
    private float attackTime = 0f;
    private float animationTime = 0f;
    [SerializeField] float attackCooldown = 0.4f; // TODO : move this to weapon behavior
    bool canAttack = true;
    Coroutine attackCoroutine = null;

    [Header("Combo")]
    private int specialAttackNr = 3;
    [SerializeField] int comboCounter = 0;
    [SerializeField] Vector2Int comboAttackWindow = new Vector2Int(1, 00);
    private float comboAttackTime = 0f;
    [SerializeField] bool doNextAttack;
    [SerializeField] bool isWindowOpen;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
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

        attackTime = animationAttackTime.x + (animationAttackTime.y / 60f);
        animationTime = totalAnimationTime.x + (totalAnimationTime.y / 60f);
        comboAttackTime = comboAttackWindow.x + (comboAttackWindow.y / 60f);
    }

    public void onDamageTaken(float damage, float currentHealth)
    {
        healthBar.SetHealth(currentHealth / health.MaxHealth);
    }

    void OnAttackStart()
    {
        if (playerState.state == PlayerState.DisableInput) return;
        if (!canAttack) return;

        // at this point it SHOULD be not null, but just in case
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(AttackSequence());
            StartCoroutine(ComboWindow());
        }
        else if (isWindowOpen && !(comboCounter == specialAttackNr))// When window was open and not last attack in combo check for next attack
        {
            doNextAttack = true;
        }
    }

    IEnumerator AttackSequence()
    {
        comboCounter++;
        Debug.Log($"Attack started | ComboCounter: {comboCounter}");

        playerState.state = PlayerState.Attacking;

        if (comboCounter == specialAttackNr)
        {
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
            yield return new WaitForSeconds(attackTime);

            equipmentController.UseWeaponStart(transform); // hit logic

            yield return new WaitForSeconds(animationTime - attackTime); // TODO : This should be in a weapon data
            Debug.Log("Special attack triggered!");
            comboCounter = 0;
        }
        else
        {
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
            yield return new WaitForSeconds(attackTime);

            equipmentController.UseWeaponStart(transform); // hit logic

            yield return new WaitForSeconds(animationTime - attackTime); // TODO : This should be in a weapon data
        }

        playerState.state = PlayerState.Normal;
        attackCoroutine = null;

        if (doNextAttack)
        {
            Debug.Log("Chaining next attack");
            doNextAttack = false;
            attackCoroutine = StartCoroutine(AttackSequence());
            StartCoroutine(ComboWindow());
        }
        else
        {
            comboCounter = 0;
            Debug.Log("Combo ended");
        }
    }

    IEnumerator ComboWindow()
    {
        yield return new WaitForSeconds(animationTime - comboAttackTime);
        isWindowOpen = true;
        Debug.Log("Combo window OPEN");

        yield return new WaitForSeconds(comboAttackTime);

        isWindowOpen = false;
        Debug.Log("Combo window CLOSED");
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void StopAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;

            staffAnimator.SetTrigger("AttackEarlyExit");
        }
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
                health.SimpleDamage(ad.damage - (ad.damage * blockPower));
            }
            else
            {
                health.SimpleDamage(ad.damage - (ad.damage * blockPower / 2f));
            }
        }
        else
        {
            health.SimpleDamage(ad.damage);
        }
    }
}
