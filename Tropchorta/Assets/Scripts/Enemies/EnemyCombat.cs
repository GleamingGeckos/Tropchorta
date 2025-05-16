using System.Collections;
using DG.Tweening;
using FMOD;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class EnemyCombat : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] int _maxDamage;//inclusive
    [SerializeField] int _minDamage;//inclusive

    [SerializeField] float _howLongAttackDealsDamage; //TODO
    [SerializeField] float _intervalsBetweenAttacks;

    Collider[] _colliders = new Collider[16];
    [SerializeField] LayerMask _excludedLayer;
    private Coroutine attackCoroutine;

    bool isCooldown = false;
    public bool isAttacking = false;
    [SerializeField, Tooltip("this should be longer than the attack animation itself")] float _cooldownInterval = 3.0f;

    [Header("Attack signal")]
    [SerializeField] private GameObject _circles;
    [SerializeField] private RectTransform _attackCircleTransform;
    [SerializeField] private Image _attackCircleImage;
    [SerializeField] private float _minCircle;
    [SerializeField] private float _maxCircle;
    [SerializeField] private Color _minColorCircle;
    [SerializeField] private Color _maxColorCircle;
    [SerializeField] private Vector2 _perfectBlockWindow;
    private float _perfectBlockInSeconds;
    public bool isPerfectBlockWindow;
    Tween circleTween;

    [Header("Temporary")]
    [SerializeField] private float attackBarFinalX = 0f;


    [SerializeField] Animator animator;

    [SerializeField, Tooltip("This is a unit from Animation timeline Unit where you input [seconds:...something] and that someting is not ms because it can only reach up to 60. Meaning if an event happens at 1:30 this is treated as 1.5s because the 30 means half a second.")]
    Vector2Int timeFromStartToAttackInUnityTimeline = new Vector2Int(0, 50);
    [SerializeField, Tooltip("Just like variable above, this is in animation timeline units. Specifies when the enemy stops moving from the animation start in timeline units. SHOULD BE LESS THAN timeFromStartToAttackInUnityTimeline.")]
    Vector2Int offsetFromAnimStartToMovementStop = new Vector2Int(0, 50);
    float moveStopOffset = 0.45f;
    private float timeToAttackInSeconds = 0f;
    EnemyMovement enemyMovement;

    private void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        timeToAttackInSeconds = timeFromStartToAttackInUnityTimeline.x + (timeFromStartToAttackInUnityTimeline.y / 60f);
        moveStopOffset = offsetFromAnimStartToMovementStop.x + (offsetFromAnimStartToMovementStop.y / 60f);
        _circles.SetActive(false);

        _perfectBlockInSeconds = _perfectBlockWindow.x + (_perfectBlockWindow.y / 60f);
        isPerfectBlockWindow = false;
        // convert ms to normalized time
    }

    public int DealDamage()
    {
        int damage = Random.Range(_minDamage, _maxDamage + 1);
        //Debug.Log($"Attack dealt {damage} damage.");
        return damage;
    }

    public void Attack()
    {
        if (isCooldown) return;
        attackCoroutine = StartCoroutine(AttackRoutine());
        StartCoroutine(PerfectBlockWindow());
    }

    public void WasBlocked()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    private IEnumerator PerfectBlockWindow()
    {
        yield return new WaitForSeconds(_cooldownInterval - _perfectBlockInSeconds);
        Debug.Log("Start window");
        isPerfectBlockWindow = true;
        yield return new WaitForSeconds(_perfectBlockInSeconds);
        Debug.Log("Stop window");
        isPerfectBlockWindow = false;
    }

    private IEnumerator AttackRoutine()
    {
        _circles.SetActive(true);
        isCooldown = true; // Set flag to prevent multiple coroutines
        float time = _cooldownInterval - timeToAttackInSeconds;
        yield return new WaitForSeconds(time);

        // start the circle animation
        circleTween = _attackCircleTransform.DOScale(_minCircle, timeToAttackInSeconds)
        .SetEase(Ease.Linear)
        .OnUpdate(() =>
        {
            float t = (_attackCircleTransform.localScale.x - _minCircle) / (_maxCircle - _minCircle);
            _attackCircleImage.color = Color.Lerp(_minColorCircle, _maxColorCircle, t);
        })
        .OnComplete(() =>
        {
            _circles.SetActive(false);
            _attackCircleTransform.localScale = new Vector3(_maxCircle, _maxCircle, 1f);
            _attackCircleImage.color = _maxColorCircle;
        });
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(moveStopOffset); // wait a bit from the attack telegraph animation and stop moving

        enemyMovement.AttackStarted(); // stop moving


        yield return new WaitForSeconds(timeToAttackInSeconds - moveStopOffset);
        // Core attack logic
        Vector3 attackPoint = transform.forward * 1.5f;
        int hits = Physics.OverlapSphereNonAlloc(transform.position + attackPoint, 1f, _colliders, ~_excludedLayer); // TODO : layermask for damageable objects or enemies?
        for (int i = 0; i < hits; i++)
        {
            // Currently assuming the collider is on the same object as the HealthComponent
            if (_colliders[i].TryGetComponent(out HealthComponent healthComponent) && !_colliders[i].isTrigger)
            {
                healthComponent.BlockableDamage(new AttackData(DealDamage()));
            }
        }
        DebugExtension.DebugWireSphere(transform.position + attackPoint, Color.red, 1f, 1f);

        yield return new WaitForSeconds(0.2f); // some extra space padding before we allow movement again so the animation doesnt feel weird 

        enemyMovement.AttackFinished(); // start moving again
        isCooldown = false;
    }

    private void OnDestroy()
    {
        circleTween.Kill();
    }

}
