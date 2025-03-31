using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : MonoBehaviour
{
    Transform _target;
    Vector3 _targetPosition;

    [SerializeField] float _speed = 4f;
    [SerializeField] float _stoppingDistance = 0.9f;

    bool _isMoving = false;
    bool _isChasing = false;
    bool _attackInterrupted = false;

    public void RotateTowards(GameObject obj) 
    {
        if (obj == null) return; // Safety check  

        Vector3 direction = (obj.transform.position - transform.position).normalized; // Get direction  
        Quaternion lookRotation = Quaternion.LookRotation(direction); // Calculate rotation  

        Quaternion newRotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f); // Smooth rotation 
        transform.rotation = Quaternion.Euler(0.0f, newRotation.eulerAngles.y, 0.0f);
    }

    public void MoveToPosition(Vector3 newTargetPosition)
    {

        _targetPosition = newTargetPosition;
        _targetPosition.y = this.transform.position.y;
        _isMoving = true;
    }

    public void StopMoving()
    {
        _isMoving = false;
    }

    // Method to dynamically change the target
    public void StartChasing(Transform newTarget)
    {
        _target = newTarget;
        _isChasing = true;
        MoveToPosition(newTarget.position);
    }

    // Optionally, you can stop chasing
    public void StopChasing()
    {
        _isChasing = false;
        _isMoving = false;
    }

    public void AttackStarted()
    {
        _attackInterrupted = true;
    }
    public void AttackFinished()
    {
        _attackInterrupted = false;
    }

    // TODO : this can be reworked to use a state machine or a ENUM state swtich for easier management
    public void MoveToTarget()
    {
        if (_attackInterrupted) return;
        if (!_isChasing && transform.position == _targetPosition)
        {
            _isMoving = false;
            return;
        }
        if (_isChasing)
        {
            _targetPosition = _target.position;
            _targetPosition.y = this.transform.position.y;
            if (Vector3.Distance(transform.position, _target.position) <= _stoppingDistance)
            {
                _isMoving = false;
                return;
            }
            else
            {
                _isMoving = true;
            }
        }
        if (!_isMoving) return;
        // Poruszamy obiekt w stron� celu
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

    }

}
