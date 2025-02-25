using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [SerializeField] private Image mapImage;
    [SerializeField] private RectTransform playerIcon;
    [SerializeField, Range(0.1f, 50.0f), Tooltip("Zoom level of the map.")] private float mapZoom = 5f;
    [SerializeField] float zoomMin = 2f;
    [SerializeField] float zoomMax = 50.0f;
    [SerializeField, Tooltip("Unity-units size of the actual terrain the map presents.")] private Vector2 mapRealSize = new Vector2(100, 100);
    [SerializeField, Tooltip("Center of the actual terrain the map represents. Coordinates represent the XZ plane size.")] private Vector2 mapWorldCenter = new Vector2(0, 0);
    [SerializeField] InputReader input;
    private PlayerMovement playerMovement;
    private Transform playerTransform;
    private RectTransform mapRectTransform;

    private float zoomPerLevel = 0.5f;
    private Vector2 playerNormalizedPosition;

    void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        playerTransform = playerMovement.transform;
        mapRectTransform = mapImage.GetComponent<RectTransform>();

        input.OnMapZoomInEvent += ZoomMapIn;
        input.OnMapZoomOutEvent += ZoomMapOut;
    }

    void LateUpdate()
    {
        if (playerTransform == null || mapRectTransform == null) return;

        // get player normalized position relative to position and size of the real world terrain
        playerNormalizedPosition.x = Mathf.Clamp01((playerTransform.position.x - mapWorldCenter.x + mapRealSize.x / 2) / mapRealSize.x);
        playerNormalizedPosition.y = Mathf.Clamp01((playerTransform.position.z - mapWorldCenter.y + mapRealSize.y / 2) / mapRealSize.y);

        // make the normalized position a map coordinates
        Vector2 mapSize = mapRectTransform.sizeDelta;
        Vector2 mapPosition = new Vector2(
            Mathf.Lerp(-mapSize.x / 2, mapSize.x / 2, playerNormalizedPosition.x),
            Mathf.Lerp(-mapSize.y / 2, mapSize.y / 2, playerNormalizedPosition.y)
        );

        // Apply position and zoom
        mapRectTransform.localPosition = -mapPosition * mapZoom;
        mapRectTransform.localScale = Vector3.one * mapZoom;

        // rotate the player icon to match the player rotation
        Vector3 rotationDirection = playerMovement.GetRotatingDirection();
        rotationDirection.y = rotationDirection.z;
        rotationDirection.z = 0.0f;
        playerIcon.up = rotationDirection;
    }

    public void ZoomMapIn()
    {
        mapZoom = Mathf.Clamp(mapZoom + zoomPerLevel, zoomMin, zoomMax);
    }

    public void ZoomMapOut()
    {
        mapZoom = Mathf.Clamp(mapZoom - zoomPerLevel, zoomMin, zoomMax);
    }

    public Vector2 GetNormalizedPlayerPosition()
    {
        return playerNormalizedPosition;
    }

    public Vector3 GetPlayerRotation()
    {
        Vector3 rotationDirection = playerMovement.GetRotatingDirection();
        rotationDirection.y = rotationDirection.z;
        rotationDirection.z = 0.0f;

        return rotationDirection;
    }
}
