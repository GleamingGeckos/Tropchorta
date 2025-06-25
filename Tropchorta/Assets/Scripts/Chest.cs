using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Chest behaviour")]
    public GameObject itemToSpawn;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private float _fadeDelay = 1.0f;
    [SerializeField] float _objHight = 0.4f;

    [Header("Object anim")]
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _jumpDuration = 0.3f;
    [SerializeField] private float _sideOffset = 1f;

    [SerializeField] private GameObject _canvas;

    private bool _used = false;

    private bool _playerInArea = false;

    private void Start()
    {
        if (_canvas != null)
            _canvas.SetActive(false);
    }

    void Update()
    {
        if (_playerInArea && Input.GetKeyDown(KeyCode.E)) // TODO powinniœmy usystematyzowaæ input
        {
            Debug.Log("Update");
            Interact();
        }
    }

    public void Interact()
    {
        if (_used) return;
        _used = true;

        if (_canvas != null)
            _canvas.SetActive(false);

        Renderer rend = GetComponent<Renderer>();
        SpawnWithJump();
        Destroy(gameObject, _fadeDelay);
    }

    private void StartParticles()
    {

    }

    private void SpawnWithJump()
    {
        if (itemToSpawn == null) return;

        Vector3 spawnPos = transform.position;
        GameObject obj = Instantiate(itemToSpawn, spawnPos, Quaternion.identity);

        Vector3 targetPos = spawnPos + transform.right * _sideOffset + Vector3.up * _objHight;
        obj.transform.DOJump(targetPos, _jumpHeight, 1, _jumpDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enter");
            _playerInArea = true;
            if (_canvas != null)
                _canvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Exit");
            _playerInArea = false;
            if (_canvas != null)
                _canvas.SetActive(false);
        }
    }
}
