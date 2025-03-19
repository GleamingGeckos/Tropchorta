using System.Collections;
using DG.Tweening;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] int _maxDamage;//inclusive
    [SerializeField] int _minDamage;//inclusive

    [SerializeField] float _howLongAttackDealsDamage; //TODO
    [SerializeField] float _intervalsBetweenAttacks;
    float _attackTimer;
    Collider[] _colliders = new Collider[16];
    [SerializeField] LayerMask _excludedLayer;

    bool _isCooldown = false;
    [SerializeField] float _cooldownInterval = 3.0f;

    [Header("Temporary")]
    [SerializeField] private RectTransform attackBar;
    private float attackBarInitialX;
    [SerializeField] private float attackBarFinalX = 0f;
    Tween barTween;


    private void Start()
    {
        attackBarInitialX = attackBar.anchoredPosition.x;
    }

    public int DealDamage()
    {
        int damage = Random.Range(_minDamage, _maxDamage + 1);
        //Debug.Log($"Attack dealt {damage} damage.");
        return damage;
    }

    public void Attack()
    {
        if (_isCooldown) return;
        // everything below this line should be later on handled by a Weapon of some sorts, this is for testing only
        Vector3 rotatingOffset = transform.forward * 1.5f;
        int hits = Physics.OverlapSphereNonAlloc(transform.position + rotatingOffset, 1f, _colliders, ~_excludedLayer); // TODO : layermask for damageable objects or enemies?
        for (int i = 0; i < hits; i++)
        {
            // Currently assuming the collider is on the same object as the HealthComponent
            if (_colliders[i].TryGetComponent(out HealthComponent healthComponent) && !_colliders[i].isTrigger)
            {
                healthComponent.BlockableDamage(new AttackData(DealDamage()));
            }
        }
        StartCoroutine(Cooldown());
        DebugExtension.DebugWireSphere(transform.position + rotatingOffset, Color.red, 1f, 1f);
    }

    private IEnumerator Cooldown()
    {
        _isCooldown = true; // Set flag to prevent multiple coroutines
        yield return new WaitForSeconds(_cooldownInterval / 2f);
        barTween = attackBar.DOAnchorPosX(attackBarFinalX, _cooldownInterval / 2f)
        .SetEase(Ease.Linear)
        .OnComplete( () => 
            {
                // reset bar
                attackBar.anchoredPosition = new Vector2(attackBarInitialX, attackBar.anchoredPosition.y);
            });
        yield return new WaitForSeconds(_cooldownInterval / 2f);
        _isCooldown = false; // Reset flag when player leaves range
    }

    private void OnDestroy()
    {
        barTween.Kill();
    }

}
