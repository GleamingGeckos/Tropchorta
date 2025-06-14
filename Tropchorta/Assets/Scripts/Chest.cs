using DG.Tweening;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Chest behaviour")]
    public GameObject itemToSpawn;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float fadeDelay = 1.0f;

    [Header("Object anim")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDuration = 0.3f;
    [SerializeField] private float sideOffset = 1f;

    private bool _used = false;

    private bool _playerInArea = false;

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

        Renderer rend = GetComponent<Renderer>();
        //rend.material.SetInt("_TransparentEnabled", 1); TODO Check on that when eny error
        DOTween.To(() => rend.material.GetFloat("_Tweak_transparency"), x => rend.material.SetFloat("_Tweak_transparency", x), -1f, fadeDuration)
            .SetDelay(fadeDelay)
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

        Vector3 targetPos = spawnPos + transform.right * sideOffset;
        obj.transform.DOJump(targetPos, jumpHeight, 1, jumpDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInArea = false;
        }
    }
}
