using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FullscreenMapController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform mapRectTransform;
    [SerializeField] private RectTransform fullscreenMapRoot;
    [SerializeField] private RectTransform mask;
    [SerializeField] private Image darkOverlay;
    [SerializeField] private RectTransform playerIcon;
    [SerializeField] private GameObject mapIconPrefab;
    [SerializeField] private InputReader input;
    [SerializeField] private PlayerStateSO playerState;

    [Header("Zoom")]
    [SerializeField, Tooltip("Speed of scroll zoom")] private float zoomSpeed = 0.1f;
    [SerializeField] private float minZoom = 0.5f, maxZoom = 2f;
    private float currentZoom = 1f;
    private float zoomInput;

    [Header("Icons")]
    [SerializeField] private float currentIconSize = 55f;
    [SerializeField] private float minIconSize = 35f;
    [SerializeField] private float maxIconSize = 120f;
    [SerializeField] private Slider iconSizeSlider;

    private Vector2 mouseDelta;
    private Vector2 mousePosition;
    private bool isDragging = false;
    private bool hasDragged = false;
    private bool isMapOpen = false;
    private Vector2 mapInitialSize;
    private MapController mapController;
    private List<MapIcon> playerPlacedIcons = new List<MapIcon>();
    private List<MapIconButton> mapIconButtons = new List<MapIconButton>();
    private Sprite selectedIcon;

    private void Start()
    {
        playerIcon.sizeDelta = new Vector2(currentIconSize, currentIconSize);
        MapIconButton[] iconButtons = FindObjectsByType<MapIconButton>(FindObjectsSortMode.None);
        foreach (var iconButton in iconButtons)
        {
            mapIconButtons.Add(iconButton);
        }
        foreach (var iconButton in mapIconButtons)
        {
            iconButton.SetSelected(false);
        }
        mapController = GetComponentInChildren<MapController>();
        mapInitialSize = mapRectTransform.sizeDelta;
        fullscreenMapRoot.gameObject.SetActive(false);
        darkOverlay.DOFade(0f, 0f);
        MoveMap(new Vector2(0.5f, -0.5f), 0.0f);
        iconSizeSlider.minValue = minIconSize;
        iconSizeSlider.maxValue = maxIconSize;
        iconSizeSlider.value = currentIconSize;
        iconSizeSlider.onValueChanged.AddListener(OnIconSizeSliderChanged);

        input.OnLeftMouseClickEvent += StartDragging;
        input.OnLeftMouseReleaseEvent += () => { TryPlaceIcon(); StopDragging(); }; // needs to preserve order of these
        input.OnMouseDeltaEvent += delta => mouseDelta = delta;
        input.OnLookInputEvent += pos => mousePosition = pos;
        input.OnScrollEvent += delta => zoomInput = delta;
        input.OnToggleMapEvent += ToggleMap;
    }

    private void Update()
    {
        if (isMapOpen)
        {
            if (isDragging) PanMap();
            ZoomMap();

            Vector2 playerNormalizedPosition = mapController.GetNormalizedPlayerPosition();
            // move the player icon to the player position
            playerIcon.anchoredPosition = new Vector2(
                playerNormalizedPosition.x * mapRectTransform.rect.width - mapRectTransform.rect.width / 2,
                playerNormalizedPosition.y * mapRectTransform.rect.height - mapRectTransform.rect.height / 2
            );
            playerIcon.up = mapController.GetPlayerRotation();
        }
    }

    private void StartDragging()
    {
        if (!isMapOpen) return;

        isDragging = true;
    }

    private void StopDragging()
    {
        isDragging = false;
        hasDragged = false;
    }

    private void PanMap()
    {
        if (!IsMouseInsideMap()) return;
        mapRectTransform.anchoredPosition += mouseDelta;
        if (mouseDelta != Vector2.zero) hasDragged = true;

        ClampPosition();
    }

    private void ZoomMap()
    {
        if (zoomInput == 0) return;
        if (!IsMouseInsideMap()) return;

        currentZoom = Mathf.Clamp(mapRectTransform.localScale.x + zoomInput * zoomSpeed, minZoom, maxZoom);
        mapRectTransform.localScale = Vector3.one * currentZoom;
        RescaleIcons();
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
            playerState.state = PlayerState.DisableInput;

            fullscreenMapRoot.gameObject.SetActive(isMapOpen);
            darkOverlay.gameObject.SetActive(isMapOpen);
            darkOverlay.DOFade(0.75f, 0.5f);
            MoveMap(new Vector2(0.5f, 0.5f), 0.5f);
        }
        else
        {
            playerState.state = PlayerState.Normal;
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

    private void RescaleIcons()
    {
        float iconSize = currentIconSize / currentZoom;
        // make icons the same size regardless of zoom
        playerIcon.sizeDelta = new Vector2(iconSize, iconSize);
        foreach (var icon in playerPlacedIcons)
        {
            icon.SetSize(iconSize);
        }
    }

    public void OnIconButtonClicked(MapIconButton button)
    {
        if (button.IsSelected())
        {
            button.SetSelected(false);
            selectedIcon = null;
            return;
        }
        foreach (var iconButton in mapIconButtons)
        {
            iconButton.SetSelected(false);
        }
        button.SetSelected(true);
        selectedIcon = button.GetIconSprite();

        if (selectedIcon == null) return;
    }

    private void TryPlaceIcon()
    {
        if (!isMapOpen) return;
        if (hasDragged) return;

        // prevent placing icons when clicking outside of visible map
        if (!IsMouseInsideMap()) return;
        if (selectedIcon == null) return;

        Vector2 iconPosOnRect;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mapRectTransform, mousePosition, null, out iconPosOnRect);

        GameObject newIcon = Instantiate(mapIconPrefab, mapRectTransform);
        MapIcon mapIcon = newIcon.GetComponent<MapIcon>();

        mapIcon.SetSprite(selectedIcon);
        mapIcon.SetSize(currentIconSize / currentZoom);

        Vector2 iconNormalizedPosition = new Vector2(
            iconPosOnRect.x / mapRectTransform.rect.width + 0.5f,
            iconPosOnRect.y / mapRectTransform.rect.height + 0.5f
        );

        mapIcon.SetPosition(iconNormalizedPosition);
        playerPlacedIcons.Add(mapIcon);
        mapIcon.SetIndex(playerPlacedIcons.Count - 1);
        mapIcon.onDestroyed.AddListener(IconDestroyed);
    }

    private void OnIconSizeSliderChanged(float newSize)
    {
        currentIconSize = newSize;
        RescaleIcons();
    }

    private void IconDestroyed(int idx)
    {
        playerPlacedIcons.RemoveAt(idx);

        for (int i = 0; i < playerPlacedIcons.Count; i++)
        {
            playerPlacedIcons[i].SetIndex(i);
        }
    }

    private bool IsMouseInsideMap()
    {
        if (mousePosition.x > mask.position.x - mask.rect.width / 2 && mousePosition.x >= mask.position.x + mask.rect.width / 2)
        {
            return false;
        }
        if (mousePosition.y > mask.position.y - mask.rect.height / 2 && mousePosition.y >= mask.position.y + mask.rect.height / 2)
        {
            return false;
        }

        return true;
    }
}
