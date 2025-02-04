using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] Vector2 movementInput;
    [SerializeField, Range(1f, 10f)] float speed;
    [SerializeField, Range(1f, 5f)] float sprintMod;
    [SerializeField, Range(0.0f, 1f)] float lerpHalfTime = 0.1f;
    [SerializeField] Transform modelRootTransform;

    private CharacterController cc;
    private Vector2 lerpedMove;
    Vector2 mousePosition;
    bool isSprinting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();
        var playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Move"].performed += OnMoveInput;
        playerInput.actions["Move"].canceled += OnMoveInput;
        playerInput.actions["Look"].performed += OnMousePosition;
        playerInput.actions["Sprint"].performed += OnSprint;
        playerInput.actions["Sprint"].canceled += OnSprint;
    }

    void FixedUpdate()
    {
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

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movementInput = context.ReadValue<Vector2>();
        }
        if (context.canceled)
        {
            movementInput = Vector2.zero;
        }
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            mousePosition = context.ReadValue<Vector2>();
            // Normlize the mouse position to (-1, 1)
            mousePosition = new Vector2(((mousePosition.x / Screen.width) - 0.5f) * 2.0f, ((mousePosition.y / Screen.height) - 0.5f) * 2.0f);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprinting = true;
        }
        if (context.canceled)
        {
            isSprinting = false;
        }
    }
}
