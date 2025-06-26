using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BaseEnemy : MonoBehaviour
{
    private Material material;
    private Color originalColor;
    private Color flashColor = new Color(1f, 0f, 0f); // Red color
    private float transitionDuration = 1f;
    private Tween colorTween;
    private Tween shakeTween;

    [SerializeField] GameObject[] spawnPrefabs;
    [SerializeField] bool distance;
    [SerializeField] bool strong;

    //Components
    protected EnemyCombat _enemyCombat;
    protected EnemyMovement _enemyMovement;
    protected HealthComponent _healthComponent;

    [Header("Push on attack")]
    [SerializeField] private float pushDistance = 1.0f;
    [SerializeField] private float pushDuration = 0.1f;
    [SerializeField] private float pushRadius = 1.0f;
    [SerializeField] private LayerMask pushCollisionMask = default;

    
    private void Update()
    {
        if (_enemyMovement.agent.hasPath)
        {
            _enemyCombat.EnemyAnimator.SetBool("isWalking", true);

        }
        else
        {
            _enemyCombat.EnemyAnimator.SetBool("isWalking", false);
        }
    }
    
    protected void Start()
    {
        material = new Material(GetComponentInChildren<Renderer>().sharedMaterial);
        GetComponentInChildren<Renderer>().material = material;
        _enemyCombat = GetComponent<EnemyCombat>();
        _enemyMovement = GetComponent<EnemyMovement>();
        _healthComponent = GetComponent<HealthComponent>();
        originalColor = material.color; // Store the original color
        _healthComponent.afterAttack.AddListener(AfterAttack);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && _enemyMovement != null && !_enemyMovement.isStuned)
        {
            _enemyMovement.StartChasing(other.gameObject.transform);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && _enemyMovement != null)
        {
            _enemyMovement.StopChasing();   
        }
    }

    virtual protected void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && _enemyMovement != null && !_enemyMovement.isStuned)
        {
            GameObject player = other.gameObject;
            float distanceSqr = (player.transform.position - transform.position).sqrMagnitude;

            if (!distance && distanceSqr < 6.0f)
            {
                if (Random.value < 0.7f)
                    _enemyCombat.Attack(); // czasami bli�szy
                else if(strong)
                    _enemyCombat.StrongAttack(); // czasami dalszy
            }
            else if (distance)
            {
                _enemyCombat.DistanceAttack(other.transform);
            }
            _enemyMovement.RotateTowards(player);
        }
    }
  
    public void OnDeath()
    {
        int index = Random.Range(0, spawnPrefabs.Length);
        Instantiate(spawnPrefabs[index], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void OnTakeDamage()
    {
        if (material != null)
        {
            Color change = flashColor;
            material.color = flashColor;
            if (colorTween.IsActive())
            {
                // Kill the tween before starting a new one
                colorTween.Kill();
            }
            colorTween = DOTween.To(() => change, c =>
            {
                change = c;
                material.SetColor("_BaseColor", c);
            }, originalColor, transitionDuration);
            //material.DOColor(originalColor, transitionDuration);
        }

        // Shake pozycji
        if (shakeTween.IsActive())
        {
            shakeTween.Kill(true);
        }

        // Shake obiektu (np. transform this)
        //shakeTween = transform.DOShakePosition(0.3f, 0.2f, 10, 90, false, true);
    }

    public void AfterAttack(AttackData ad)
    {
        Vector3 direction = transform.position - ad.attacker.transform.position;
        direction.y = 0f;
        direction = direction.normalized; 
        _enemyCombat.PushBack(direction, pushDistance, pushDuration, pushRadius, pushCollisionMask);
    }
}
