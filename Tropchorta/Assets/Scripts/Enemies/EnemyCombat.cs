using DG.Tweening;
using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyCombat : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] protected CharmType _attackCharm;
    
    public Animator EnemyAnimator;

    public CharmType AttackCharm => _attackCharm; 
    [SerializeField] protected CharmType _weakToCharm;

    public CharmType WeakToCharm => _weakToCharm;
    [SerializeField] protected int _maxDamage;//inclusive
    [SerializeField] protected int _minDamage;//inclusive


    protected Collider[] _colliders = new Collider[16];
    [SerializeField] protected LayerMask _excludedLayer;
    protected Coroutine attackCoroutine;

    [SerializeField] public bool isCooldown = false;
    [SerializeField, Tooltip("this should be longer than the attack animation itself")] protected float _cooldownInterval = 3.0f;

    [Header("Par")]
    [SerializeField] private Vector2 _perfectBlockWindow;
    protected Tween circleTween;
    protected float _perfectBlockInSeconds;
    public bool isPerfectBlockWindow;

    [Header("Normal attack signal")]
    [SerializeField] protected GameObject _circles;
    [SerializeField] protected RectTransform _attackCircleTransform;
    [SerializeField] protected Image _attackCircleImage;
    [SerializeField] protected float _minCircle;
    [SerializeField] protected float _maxCircle;
    [SerializeField] protected Color _minColorCircle;
    [SerializeField] protected Color _maxColorCircle;

    [Header("Strong attack signal")]
    [SerializeField] protected RectTransform _attackStrongTransform;
    [SerializeField] protected Image _attackStrongImage;
    [SerializeField] protected Vector3 _minStrong;
    [SerializeField] protected Vector3 _maxStrong;
    protected Tween StrongTween;

    [SerializeField] protected Animator animator;

    [SerializeField, Tooltip("This is a unit from Animation timeline Unit where you input [seconds:...something] and that someting is not ms because it can only reach up to 60. Meaning if an event happens at 1:30 this is treated as 1.5s because the 30 means half a second.")]
    Vector2Int timeFromStartToAttackInUnityTimeline = new Vector2Int(0, 50);
    [SerializeField, Tooltip("Just like variable above, this is in animation timeline units. Specifies when the enemy stops moving from the animation start in timeline units. SHOULD BE LESS THAN timeFromStartToAttackInUnityTimeline.")]
    Vector2Int offsetFromAnimStartToMovementStop = new Vector2Int(0, 50);
    protected float moveStopOffset = 0.45f;
    protected float timeToAttackInSeconds = 0f;
    protected EnemyMovement enemyMovement;

    public GameObject arrowPrefab;

    [SerializeField] private EventReference _attackSound;

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
        int damage = _minDamage;
        //Debug.Log($"Attack dealt {damage} damage.");
        return damage;
    }

    public void Attack()
    {
        if (isCooldown) return;
        attackCoroutine = StartCoroutine(NormalAttackRoutine());
        RuntimeManager.PlayOneShot(_attackSound, transform.position);
        StartCoroutine(PerfectBlockWindow());
        EnemyAnimator.SetTrigger("attackTrigger");

    }

    public void StrongAttack()
    {
        if (isCooldown) return;
        attackCoroutine = StartCoroutine(StrongAttackRoutine());
        StartCoroutine(PerfectBlockWindow());
        EnemyAnimator.SetTrigger("powerfulTrigger");

    }

    public virtual void DistanceAttack(Transform target)
    {
        if (isCooldown) return;

        //Check if on screen
        if (!IsModelVisible()) return;

        attackCoroutine = StartCoroutine(DistanceAttackRoutine(target));
        StartCoroutine(PerfectBlockWindow());
        EnemyAnimator.SetTrigger("attackTrigger");

    }
    bool IsModelVisible()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        return renderer != null && renderer.isVisible;
    }
    public void WasBlocked()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    protected IEnumerator PerfectBlockWindow()
    {
        yield return new WaitForSeconds(_cooldownInterval - _perfectBlockInSeconds);
        isPerfectBlockWindow = true;
        yield return new WaitForSeconds(_perfectBlockInSeconds);
        isPerfectBlockWindow = false;
    }

    private IEnumerator NormalAttackRoutine()
    {
        isCooldown = true; // Set flag to prevent multiple coroutines
        float time = _cooldownInterval - timeToAttackInSeconds;
        yield return new WaitForSeconds(time);
        _circles.SetActive(true);

        enemyMovement.AttackStarted(); // stop moving
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

        yield return new WaitForSeconds(moveStopOffset); // wait a bit from the attack telegraph animation and stop moving

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(timeToAttackInSeconds - moveStopOffset);
        // Core attack logic
        Vector3 attackPoint = transform.forward * 1.5f;
        int hits = Physics.OverlapSphereNonAlloc(transform.position + attackPoint, 1f, _colliders, ~_excludedLayer); // TODO : layermask for damageable objects or enemies?
        for (int i = 0; i < hits; i++)
        {
            // Currently assuming the collider is on the same object as the HealthComponent
            if (_colliders[i].TryGetComponent(out HealthComponent healthComponent) &&
                _colliders[i].TryGetComponent(out PlayerCombat playerCombatComponent) &&
                _colliders[i].TryGetComponent(out PlayerMovement playerMovementComponent) &&
                !_colliders[i].isTrigger &&
    _colliders[i] is CapsuleCollider)
            {
                playerMovementComponent.RotatePlayerTowards(transform.position);
                if (enemyMovement.perfectParWasInitiated && playerCombatComponent.isBlocking)
                {
                    playerCombatComponent.PerfectBlocked();
                    enemyMovement.Stun();
                } else if (playerCombatComponent.isBlocking)
                {
                    playerCombatComponent.NormalBlocked();
                } 
                else if (!playerCombatComponent.isBlocking)
                {
                    healthComponent.SimpleDamage(new AttackData(gameObject, DealDamage(), _attackCharm));
                }
                    
                enemyMovement.perfectParWasInitiated = false;
            }
        }
        DebugExtension.DebugWireSphere(transform.position + attackPoint, new Color(0.5f, 0.2f, 0.0f), 1f, 1f);

        yield return new WaitForSeconds(0.2f); // some extra space padding before we allow movement again so the animation doesnt feel weird 

        enemyMovement.AttackFinished(); // start moving again
        isCooldown = false;
    }

    protected virtual IEnumerator DistanceAttackRoutine(Transform target)
    {
        isCooldown = true; // Set flag to prevent multiple coroutines
        float time = _cooldownInterval - timeToAttackInSeconds;
        yield return new WaitForSeconds(time);
        _circles.SetActive(true);
        enemyMovement.AttackStarted();

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
        yield return new WaitForSeconds(moveStopOffset); // wait a bit from the attack telegraph animation and stop moving

        animator.SetTrigger("Attack");
 // stop moving

        yield return new WaitForSeconds(timeToAttackInSeconds - moveStopOffset);
        // Core attack logic
        Vector3 attackPoint = transform.forward * 1.5f;
        int hits = Physics.OverlapSphereNonAlloc(transform.position + attackPoint, 1f, _colliders, ~_excludedLayer); // TODO : layermask for damageable objects or enemies?
        GameObject arrow = Instantiate(arrowPrefab, transform.position + transform.forward + transform.up, transform.rotation);
        arrow.GetComponent<Projectile>().Initialize(target, _attackCharm, DealDamage(), transform.gameObject);

        yield return new WaitForSeconds(0.2f); // some extra space padding before we allow movement again so the animation doesnt feel weird 

        enemyMovement.AttackFinished(); // start moving again
        isCooldown = false;
    }

    private IEnumerator StrongAttackRoutine()
    {
        isCooldown = true; // Set flag to prevent multiple coroutines
        float time = _cooldownInterval - timeToAttackInSeconds;
        yield return new WaitForSeconds(time);
        enemyMovement.AttackStarted();

        // start the circle animation
        _attackStrongTransform.pivot = new Vector2(_attackStrongTransform.pivot.x, 0f); // trzyma d�
        _attackStrongTransform.localScale = new Vector3(_minStrong.x, _minStrong.y, 1f);
        _attackStrongImage.enabled = true;

        StrongTween = DOTween.Sequence()
            .Append(_attackStrongTransform.DOScaleY(_maxStrong.y, timeToAttackInSeconds / 5.2f)
                .From(_minStrong.y)
                .SetEase(Ease.InOutBack))
            .SetLoops(5, LoopType.Yoyo)
            .OnComplete(() =>
            {
                _attackStrongImage.enabled = false;
                _attackStrongTransform.localScale = new Vector3(_minStrong.x, _minStrong.y, 1f);
            });
        _attackStrongImage.DOFade(1f, timeToAttackInSeconds).From(0.2f);


        yield return new WaitForSeconds(moveStopOffset); // wait a bit from the attack telegraph animation and stop moving

        animator.SetTrigger("Attack"); // stop moving

        yield return new WaitForSeconds(timeToAttackInSeconds - moveStopOffset);
        // Core attack logic
        Vector3 attackPoint = transform.forward * 2.5f;
        int hits = Physics.OverlapSphereNonAlloc(transform.position + attackPoint, 3f, _colliders, ~_excludedLayer); // TODO : layermask for damageable objects or enemies?
        for (int i = 0; i < hits; i++)
        {
            // Currently assuming the collider is on the same object as the HealthComponent
            if (_colliders[i].TryGetComponent(out PlayerHealthComponent healthComponent) &&
                _colliders[i].TryGetComponent(out PlayerMovement playerMovementComponent)
                && !_colliders[i].isTrigger &&
                _colliders[i] is CapsuleCollider)
            {
                
                playerMovementComponent.RotatePlayerTowards(transform.position);
                healthComponent.UnblockableDamage(new AttackData(gameObject, DealDamage(), _attackCharm));
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
        StrongTween.Kill();
    }

    public void PushBack(Vector3 direction, float distance, float duration, float radius, LayerMask collisionMask)
    {
        StartCoroutine(PushBackSmooth(direction, distance, duration, radius, collisionMask));
    }

    public IEnumerator PushBackSmooth(Vector3 direction, float distance, float duration, float radius, LayerMask collisionMask)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent == null || !agent.isOnNavMesh) yield break;

        agent.isStopped = true;
        agent.updatePosition = false;

        Transform target = agent.transform;
        Vector3 start = target.position;
        Vector3 dir = direction.normalized;
        Vector3 pos = target.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            Vector3 nextPos = Vector3.Lerp(start, start + dir * distance, t);

            Debug.DrawRay(start, dir * Vector3.Distance(start, nextPos), Color.red, 10f);
            // Obstacle check using SphereCast
            if (Physics.SphereCast(start, radius, dir, out RaycastHit hit, Vector3.Distance(start, nextPos), collisionMask))
            {
                yield break;
            }

            pos = nextPos;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ustaw ko�cow� pozycj� tylko je�li nie by�o kolizji
        if (!Physics.SphereCast(start, radius, dir, out _, distance, collisionMask))
        {
            pos = start + dir * distance;
        }

        agent.Warp(pos);
        agent.updatePosition = true;
        agent.isStopped = false;
    }

}
