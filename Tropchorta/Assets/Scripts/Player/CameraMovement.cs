using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    #region SerializeFields
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    [SerializeField, Range(0.01f, 1f)] float lerpHalfTime = 0.1f;
    [SerializeField] InputReader input;
    [SerializeField] PlayerStateSO playerState;

    [Header("Look Influence")]
    [SerializeField, Range(0.0f, 20f), Tooltip("Total look influence in calcualtions of camera offset")] float lookInfluence = 1f;
    [SerializeField, Range(0.0f, 4f), Tooltip("Strength of the vertical look influence")] float verticalLookInfluence = 1f;
    [SerializeField, Range(0.0f, 4f), Tooltip("Strength of the horizontal look influence")] float horizontalLookInfluence = 1f;

    #endregion

    #region privates
    // most are just cached for perf reasons

    Camera cam;
    Transform camTr;
    Vector2 mousePosition;
    Vector3 lerpedPosition;
    Vector2 forwardXZProjected;
    Vector2 rightXZProjected;
    Vector2 mouseOffset;
    Vector3 targetPosition;
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
        input.OnLookInputEvent += OnMousePosition;
    }

    // this is FixedUpdate, the same as movement of the player.
    // if you ever change this to Update, or the player's movement to Update, remember that both need to be the same
    // otherwise you might notice jittering of the player.
    void LateUpdate()
    {
        if (playerState.state == PlayerState.DisableInput) return;

        // We only want to move the camera on the XZ plane, assuming it will never need to move up/down
        forwardXZProjected = new Vector2(player.forward.x, player.forward.z).normalized * verticalLookInfluence;
        rightXZProjected = new Vector2(player.right.x, player.right.z).normalized * horizontalLookInfluence;

        mouseOffset = (forwardXZProjected * mousePosition.y + rightXZProjected * mousePosition.x) * lookInfluence;

        targetPosition = player.position + offset + new Vector3(mouseOffset.x, 0, mouseOffset.y);
        transform.position = transform.position.LerpFI(targetPosition, Time.deltaTime, lerpHalfTime);
    }

    public void OnMousePosition(Vector2 newMousePosition)
    {
        mousePosition = newMousePosition;
        // Normlize the mouse position to (-1, 1)
        mousePosition = new Vector2(((mousePosition.x / Screen.width) - 0.5f) * 2.0f, ((mousePosition.y / Screen.height) - 0.5f) * 2.0f);
    }
}
