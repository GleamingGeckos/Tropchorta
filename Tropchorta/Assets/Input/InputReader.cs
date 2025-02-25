using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject
{
    [SerializeField] private InputActionAsset inputAsset;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction leftMouseAction;
    private InputAction rightMouseAction;
    private InputAction sprintAction;
    private InputAction mapZoomInAction;
    private InputAction mapZoomOutAction;
    private InputAction toggleMapAction;
    private InputAction mouseDeltaAction;
    private InputAction scrollAction;
    private InputAction escapeAction;

    #region MouseEvents
    public UnityAction<Vector2> OnMoveInputEvent;
    public UnityAction<Vector2> OnLookInputEvent;
    public UnityAction OnLeftMouseClickEvent;
    public UnityAction OnLeftMouseReleaseEvent;
    public UnityAction OnRightMouseClickEvent;
    public UnityAction OnRightMouseReleaseEvent;
    public UnityAction<Vector2> OnMouseDeltaEvent;
    public UnityAction<float> OnScrollEvent;
    #endregion

    #region KeyboardEvents
    public UnityAction OnSprintEvent;
    public UnityAction OnSprintCancelledEvent;
    public UnityAction OnMapZoomInEvent;
    public UnityAction OnMapZoomOutEvent;
    public UnityAction OnToggleMapEvent;
    public UnityAction OnEscapeEvent;
    #endregion

    private void OnEnable()
    {
        moveAction = inputAsset.FindAction("Move");
        lookAction = inputAsset.FindAction("Look");
        leftMouseAction = inputAsset.FindAction("LeftMouse");
        rightMouseAction = inputAsset.FindAction("RightMouse");
        sprintAction = inputAsset.FindAction("Sprint");
        mapZoomInAction = inputAsset.FindAction("MapZoomIn");
        mapZoomOutAction = inputAsset.FindAction("MapZoomOut");
        toggleMapAction = inputAsset.FindAction("ToggleMap");
        mouseDeltaAction = inputAsset.FindAction("MouseDelta");
        scrollAction = inputAsset.FindAction("ScrollWheel");
        escapeAction = inputAsset.FindAction("Escape");

        moveAction.Enable();
        lookAction.Enable();
        leftMouseAction.Enable();
        rightMouseAction.Enable();
        sprintAction.Enable();
        mapZoomInAction.Enable();
        mapZoomOutAction.Enable();
        toggleMapAction.Enable();
        mouseDeltaAction.Enable();
        scrollAction.Enable();
        escapeAction.Enable();

        moveAction.performed += ctx => OnMoveInputEvent?.Invoke(ctx.ReadValue<Vector2>());
        moveAction.canceled += ctx => OnMoveInputEvent?.Invoke(Vector2.zero);

        lookAction.performed += ctx => OnLookInputEvent?.Invoke(ctx.ReadValue<Vector2>());

        leftMouseAction.performed += ctx => OnLeftMouseClickEvent?.Invoke();
        leftMouseAction.canceled += ctx => OnLeftMouseReleaseEvent?.Invoke();

        rightMouseAction.performed += ctx => OnRightMouseClickEvent?.Invoke();
        rightMouseAction.canceled += ctx => OnRightMouseReleaseEvent?.Invoke();

        sprintAction.performed += ctx => OnSprintEvent?.Invoke();
        sprintAction.canceled += ctx => OnSprintCancelledEvent?.Invoke();

        mapZoomInAction.performed += ctx => OnMapZoomInEvent?.Invoke();
        mapZoomOutAction.performed += ctx => OnMapZoomOutEvent?.Invoke();

        toggleMapAction.performed += ctx => OnToggleMapEvent?.Invoke();

        mouseDeltaAction.performed += ctx => OnMouseDeltaEvent?.Invoke(ctx.ReadValue<Vector2>());
        mouseDeltaAction.canceled += ctx => OnMouseDeltaEvent?.Invoke(Vector2.zero);

        scrollAction.performed += ctx => OnScrollEvent?.Invoke(ctx.ReadValue<float>());
        scrollAction.canceled += ctx => OnScrollEvent?.Invoke(0);

        escapeAction.performed += ctx => OnEscapeEvent?.Invoke();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        leftMouseAction.Disable();
        rightMouseAction.Disable();
        sprintAction.Disable();
        mapZoomInAction.Disable();
        mapZoomOutAction.Disable();
        toggleMapAction.Disable();
        mouseDeltaAction.Disable();
        scrollAction.Disable();
        escapeAction.Disable();
    }


}
