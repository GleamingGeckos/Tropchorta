using DG.Tweening;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Chest behaviour")]
    public GameObject itemToSpawn;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private float _fadeDelay = 1.0f;

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
        //rend.material.SetInt("_TransparentEnabled", 1); TODO Check on that when eny error
        DOTween.To(() => rend.material.GetFloat("_Tweak_transparency"), x => rend.material.SetFloat("_Tweak_transparency", x), -1f, _fadeDuration)
            .SetDelay(_fadeDelay)
            .OnStart(() =>
            {
                SpawnWithJump();
            })
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }

    private void StartParticles()
    {

    }

    private void SpawnWithJump()
    {
        if (itemToSpawn == null) return;

        Vector3 spawnPos = transform.position;
        GameObject obj = Instantiate(itemToSpawn, spawnPos, Quaternion.identity);

        Vector3 targetPos = spawnPos + transform.right * _sideOffset;
        obj.transform.DOJump(targetPos, _jumpHeight, 1, _jumpDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInArea = true;
            if (_canvas != null)
                _canvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInArea = false;
            if (_canvas != null)
                _canvas.SetActive(false);
        }
    }
}
