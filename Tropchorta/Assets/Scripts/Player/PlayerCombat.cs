using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] Animator staffAnimator;
    [SerializeField] public GameObject weaponSlot;
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
    private float animationTime = 0f;// TODO : move this to weapon behavior
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
    public bool isWindowOpen;


    [Header("Step during attack")]
    [SerializeField] float distance = 1.0f;
    float playerRadius = 1.0f;
    [SerializeField] float stepTime = 0.4f;
    Coroutine stepCoroutine = null;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        var pm = GetComponent<PlayerMovement>();
        pm.input.OnLeftMouseClickEvent += OnClickStart;
        pm.input.OnLeftMouseReleaseEvent += OnClickEnd;
        pm.input.OnRightMouseClickEvent += AltUseStart;
        pm.input.OnRightMouseReleaseEvent += AltUseEnd;
        playerState = pm.playerState;
        health = GetComponent<PlayerHealthComponent>();
        health.onDamageTaken.AddListener(OnDamageTaken);
        health.onAttacked.AddListener(OnAttacked);
        health.onDeath.AddListener(Die);
        equipmentController.Initialize(transform);

        stepTime = 0.2f;
        animationTime = totalAnimationTime.x + (totalAnimationTime.y / 60f);
        comboAttackTime = comboAttackWindow.x + (comboAttackWindow.y / 60f);

        playerRadius = GetComponent<CapsuleCollider>().radius;
    }

    public void OnDamageTaken(float damage, float currentHealth)
    {
        healthBar.SetHealth(currentHealth / health.MaxHealth);
    }

    public void Die()
    {
        playerState.state = PlayerState.DisableInput;
        StopAllCoroutines(); // Zatrzymaj wszystkie dzia�ania (np. ataki)
        equipmentController.UseWeaponEnd(transform); // Upewnij si�, �e bro� si� deaktywuje
        SceneManager.LoadScene("MainMenu");
    }

    void OnClickStart()
    {
        attackPressTime = Time.time;
        isHoldingAttack = false;
    }

    public void OnClickEnd()
    {
        float pressDuration = Time.time - attackPressTime;
        isHoldingAttack = pressDuration > whenConsiderHoldingAttack; 

        if (playerState.state == PlayerState.DisableInput) return;
        if (playerState.state == PlayerState.Attacking) return;
        if (!canAttack) return;

        StartAttackAnim();
        
        if (isWindowOpen)
        {
            isNextStrongAttack = isHoldingAttack;
            doNextAttack = true;
        }
    }
     
    public void Attack()
    {
        equipmentController.UseWeaponStart(rotatingRootTransform);
        stepCoroutine = StartCoroutine(MoveForwardSmooth(transform, distance, stepTime));
        Debug.Log("Attack");
    }  

    private void StartAttackAnim(bool isHolding = false)
    {
        comboCounter++;
        playerState.state = PlayerState.Attacking;
        movement.RotatePlayerTowardsMouse();
        //if (movement.PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        //{
            // reset the clip
            //movement.PlayerAnimator.ResetTrigger("attackTriggerPlayer");
           // movement.WeaponAnimator.ResetTrigger("attackTriggerPlayer");
        //}
        //else
        //{
            // play the clip, if it's not already playing
            movement.PlayerAnimator.SetTrigger("attackTriggerPlayer");
            movement.WeaponAnimator.SetTrigger("attackTriggerPlayer");
        //}
    }

    public void EndAttack(bool isHolding = false)
    {
        Debug.Log("End Attack");
        playerState.state = PlayerState.Normal;
        if (doNextAttack)
        {
            doNextAttack = false;
            StartAttackAnim(isNextStrongAttack);
        }
        else
        {
            comboCounter = 0;
            if (isHolding)
            {
                equipmentController.UseWeaponEnd(transform);
            }
            else
            {
                equipmentController.UseWeaponEnd(transform);
            }
        }
    }

    IEnumerator AttackSequence(bool isHolding = false)
    {
        comboCounter++;
        //movement.NormalMovement();
        playerState.state = PlayerState.Attacking;
        movement.RotatePlayerTowardsMouse();
        //RuntimeManager.PlayOneShot(tempAttackEvent, transform.position); // TODO : move this to weapon behavior
        if (comboCounter == specialAttackNr)
        {
            stepCoroutine = StartCoroutine(MoveForwardSmooth(transform, distance, stepTime));
            // TODO : move this to a weapon behavior somehow
            // check if the clip is already playing, if it is simply reset it
            if (staffAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                // reset the clip
                staffAnimator.Play("Attack", -1, 0);
                movement.PlayerAnimator.ResetTrigger("attackTriggerPlayer");
                movement.WeaponAnimator.ResetTrigger("attackTriggerPlayer");
                
            }
            else
            {
                // play the clip, if it's not already playing
                staffAnimator.SetTrigger("Attack");
               
                movement.PlayerAnimator.SetTrigger("attackTriggerPlayer");
                movement.WeaponAnimator.SetTrigger("attackTriggerPlayer");
             
            }
            yield return new WaitForSeconds(stepTime);

            //equipmentController.UseWeaponSpecialAttack(rotatingRootTransform); // hit logic

            yield return new WaitForSeconds(animationTime - stepTime); // TODO : This should be in a weapon data
            comboCounter = 0;
            doNextAttack = false;
            equipmentController.UseWeaponEnd(transform);
        }
        else if (isHolding)
        {
            stepCoroutine = StartCoroutine(MoveForwardSmooth(transform, distance, stepTime));
            // TODO : move this to a weapon behavior somehow
            // check if the clip is already playing, if it is simply reset it
            if (staffAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                // reset the clip
                staffAnimator.Play("Attack", -1, 0);
                movement.PlayerAnimator.ResetTrigger("attackTriggerPlayer");
                movement.WeaponAnimator.ResetTrigger("attackTriggerPlayer");
            }
            else
            {
                // play the clip, if it's not already playing
                staffAnimator.SetTrigger("Attack");
                movement.PlayerAnimator.SetTrigger("attackTriggerPlayer");
                movement.WeaponAnimator.SetTrigger("attackTriggerPlayer");
            }

            yield return new WaitForSeconds(stepTime);

            //equipmentController.UseWeaponStrongStart(rotatingRootTransform); // hit logic

            yield return new WaitForSeconds(animationTime - stepTime); // TODO : This should be in a weapon data
        }
        else
        {
            stepCoroutine = StartCoroutine(MoveForwardSmooth(transform, distance, stepTime));
            // TODO : move this to a weapon behavior somehow
            // check if the clip is already playing, if it is simply reset it
            if (staffAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                // reset the clip
                staffAnimator.Play("Attack", -1, 0);
                movement.PlayerAnimator.ResetTrigger("attackTriggerPlayer");
                movement.WeaponAnimator.ResetTrigger("attackTriggerPlayer");
            }
            else
            {
                // play the clip, if it's not already playing
                staffAnimator.SetTrigger("Attack");
                movement.PlayerAnimator.SetTrigger("attackTriggerPlayer");
                movement.WeaponAnimator.SetTrigger("attackTriggerPlayer");
            }

            yield return new WaitForSeconds(stepTime);

            //equipmentController.UseWeaponStart(rotatingRootTransform); // hit logic

            //yield return new WaitForSeconds(animationTime - attackTime); // TODO : This should be in a weapon data
        }

        playerState.state = PlayerState.Normal;
        attackCoroutine = null;

        if (doNextAttack)
        {
            doNextAttack = false;
            //attackCoroutine = StartCoroutine(AttackSequence(isNextStrongAttack));
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
