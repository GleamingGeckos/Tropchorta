using DG.Tweening;
using FMODUnity;
using System;
using System.Collections;
using Unity.Burst;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Vector2 movementInput;
    [SerializeField, Range(1f, 10f)] float speed;
    [SerializeField, Range(1f, 5f)] float sprintMod;
    [SerializeField, Range(0.0f, 1f)] float lerpHalfTime = 0.1f;
    [SerializeField] float dashDistance = 15f;
    [SerializeField] float dashDuration = 0.3f;
    [SerializeField] Transform modelRootTransform;
    [SerializeField] public InputReader input;
    [SerializeField] public PlayerStateSO playerState;
    [NonSerialized] public bool isMoving;
    [SerializeField] private float maxStepHight = 0.001f;
    [SerializeField] protected LayerMask _excludedLayer;
    [SerializeField] protected float _wantedYPos;

    // For dash
    [SerializeField] PlayerHealthComponent playerHealthComponent;
    [SerializeField] TrailRenderer trail;

    private PlayerCombat playerCombat;
    private CharacterController cc;
    private Vector2 lerpedMove;
    Vector2 mousePosition;
    [NonSerialized] public bool isSprinting;
    Coroutine dashCoroutine = null;
    public bool useMouseRotation;

    // Sounds
    [SerializeField] EventReference dashSound;


    //Animator
    public Animator PlayerAnimator;
    [SerializeField] GameObject hobbyHorseMesh;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCombat = GetComponent<PlayerCombat>();
        cc = GetComponent<CharacterController>();
        input.OnMoveInputEvent += OnMoveInput;
        input.OnLookInputEvent += OnMousePosition;
        input.OnSprintEvent += () => isSprinting = true;
        input.OnSprintCancelledEvent += () => isSprinting = false;
        input.OnSpaceEvent += OnDash;
        //WeaponAnimator = playerCombat.weaponSlot.GetComponentInChildren<Animator>();
    }

    public void RotatePlayerTowards(Vector3 lookDirection)
    {
        if (lookDirection != Vector3.zero && !isMoving)
        {
            Vector3 direction = lookDirection - modelRootTransform.position;
            //modelRootTransform.forward = new Vector3(direction.x, 0f, direction.z);
            //Vector3 targetPos = new Vector3(direction.x, 0f, direction.z);

            Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z).normalized;
            Vector3 targetPosition = modelRootTransform.position + flatDirection;

            modelRootTransform
                .DOLookAt(targetPosition, 0.1f)
                .SetEase(Ease.OutSine);

        }
    }

    public void RotatePlayerTowardsMouse()
    {
        if (mousePosition != Vector2.zero)
        {
            Vector3 lookDirection = new Vector3(mousePosition.x, 0, mousePosition.y);
            if (lookDirection != Vector3.zero)
            {
                //modelRootTransform.forward = lookDirection;
                modelRootTransform.DOLookAt(modelRootTransform.position + lookDirection, 0.1f);
            }
        }
    }

    void Update()
    {
        if (PauseController.IsPaused) return;
        isMoving = movementInput != Vector2.zero;


        if (isSprinting && isMoving)
        {
            hobbyHorseMesh.transform.localScale = new Vector3(1, 1, 1);
            playerCombat.StopBlocking();
        }
        else
        {
            hobbyHorseMesh.transform.localScale = new Vector3(0, 0, 0);

        }


        switch (playerState.state)
        {
            case PlayerState.Normal:
                NormalMovement();
                PlayerAnimator.SetBool("isSprinting", isSprinting);
                PlayerAnimator.SetBool("isMoving", isMoving);
                PlayerAnimator.SetBool("isDashing", false);


                // WeaponAnimator.SetBool("isSprinting", isSprinting);
                // WeaponAnimator.SetBool("isMoving", isMoving);

                break;
            case PlayerState.DisableInput:
                // Disable input, do nothing
                break;
            case PlayerState.Attacking:
                AttackingState();
                break;
            case PlayerState.Dashing:
                // Dashing state, Dash is handled outside of Update and disables normal movement
                PlayerAnimator.SetBool("isDashing", true);
                break;
        }
    }



    // while attacking, lerp the movement to zero
    void AttackingState()
    {
        lerpedMove = lerpedMove.LerpFI(Vector2.zero, Time.fixedDeltaTime, lerpHalfTime);
    }
    protected void CheckYPosition()
    {
        if (Mathf.Abs(transform.position.y - _wantedYPos) > 0.01f)
        {
            Vector3 correctedPosition = new Vector3(transform.position.x, _wantedYPos, transform.position.z);
            transform.position = correctedPosition;
        }
    }
    public void NormalMovement()
    {
        // normal state
        lerpedMove = lerpedMove.LerpFI(movementInput, Time.fixedDeltaTime, lerpHalfTime);

        Vector3 flatMove = new Vector3(lerpedMove.x, 0, lerpedMove.y).normalized;
        Vector3 desiredMove = new Vector3(lerpedMove.x, 0, lerpedMove.y) * speed * (isSprinting ? sprintMod : 1);
        Ray ray = new Ray(transform.position + Vector3.up * maxStepHight, flatMove);
        if (!Physics.SphereCast(ray, 0.4f, out RaycastHit hit, 0.5f, ~_excludedLayer, QueryTriggerInteraction.Ignore) || hit.normal.y > 0.7f)
        {
            cc.Move(desiredMove * Time.deltaTime);
        }
        else
        {
            Debug.Log("SphereCast trafi� w: " + hit.collider.name);
        }
        Debug.DrawRay(ray.origin, ray.direction * 0.5f, Color.red, 0.1f);

        CheckYPosition();

        if (lerpedMove.sqrMagnitude > 0.1f) // sqrt so with normal values (>1) it should always be greater than speed
        {
            modelRootTransform.forward = flatMove;
        }
    }


    IEnumerator Dash()
    {
        playerState.state = PlayerState.Dashing;

        playerCombat.StopBlocking();
        PlayerAnimator.SetTrigger("dashTrigger");

        Vector3 direction = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        if (direction == Vector3.zero)
            direction = modelRootTransform.forward;

        float dashSpeed = dashDistance / dashDuration;
        float elapsed = 0f;

        modelRootTransform.DOLookAt(modelRootTransform.position + direction, 0.1f);

        while (elapsed < dashDuration)
        {
            float t = elapsed / dashDuration;
            float easedT = 1f - Mathf.Pow(1f - t, 2f); // Quadratic ease-out
            float currentSpeed = Mathf.Lerp(dashSpeed, 7, easedT);

            // Obstacle check using SphereCast
            Ray ray = new Ray(transform.position + Vector3.up * maxStepHight, direction);
            if (Physics.SphereCast(ray, 0.4f, out RaycastHit hit, currentSpeed * Time.deltaTime + 0.1f, ~_excludedLayer, QueryTriggerInteraction.Ignore)
                && hit.normal.y <= 0.7f)
            {
                StopDash();
                yield break;
            }

            cc.Move(direction * currentSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        StopDash();
    }


    void OnDash()
    {
        if (dashCoroutine == null && playerState.state != PlayerState.Attacking) //TODO:: ZAKOLEJKOWANIE DASZA JESLI KLIKNIETY PODCZAS ATAKU
        {
            isSprinting = false;
            PlayerAnimator.SetBool("isSprinting", isSprinting);

            dashCoroutine = StartCoroutine(Dash());
            RuntimeManager.PlayOneShot(dashSound, transform.position);
            //playerCombat.StopAttack();
            playerHealthComponent.isInvulnerable = true;
            cc.excludeLayers = LayerMask.GetMask("Enemy");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        StopDash();
    }

    private void StopDash()
    {
        playerState.state = PlayerState.Normal;
        if (dashCoroutine != null)
        {

            playerHealthComponent.isInvulnerable = false;
            cc.includeLayers = 0;
            StopCoroutine(dashCoroutine);
            dashCoroutine = null;
            if (playerCombat.queuedAttack)
            {
                playerCombat.queuedAttack = false;
                playerCombat.StartAttackAnim();
            }
        }
    }

    public void OnMoveInput(Vector2 move)
    {
        movementInput = move;
    }

    public void OnMousePosition(Vector2 newMousePosition)
    {
        mousePosition = newMousePosition;
        // Normalize the mouse position to (-1, 1)
        mousePosition = new Vector2(((mousePosition.x / Screen.width) - 0.5f) * 2.0f, ((mousePosition.y / Screen.height) - 0.5f) * 2.0f);
    }

    public Vector3 GetRotatingDirection()
    {
        return modelRootTransform.forward;
    }
}
