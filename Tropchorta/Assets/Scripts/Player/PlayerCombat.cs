using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] Animator staffAnimator;
    [SerializeField] public GameObject weaponSlot;
    [SerializeField] Transform rotatingRootTransform;
    [SerializeField] PlayerStateSO playerState;
    [SerializeField] EquipmentController equipmentController;
    private PlayerHealthComponent health;
    private PlayerMovement movement;
    public bool isBlocking = false;
    [NonSerialized] public bool queuedAttack = false;

    // array of colliders so that SphereCast doesn't allocate everytime it's called
    Collider[] colliders = new Collider[16];

    [Header("Temporary stuff")]
    bool canAttack = true;

    [Header("Input")]
    float attackPressTime;
    [SerializeField] float whenConsiderHoldingAttack = 0.2f;
    [SerializeField] bool isHoldingAttack;
    [NonSerialized] private bool mouseBeingHeld;

    [Header("Combo")]
    [SerializeField] int comboCounter = 0;
    [SerializeField] ParticleSystem shockWave;
    private Coroutine comboCorutine;
    private int specialAttackNr = 3;
    [SerializeField] Vector2Int comboAttackWindow = new Vector2Int(1, 00);
    private float comboAttackTime = 0f;
    [SerializeField] public bool doNextAttack;
    [SerializeField] bool isNextStrongAttack;
    public bool isWindowOpen;


    [Header("Step during attack")]
    [SerializeField] float distance = 1.0f;
    float playerRadius = 1.0f;
    [SerializeField] float stepTime = 0.4f;
    Coroutine stepCoroutine = null;
    [SerializeField] private float maxStepHight = 0.001f;
    [SerializeField] protected LayerMask _excludedLayer;

    [Header("Blocking")]
    [SerializeField] float blockDuration = 1.5f;
    private Coroutine blockRoutine;

    public event Action OnCombo3Attacks;
    public event Action OnPerfectParry;

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
        health.onAttacked.AddListener(OnAttacked);
        health.onDeath.AddListener(Die);
        equipmentController.Initialize(transform);

        stepTime = 0.1f;
        comboAttackTime = comboAttackWindow.x + (comboAttackWindow.y / 60f);

        playerRadius = GetComponent<CapsuleCollider>().radius;
    }

    private void OnDestroy()
    {
        var pm = GetComponent<PlayerMovement>();
        pm.input.OnLeftMouseClickEvent -= OnClickStart;
        pm.input.OnLeftMouseReleaseEvent -= OnClickEnd;
        pm.input.OnRightMouseClickEvent -= AltUseStart;
        pm.input.OnRightMouseReleaseEvent -= AltUseEnd;
    }
    private void Update()
    {
        if (mouseBeingHeld&&equipmentController.IsDistance())
        {
            movement.RotatePlayerTowardsMouse();
            //no tu trzeba jeszcze ze gracza zatrzymuje w miejscu podczas trzymania i zeby strzalki nie skrecaly jak
            //gracz sie obraca 
        }
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
        mouseBeingHeld = true;
        isHoldingAttack = false;
    }

    public void OnClickEnd()
    {
        float pressDuration = Time.time - attackPressTime;
        isHoldingAttack = pressDuration > whenConsiderHoldingAttack; 
        mouseBeingHeld = false;

        if (playerState.state == PlayerState.DisableInput) return;
        if (!canAttack) return;

        if (!isWindowOpen)
            comboCounter = 0;
        
        if (playerState.state == PlayerState.Attacking)
            doNextAttack = true;
        else
            StartAttackAnim();

    }

    IEnumerator ComboWindow()
    {
        isWindowOpen = true;
        yield return new WaitForSeconds(comboAttackTime);
        isWindowOpen = false;
    }
    

    public void Attack()
    {
        if (isHoldingAttack) {
            equipmentController.UseWeaponStrongStart(rotatingRootTransform);
        }
        else if (comboCounter == specialAttackNr) {
            equipmentController.UseWeaponSpecialAttack(rotatingRootTransform);
            // Tu wywołujemy event combo 3 ataków
            OnCombo3Attacks?.Invoke();
            if (shockWave)
                shockWave.Play();// To po spadnięciu
        } 
        else {
            equipmentController.UseWeaponStart(rotatingRootTransform);
        }
        if(!equipmentController.IsDistance())
            stepCoroutine = StartCoroutine(MoveForwardSmooth(transform, distance, stepTime));
    }  

    public void StartAttackAnim()
    {
        if (playerState.state == PlayerState.Dashing)
        {
            queuedAttack = true;
            return;
        }
      
        if (comboCorutine != null)
        {
            StopCoroutine(comboCorutine);
        }
        comboCorutine = StartCoroutine(ComboWindow());
        comboCounter++;
        playerState.state = PlayerState.Attacking;
        movement.isSprinting = false;
        movement.PlayerAnimator.SetBool("canExitAttack", false);
        movement.RotatePlayerTowardsMouse();
        movement.PlayerAnimator.SetTrigger("attackTriggerPlayer");
       // movement.WeaponAnimator.SetTrigger("atakTrigger");
    }

    public void EndAttack()
    {

        playerState.state = PlayerState.Normal;
        //movement.WeaponAnimator.SetBool("canExitAttack", true);
        movement.PlayerAnimator.SetBool("canExitAttack", true);
        if (isHoldingAttack)
        {
            equipmentController.UseWeaponStrongEnd(rotatingRootTransform);
            isHoldingAttack = false;
        }
        else if (comboCounter == specialAttackNr)
        {
            equipmentController.UseWeaponEnd(rotatingRootTransform);
            comboCounter = 0;
        }
        else
        {
            equipmentController.UseWeaponEnd(rotatingRootTransform);
        }
    }

    public bool IsDistance()
    {
        return equipmentController.IsDistance();
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

            // SphereCast jak w NormalMovement
            Ray ray = new Ray(nextPos + Vector3.up * 0.4f, GetRotatingRootForward());
            if (Physics.SphereCast(ray, 0.4f, out hit, playerRadius + 0.1f, ~_excludedLayer, QueryTriggerInteraction.Ignore)
                && hit.normal.y <= 0.7f)
            {
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
        if (playerState.state == PlayerState.DisableInput||movement.isSprinting||playerState.state == PlayerState.Dashing) return;
        isBlocking = true;
        movement.PlayerAnimator.SetBool("isBlocking", true);
        
        movement.PlayerAnimator.SetTrigger("blockTrigger");

        CheckForAttackingEnemies(isBlocking);
        blockRoutine = StartCoroutine(StopBlockingAfterTime());
    }


    IEnumerator StopBlockingAfterTime()
    {
        yield return new WaitForSeconds(blockDuration);
        StopBlocking();
    }

    public void StopBlocking()
    {
        if (playerState.state == PlayerState.DisableInput) return;
        isBlocking = false;
        movement.PlayerAnimator.SetBool("isBlocking", false);
    }


    public void OnAttacked(AttackData ad, bool blockable)
    {
        if ((!isBlocking && blockable) || !blockable)
        {
            AttackData attackData = ad;
            if(equipmentController.GetDefensiveCharm() != null)
            {
                attackData = equipmentController.GetDefensiveCharm().CharmEffectOnArmor(attackData);
            }
            health.BaseSimpleDamage(attackData);
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
                var enemyCombat = obj.GetComponent<EnemyCombat>();
                var enemyMovement = obj.GetComponent<EnemyMovement>();

                if (enemyCombat != null && enemyMovement != null && enemyCombat.isPerfectBlockWindow)
                {
                    enemyMovement.perfectParWasInitiated = true;
                    movement.PlayerAnimator.SetTrigger("parryTrigger");
                    //movement.WeaponAnimator.SetTrigger("parryTrigger");

                    OnPerfectParry?.Invoke();  // <<< WYWOŁANIE EVENTU
                }
            }
        }
    }

}
