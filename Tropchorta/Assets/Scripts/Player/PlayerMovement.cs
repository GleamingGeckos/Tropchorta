using System.Collections;
using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Vector2 movementInput;
    [SerializeField, Range(1f, 10f)] float speed;
    [SerializeField, Range(1f, 5f)] float sprintMod;
    [SerializeField, Range(0.0f, 1f)] float lerpHalfTime = 0.1f;
    [SerializeField] float dashDistance = 5f;
    [SerializeField] float dashDuration = 0.5f;
    [SerializeField] Transform modelRootTransform;
    [SerializeField] public InputReader input;
    [SerializeField] public PlayerStateSO playerState;
    private PlayerCombat playerCombat;
    private CharacterController cc;
    private Vector2 lerpedMove;
    Vector2 mousePosition;
    bool isSprinting;
    Coroutine dashCoroutine = null;

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
    }

    void Update()
    {
        switch (playerState.state)
        {
            case PlayerState.Normal:
                NormalMovement();
                break;
            case PlayerState.DisableInput:
                // Disable input, do nothing
                break;
            case PlayerState.Attacking:
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

    void NormalMovement()
    {
        // normal state
        lerpedMove = lerpedMove.LerpFI(movementInput, Time.fixedDeltaTime, lerpHalfTime);
        Vector3 move = new Vector3(lerpedMove.x, 0, lerpedMove.y);
        cc.Move(move * speed * (isSprinting ? sprintMod : 1) * Time.deltaTime);

        // Rotate the player to face the mouse position
        if (mousePosition != Vector2.zero)
        {
            Vector3 lookDirection = new Vector3(mousePosition.x, 0, mousePosition.y);
            if (lookDirection != Vector3.zero)
            {
                modelRootTransform.forward = lookDirection;
            }
        }
    }


    IEnumerator Dash()
    {
        playerState.state = PlayerState.Dashing;
        lerpedMove = Vector2.zero;

        Vector3 direction = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        if (direction != Vector3.zero)
        {
            float elapsed = 0f;
            Vector3 startPosition = transform.position;

            while (elapsed < dashDuration)
            {
                float t = elapsed / dashDuration;
                float easedT = 1f - Mathf.Pow(1f - t, 2f); // Quadratic ease-out
                float currentDistance = easedT * dashDistance;

                Vector3 targetPosition = startPosition + direction * currentDistance;
                Vector3 move = targetPosition - transform.position;

                cc.Move(move);

                elapsed += Time.deltaTime;
                yield return null;
            }

            // Final correction to ensure exact position
            Vector3 finalTarget = startPosition + direction * dashDistance;
            Vector3 remaining = finalTarget - transform.position;
            cc.Move(remaining);
        }

        playerState.state = PlayerState.Normal;
        dashCoroutine = null;
    }

    void OnDash()
    {
        if (dashCoroutine == null)
        {
            playerCombat.StopAttack();

            dashCoroutine = StartCoroutine(Dash());
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
