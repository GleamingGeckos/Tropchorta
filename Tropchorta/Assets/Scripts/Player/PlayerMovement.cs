using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] Vector2 movementInput;
    [SerializeField, Range(1f, 10f)] float speed;
    [SerializeField, Range(1f, 5f)] float sprintMod;
    [SerializeField, Range(0.0f, 1f)] float lerpHalfTime = 0.1f;
    [SerializeField] Transform modelRootTransform;
    [SerializeField] public InputReader input;
    [SerializeField] public PlayerStateSO playerState;

    private CharacterController cc;
    private Vector2 lerpedMove;
    Vector2 mousePosition;
    bool isSprinting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();
        input.OnMoveInputEvent += OnMoveInput;
        input.OnLookInputEvent += OnMousePosition;
        input.OnSprintEvent += () => isSprinting = true;
        input.OnSprintCancelledEvent += () => isSprinting = false;
    }

    void FixedUpdate()
    {
        if (playerState.state == PlayerState.DisableInput) return;

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
