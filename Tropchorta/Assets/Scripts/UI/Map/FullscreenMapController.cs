using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FullscreenMapController : MonoBehaviour
{
    public RectTransform mapRectTransform;
    public RectTransform fullscreenMapRoot;
    public Image darkOverlay;
    public float zoomSpeed = 0.1f;
    public float minZoom = 0.5f, maxZoom = 2f;

    private Vector2 mouseDelta;
    private Vector2 mousePosition;
    private bool isDragging = false;
    private float currentZoom = 1f;
    private float zoomInput;
    private bool isMapOpen = false;
    private Vector2 mapStartPosition;
    private Vector2 dragStartPosition;
    private Vector2 mapInitialSize;


    private void Start()
    {
        mapInitialSize = mapRectTransform.sizeDelta;
        fullscreenMapRoot.gameObject.SetActive(false);
        darkOverlay.DOFade(0f, 0f);
        MoveMap(new Vector2(0.5f, -0.5f), 0.0f);
    }

    private void OnEnable()
    {
        var playerInput = FindAnyObjectByType<PlayerInput>();

        playerInput.actions["LeftMouse"].performed += ctx => StartDragging(ctx);
        playerInput.actions["LeftMouse"].canceled += ctx => StopDragging();

        playerInput.actions["MouseDelta"].performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
        playerInput.actions["MouseDelta"].canceled += ctx => mouseDelta = Vector2.zero;

        playerInput.actions["Look"].performed += ctx => mousePosition = ctx.ReadValue<Vector2>();

        playerInput.actions["ScrollWheel"].performed += ctx => zoomInput = ctx.ReadValue<float>();
        playerInput.actions["ScrollWheel"].canceled += ctx => zoomInput = 0f;

        playerInput.actions["ToggleMap"].performed += ctx => ToggleMap();
    }

    private void Update()
    {
        if (isMapOpen)
        {
            if (isDragging) PanMap();
            ZoomMap();
        }
    }

    private void StartDragging(InputAction.CallbackContext ctx)
    {
        if (!isMapOpen) return;

        isDragging = true;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            mapRectTransform,
            mousePosition,
            null,
            out dragStartPosition);

        mapStartPosition = mapRectTransform.anchoredPosition;
    }

    private void StopDragging()
    {
        isDragging = false;
    }

    private void PanMap()
    {
        mapRectTransform.anchoredPosition += mouseDelta * currentZoom;

        ClampPosition();
    }

    private void ZoomMap()
    {
        if (zoomInput == 0) return;

        currentZoom = Mathf.Clamp(mapRectTransform.localScale.x + zoomInput * zoomSpeed, minZoom, maxZoom);
        mapRectTransform.localScale = Vector3.one * currentZoom;

        ClampPosition();
    }

    private void ClampPosition()
    {
        float mapWidth = mapRectTransform.rect.width * mapRectTransform.localScale.x;
        float mapHeight = mapRectTransform.rect.height * mapRectTransform.localScale.y;

        float maxOffsetX = Mathf.Max(0, (mapWidth - mapInitialSize.x) / 2);
        float maxOffsetY = Mathf.Max(0, (mapHeight - mapInitialSize.y) / 2);

        float clampedX = Mathf.Clamp(mapRectTransform.anchoredPosition.x, -maxOffsetX, maxOffsetX);
        float clampedY = Mathf.Clamp(mapRectTransform.anchoredPosition.y, -maxOffsetY, maxOffsetY);

        mapRectTransform.anchoredPosition = new Vector2(clampedX, clampedY);
    }

    private void ToggleMap()
    {
        isMapOpen = !isMapOpen;

        if (isMapOpen)
        {
            fullscreenMapRoot.gameObject.SetActive(isMapOpen);
            darkOverlay.gameObject.SetActive(isMapOpen);
            darkOverlay.DOFade(0.75f, 0.5f);
            MoveMap(new Vector2(0.5f, 0.5f), 0.5f);
        }
        else
        {
            darkOverlay.DOFade(0, 0.5f).onComplete += () =>
            {
                fullscreenMapRoot.gameObject.SetActive(isMapOpen);
                darkOverlay.gameObject.SetActive(isMapOpen);
            };
            MoveMap(new Vector2(0.5f, -0.5f), 0.5f);
        }
    }

    private void MoveMap(Vector2 normalizedPosition, float time)
    {
        fullscreenMapRoot.DOAnchorMax(normalizedPosition, time);
        fullscreenMapRoot.DOAnchorMin(normalizedPosition, time);

        fullscreenMapRoot.anchoredPosition = Vector2.zero;
    }
}
