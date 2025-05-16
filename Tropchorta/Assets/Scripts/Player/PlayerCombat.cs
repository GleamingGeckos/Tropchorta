using System.Collections;
using System.Collections.Generic;
using FMODUnity;
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
    public bool isBlocking = false;
    private float blockingStartTime = 0f;

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

    [Header("Input")]
    float attackPressTime;
    [SerializeField] float whenConsiderHoldingAttack = 0.2f;
    [SerializeField] bool isHoldingAttack;

    [Header("Combo")]
    [SerializeField] int comboCounter = 0;
    private int specialAttackNr = 3;
    [SerializeField] Vector2Int comboAttackWindow = new Vector2Int(1, 00);
    private float comboAttackTime = 0f;
    [SerializeField] bool doNextAttack;
    [SerializeField] bool isNextStrongAttack;
    [SerializeField] bool isWindowOpen;


    [Header("Step during attack")]
    [SerializeField] float distance = 1.0f;
    float playerRadius = 1.0f;
    Coroutine stepCoroutine = null;

    [SerializeField] EventReference tempAttackEvent; // TODO : move this to weapons


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

        playerRadius = GetComponent<CapsuleCollider>().radius;
    }

    public void onDamageTaken(float damage, float currentHealth)
    {
        healthBar.SetHealth(currentHealth / health.MaxHealth);
    }

    void OnAttackStart()
    {
        attackPressTime = Time.time;
        isHoldingAttack = false;
    }

    void OnAttackEnd()
    {
        float pressDuration = Time.time - attackPressTime;
        isHoldingAttack = pressDuration > whenConsiderHoldingAttack; 

        if (playerState.state == PlayerState.DisableInput) return;
        if (!canAttack) return;

        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(AttackSequence(isHoldingAttack));
            StartCoroutine(ComboWindow());
        }
        else if (isWindowOpen)
        {
            isNextStrongAttack = isHoldingAttack;
            doNextAttack = true;
        }
    }


    IEnumerator AttackSequence(bool isHolding = false)
    {
        comboCounter++;

        playerState.state = PlayerState.Attacking;
        RuntimeManager.PlayOneShot(tempAttackEvent, transform.position); // TODO : move this to weapon behavior
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

            equipmentController.UseWeaponSpecialAttack(transform); // hit logic

            yield return new WaitForSeconds(animationTime - attackTime); // TODO : This should be in a weapon data
            comboCounter = 0;
            doNextAttack = false;
            equipmentController.UseWeaponEnd(transform);
        }
        else if (isHolding)
        {
            stepCoroutine = StartCoroutine(MoveForwardSmooth(transform, distance, attackTime));
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

            equipmentController.UseWeaponStrongStart(transform); // hit logic

            yield return new WaitForSeconds(animationTime - attackTime); // TODO : This should be in a weapon data
        }
        else
        {
            stepCoroutine = StartCoroutine(MoveForwardSmooth(transform, distance, attackTime));
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
            doNextAttack = false;
            attackCoroutine = StartCoroutine(AttackSequence(isNextStrongAttack));
            StartCoroutine(ComboWindow());
        }
        else
        {
            comboCounter = 0;
            if(isHolding)
            {
                equipmentController.UseWeaponEnd(transform);
            }
            else
                equipmentController.UseWeaponEnd(transform);
        }
    }

    IEnumerator ComboWindow()
    {
        yield return new WaitForSeconds(animationTime - comboAttackTime);
        isWindowOpen = true;

        yield return new WaitForSeconds(comboAttackTime);
        isWindowOpen = false;
    }

    IEnumerator MoveForwardSmooth(Transform target, float distance, float duration)
    {
        Vector3 start = target.position;
        Vector3 end = start + GetRotatingRootForward() * distance;
        float elapsed = 0f;
        RaycastHit hit;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            Vector3 nextPos = Vector3.Lerp(start, end, t);

            if (Physics.Raycast(target.position, GetRotatingRootForward(), out hit, Vector3.Distance(target.position, nextPos + (GetRotatingRootForward() * playerRadius))))
            {
                target.position = hit.point + (GetRotatingRootForward() * -playerRadius);
                yield break;
            }

            target.position = nextPos;
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.position = end;
    }

    public void BreakCombo()
    {
        comboCounter = 0;
        doNextAttack = false;
        if(stepCoroutine != null)
            StopCoroutine(stepCoroutine);
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

    public void StartBlocking()
    {
        if (playerState.state == PlayerState.DisableInput) return;
        isBlocking = true;
        CheckForAttackingEnemies(isBlocking);
        blockingStartTime = Time.time;
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
        if (!isBlocking)
        {
            health.SimpleDamage(ad.damage);
        }
    }

    private void CheckForAttackingEnemies(bool isBlocking)//TODO im tired, needs to be redone, but works for now
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 3);
        HashSet<GameObject> processed = new HashSet<GameObject>();

        foreach (var hit in hits)
        {
            GameObject obj = hit.gameObject;
            if (processed.Contains(obj)) continue;
            processed.Add(obj);

            if (obj.CompareTag("Enemy"))
            {
                var combat = obj.GetComponent<EnemyCombat>();
                var movement = obj.GetComponent<EnemyMovement>();

                if (combat != null && movement != null && combat.isPerfectBlockWindow)
                {
                    movement.perfectParWasInitiated = true;
                }
            }
        }
    }

}
