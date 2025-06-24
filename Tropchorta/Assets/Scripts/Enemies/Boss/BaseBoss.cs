using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class BaseBoss : BaseEnemy
{
    private bool _delay = false;

    // Update is called once per frame
    override protected void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && _enemyMovement != null && !_delay)
        {
            BossCombat bossCombat = (BossCombat)_enemyCombat;
            GameObject player = other.gameObject;
            float distanceSqr = (player.transform.position - transform.position).sqrMagnitude;

            int value = Random.Range(1, 4);
            if (value == 1) // Plucie
            {
                if (distanceSqr < 10.0f)
                {
                    Vector3 backDirection = -transform.forward;
                    Vector3 retreatPosition = transform.position + backDirection * 30.0f; 
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(retreatPosition, out hit, 3f, NavMesh.AllAreas))
                    {
                        _delay = true;
                        _enemyMovement.agent.updateRotation = false;
                        _enemyMovement.isChasing = false;
                        _enemyMovement.agent.isStopped = false; 
                        _enemyMovement.agent.ResetPath();
                        _enemyMovement.agent.SetDestination(hit.position);
                        //Debug.Log("SetDestination to: " + hit.position);
                        //Debug.Log("Agent path status: " + _enemyMovement.agent.pathStatus);
                        DelayedAttack(other.transform, 0.3f);
                    }
                }
                else
                {
                    bossCombat.DistanceAttack(other.transform);
                }

            }
            else if (value == 2 && distanceSqr < 10.0f)
            {
                bossCombat.PunchAttack(other.transform);
            }
        }
    }

    private IEnumerator DelayedAttack(Transform target, float delay)
    {
        Debug.Log("Start");
        BossCombat bossCombat = (BossCombat)_enemyCombat;
        yield return new WaitForSeconds(delay);
        bossCombat.DistanceAttack(target);
        Debug.Log("Hello");
        _delay = false;
    }
}
