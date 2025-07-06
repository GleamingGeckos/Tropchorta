using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : MonoBehaviour
{
    Transform _target;
    public NavMeshAgent agent;

    [SerializeField] float _speed = 4f;
    [SerializeField] float _stoppingDistance = 0.9f;
    [SerializeField] float _stunTime = 0.5f;
    [SerializeField] Image _stunImage;

    public bool canBeStuned = true;
    public bool isStuned = false;
    public bool perfectParWasInitiated = false;
    public bool isChasing = false;
    bool _attackInterrupted = false;
    private float _wantedYPos;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = _stoppingDistance;
        agent.speed = _speed;
        _stunImage.enabled = false;
        _wantedYPos = transform.position.y;
    }

    public void RotateTowards(GameObject obj) 
    {
        if (obj == null) return; // Safety check  

        Vector3 direction = (obj.transform.position - transform.position).normalized; // Get direction  
        Quaternion lookRotation = Quaternion.LookRotation(direction); // Calculate rotation  

        Quaternion newRotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f); // Smooth rotation 
        transform.rotation = Quaternion.Euler(0.0f, newRotation.eulerAngles.y, 0.0f);
    }

    // Method to dynamically change the target
    public void StartChasing(Transform newTarget)
    {
        _target = newTarget;
        isChasing = true;
        agent.isStopped = false;
    }

    // Optionally, you can stop chasing
    public void StopChasing()
    {
        isChasing = false;
        agent.isStopped = true;
    }

    public void Stun()
    {
        if (isChasing && canBeStuned)
        {
            perfectParWasInitiated = false;
            isStuned = true;
            _stunImage.enabled = true;
            StopChasing();
            StartCoroutine(StunWindow());
        }
    }

    private IEnumerator StunWindow()
    {
        yield return new WaitForSeconds(_stunTime);
        _stunImage.enabled = false;
        isStuned = false;
        Collider[] hits = Physics.OverlapSphere(transform.position, 6); //TODO Radius should be taken from collider
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                // Znaleziono gracza w sferze
                StartChasing(hit.transform);
                break;
            }
        }
    }

    public void AttackStarted()
    {
        _attackInterrupted = true;
        agent.isStopped = true;
    }
    public void AttackFinished()
    {
        _attackInterrupted = false;
        agent.isStopped = false;
    }

    protected void CheckYPosition()
    {
        if (Mathf.Abs(transform.position.y - _wantedYPos) > 0.01f)
        {
            Vector3 correctedPosition = new Vector3(transform.position.x, _wantedYPos, transform.position.z);
            transform.position = correctedPosition;
        }
    }
    void Update()
    {
        CheckYPosition();
        if (isChasing && _target != null)
            RotateTowards(_target.gameObject);
        if (!isChasing || _target == null || _attackInterrupted) return;
            agent.SetDestination(_target.position);
    }


}
