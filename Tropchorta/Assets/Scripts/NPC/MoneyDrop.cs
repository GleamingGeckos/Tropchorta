using System.Collections;
using UnityEngine;

public class MoneyDrop : MonoBehaviour
{
    [SerializeField] private int _value = 1;
    [SerializeField] private int _minValue = 1;
    [SerializeField] private int _maxValue = 1;
    public float delay = 0.2f;
    private Transform _player;
    private bool _isTaken = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _value = Random.Range(_minValue, _maxValue);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isTaken)
        {
            _isTaken = true;
            _player = other.transform;
            EquipmentController equipment = _player.GetComponentInChildren<EquipmentController>();
            if (equipment != null)
            {
                //Debug.Log("Add value " + value);
                equipment.AddGold(_value);
            }
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
        Destroy(gameObject);
    }
}
