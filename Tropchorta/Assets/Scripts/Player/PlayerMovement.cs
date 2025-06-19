using System;
using System.Collections;
using DG.Tweening;
using FMODUnity;
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
    [NonSerialized]  public bool isMoving;

    // For dash
    [SerializeField] PlayerHealthComponent playerHealthComponent;
    [SerializeField] TrailRenderer trail;

    private PlayerCombat playerCombat;
    private CharacterController cc;
    private Vector2 lerpedMove;
    Vector2 mousePosition;
    bool isSprinting;
    Coroutine dashCoroutine = null;
    public bool useMouseRotation;

    // Sounds
    [SerializeField] EventReference footstepsSound;
    [SerializeField] EventReference dashSound;
    float footstepsTime = 0.5f;
    Coroutine footstepsCoroutine = null;

    
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
        if (lookDirection != Vector3.zero&&!isMoving)
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
                
                // WeaponAnimator.SetBool("isSprinting", isSprinting);
                // WeaponAnimator.SetBool("isMoving", isMoving);
                
                break;
            case PlayerState.DisableInput:
                StopFootstepsSound();
                // Disable input, do nothing
                break;
            case PlayerState.Attacking:
                StopFootstepsSound();
                AttackingState();
                break;
            case PlayerState.Dashing:
                // Dashing state, Dash is handled outside of Update and disables normal movement
                break;
        }
    }
    
    

    // while attacking, lerp the movement to zero
    void AttackingState()
    {
        lerpedMove = lerpedMove.LerpFI(Vector2.zero, Time.fixedDeltaTime, lerpHalfTime);
    }

    public void NormalMovement()
    {
        // normal state
        lerpedMove = lerpedMove.LerpFI(movementInput, Time.fixedDeltaTime, lerpHalfTime);
        Vector3 move = new Vector3(lerpedMove.x, 0, lerpedMove.y);

        cc.Move(move * speed * (isSprinting ? sprintMod : 1) * Time.deltaTime);
        //transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        
        if (lerpedMove.sqrMagnitude > 0.1f) // sqrt so with normal values (>1) it should always be greater than speed
        { 
            PlayFootstepsSound();
            modelRootTransform.forward = move;
        }
        else
        {
            StopFootstepsSound();
        }
    }


    IEnumerator Dash()
    {
        playerState.state = PlayerState.Dashing;
        StopFootstepsSound();

        Vector3 direction = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        if (direction == Vector3.zero)
            direction = modelRootTransform.forward;

        float dashSpeed = dashDistance / dashDuration;
        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            float t = elapsed / dashDuration;
            float easedT = 1f - Mathf.Pow(1f - t, 2f); // Quadratic ease-out
            float currentSpeed = Mathf.Lerp(dashSpeed, 7, easedT);

            cc.Move(direction * currentSpeed * Time.deltaTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        StopDash();
    }

    void OnDash()
    {
        if (dashCoroutine == null&&playerState.state != PlayerState.Attacking) //TODO:: ZAKOLEJKOWANIE DASZA JESLI KLIKNIETY PODCZAS ATAKU
        {
            dashCoroutine = StartCoroutine(Dash());
            RuntimeManager.PlayOneShot(dashSound, transform.position);
            //playerCombat.StopAttack();
            trail.enabled = true;
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
        if (dashCoroutine != null)
        {
            playerState.state = PlayerState.Normal;
            trail.enabled = false;
            playerHealthComponent.isInvulnerable = false;
            cc.includeLayers = 0;
            StopCoroutine(dashCoroutine);
            dashCoroutine = null;
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

    private IEnumerator FootstepsSound()
    {
        while (true)
        {
            RuntimeManager.PlayOneShot(footstepsSound, transform.position);
            yield return new WaitForSeconds(footstepsTime);
        }
    }

    private void PlayFootstepsSound()
    {
        if (footstepsCoroutine == null)
        {
            footstepsCoroutine = StartCoroutine(FootstepsSound());
        }
    }

    private void StopFootstepsSound()
    {
        if (footstepsCoroutine != null)
        {
            StopCoroutine(footstepsCoroutine);
            footstepsCoroutine = null;
        }
    }
}
