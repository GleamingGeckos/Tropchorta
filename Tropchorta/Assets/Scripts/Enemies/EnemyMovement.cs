using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : MonoBehaviour
{
    Transform _target;
    Vector3 _targetPosition;
    NavMeshAgent agent;

    [SerializeField] float _speed = 4f;
    [SerializeField] float _stoppingDistance = 0.9f;

    bool _isMoving = false;
    bool _isChasing = false;
    bool _attackInterrupted = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = _stoppingDistance;
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
        _isChasing = true;
        agent.isStopped = false;
    }

    // Optionally, you can stop chasing
    public void StopChasing()
    {
        _isChasing = false;
        agent.isStopped = true;
    }

    public void AttackStarted()
    {
        _attackInterrupted = true;
    }
    public void AttackFinished()
    {
        _attackInterrupted = false;
    }

    void Update()
    {
        if (!_isChasing || _target == null || _attackInterrupted) return;
            agent.SetDestination(_target.position);
    }
}
