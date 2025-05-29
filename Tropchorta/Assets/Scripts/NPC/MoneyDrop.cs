using System.Collections;
using UnityEngine;

public class MoneyDrop : MonoBehaviour
{
    [SerializeField] int value;
    public float delay = 0.2f;
    private Transform player;
    private bool isTaken = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        value = Random.Range(1, 4);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTaken)
        {
            isTaken = true;
            player = other.transform;
            EquipmentController equipment = player.GetComponentInChildren<EquipmentController>();
            if (equipment != null)
            {
                //Debug.Log("Add value " + value);
                equipment.AddGold(value);
            }
            StartCoroutine(MoveToPlayerAfterDelay());
        }
    }

    private IEnumerator MoveToPlayerAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        float currentSpeed = 0f;
        float acceleration = 5f;

        while (Vector3.Distance(transform.position, player.position) > 0.1f)
        {
            currentSpeed += acceleration * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}
