using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class BaseBoss : BaseEnemy
{
    private bool _delay = false;

    [SerializeField] int spitCount = 2;
    [SerializeField] int punchCount = 1;
    [SerializeField] int jumpCount = 1;
    public GameObject endUI;

    private List<int> attackSet = new();

    public BossCombat bossCombat;
    
    private void Update()
    {
        if (_enemyMovement.agent.hasPath)
        {
            bossCombat.AlbastAnimator.SetBool("isWalking",true);
        }
        else
        {
            bossCombat.AlbastAnimator.SetBool("isWalking", false);
        }
    }

    void InitAttackSet()
    {
        attackSet.Clear();
        attackSet.AddRange(Enumerable.Repeat(1, spitCount));
        attackSet.AddRange(Enumerable.Repeat(2, punchCount));
        attackSet.AddRange(Enumerable.Repeat(3, jumpCount));
    }

    int GetRandomAttack()
    {
        if (attackSet.Count == 0)
            InitAttackSet();

        int index = Random.Range(0, attackSet.Count);
        int attack = attackSet[index];
        attackSet.RemoveAt(index);
        return attack;
    }
    // Update is called once per frame
    override protected void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && _enemyMovement != null && !_delay && !_enemyCombat.isCooldown)
        {
            //if (_enemyMovement.agent.isOnNavMesh && !_enemyMovement.agent.isStopped && _enemyMovement.agent.hasPath && _enemyMovement.agent.remainingDistance > _enemyMovement.agent.stoppingDistance)
            //{
            //    _enemyCombat.EnemyAnimator.SetBool("isWalking", true);
            //}
            //else
            //{
            //    _enemyCombat.EnemyAnimator.SetBool("isWalking", false);
            //}

            attackTimer += Time.deltaTime;

            if (attackTimer < _intervalsBetweenAttacks) return;
            attackTimer = 0f;
            BossCombat bossCombat = (BossCombat)_enemyCombat;
            GameObject player = other.gameObject;
            float distanceSqr = (player.transform.position - transform.position).sqrMagnitude;

            int value = GetRandomAttack();
            if (value == 1) // Plucie
            {
                if (distanceSqr < 15.0f)
                {
                    Vector3 backDirection = -transform.forward;
                    Vector3 retreatPosition = transform.position + backDirection * 15.0f; 
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(retreatPosition, out hit, 3f, NavMesh.AllAreas))
                    {
                        _delay = true;
                        _enemyMovement.isChasing = false;
                        _enemyMovement.agent.updateRotation = false;
                        _enemyMovement.agent.isStopped = false; 
                        _enemyMovement.agent.ResetPath();
                        _enemyMovement.agent.SetDestination(hit.position);
                        StartCoroutine(DelayedDistanceAttack(other.transform, 0.3f));
                    }
                }
                else
                {
                    //_enemyMovement.agent.isStopped = true;
                    bossCombat.DistanceAttack(other.transform);
                }

            }
            else if (value == 2)
            {
                float distance = Vector3.Distance(transform.position, other.transform.position);
                if (distance > 10f)
                {
                    _delay = true;
                    _enemyMovement.isChasing = false;
                    _enemyMovement.agent.updateRotation = false;
                    _enemyMovement.agent.isStopped = false;
                    _enemyMovement.agent.ResetPath();
                    _enemyMovement.agent.SetDestination(other.transform.position);
                    StartCoroutine(DelayedPunchAttack(other.transform));
                }
                else
                {
                    bossCombat.PunchAttack(other.transform);
                }
            }
            else
            {
                bossCombat.JumpAttack(other.transform);
            }
            _enemyMovement.RotateTowards(player);
        }
    }

    public void OnDeath()
    {
        Destroy(gameObject);
        endUI.SetActive(true);
        
    }

    private IEnumerator DelayedDistanceAttack(Transform target, float delay)
    {
        BossCombat bossCombat = (BossCombat)_enemyCombat;
        yield return new WaitForSeconds(delay);
        bossCombat.DistanceAttack(target);
        _delay = false;
    }

    private IEnumerator DelayedPunchAttack(Transform target)
    {
        BossCombat bossCombat = (BossCombat)_enemyCombat;
        yield return new WaitUntil(() =>
                Vector3.Distance(transform.position, target.position) <= 5f ||
        !_enemyMovement.agent.hasPath || _enemyMovement.agent.pathStatus != NavMeshPathStatus.PathComplete);
        bossCombat.PunchAttack(target);
        _delay = false;
    }
}
