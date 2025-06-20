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
    [SerializeField] GameObject moneyPrefab;
    [SerializeField] bool distance;

    //Components
    private EnemyCombat _enemyCombat;
    private EnemyMovement _enemyMovement;
    private HealthComponent _healthComponent;

    [Header("Push on attack")]
    [SerializeField] private float pushDistance = 1.0f;
    [SerializeField] private float pushDuration = 0.1f;
    [SerializeField] private float pushRadius = 1.0f;
    [SerializeField] private LayerMask pushCollisionMask = default;

    void Start()
    {
        material = new Material(GetComponentInChildren<Renderer>().sharedMaterial);
        GetComponentInChildren<Renderer>().material = material;
        _enemyCombat = GetComponent<EnemyCombat>();
        _enemyMovement = GetComponent<EnemyMovement>();
        _healthComponent = GetComponent<HealthComponent>();
        originalColor = material.color; // Store the original color
        _healthComponent.afterAttack.AddListener(AfterAttack);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if (collision.gameObject.tag == "Player")
        // {
        //     collision.gameObject.GetComponent<HealthComponent>().Damage(_enemyCombat.DealDamage());
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && _enemyMovement != null && !_enemyMovement.isStuned)
        {
            _enemyMovement.StartChasing(other.gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _enemyMovement.StopChasing();   
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && _enemyMovement != null && !_enemyMovement.isStuned)
        {
            GameObject player = other.gameObject;
            float distanceSqr = (player.transform.position - transform.position).sqrMagnitude;

            if (!distance && distanceSqr < 6.0f)
            {
                if (Random.value < 0.7f)
                    _enemyCombat.Attack(); // czasami bli¿szy
                else
                    _enemyCombat.StrongAttack(); // czasami dalszy
            }
            else if (distance)
            {
                _enemyCombat.DistanceAttack();
            }
            _enemyMovement.RotateTowards(player);
        }
    }
  
    public void OnDeath()
    {
        Instantiate(moneyPrefab, transform.position, Quaternion.identity);
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
