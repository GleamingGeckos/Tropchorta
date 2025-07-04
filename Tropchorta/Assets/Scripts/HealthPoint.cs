using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HealthPoint : MonoBehaviour
{
    [SerializeField] int _healValue = 1;
    public float delay = 0.1f;
    private Transform _player;
    private bool _isTaken = false;
    [SerializeField] float _rotationSpeed = 90.0f;


    private void Update()
    {
        transform.Rotate(0f, _rotationSpeed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isTaken)
        {
            _isTaken = true;
            _player = other.transform;
            StartCoroutine(MoveToPlayerAfterDelay());
        }
    }

    private IEnumerator MoveToPlayerAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        float currentSpeed = 0f;
        float acceleration = 5f;

        while (Vector3.Distance(transform.position, _player.position) > 0.1f)
        {
            currentSpeed += acceleration * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _player.position, currentSpeed * Time.deltaTime);
            yield return null;
        }
        if (_player.gameObject.TryGetComponent(out HealthComponent healthComponent))
        {
            healthComponent.Heal(_healValue);
        }
        Destroy(gameObject);
    }
}
