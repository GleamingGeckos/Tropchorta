using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BossCombat : EnemyCombat
{
    [Header("Speciul stuff")]
    [SerializeField] float jumpDamageRadius = 5;
    [SerializeField] ParticleSystem shockWave;

    public override void DistanceAttack(Transform target)
    {
        if (isCooldown) return;
        Debug.Log("Plucie attack");
        attackCoroutine = StartCoroutine(DistanceAttackRoutine(target));
        StartCoroutine(PerfectBlockWindow());
    }
    
    public void PunchAttack(Transform target)
    {
        if (isCooldown) return;
        Debug.Log("Punch attack");
        attackCoroutine = StartCoroutine(PunchAttackRoutine(target));
    }
    
    public void JumpAttack(Transform target)
    {
        if (isCooldown) return;
        Debug.Log("Jump attack");
        attackCoroutine = StartCoroutine(JumpAttackRoutine(target));
    }

    protected override IEnumerator DistanceAttackRoutine(Transform target)
    {
        isCooldown = true; // Set flag to prevent multiple coroutines
        float time = _cooldownInterval - timeToAttackInSeconds;
        yield return new WaitForSeconds(time);
        _circles.SetActive(true);

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
        GameObject arrow = Instantiate(arrowPrefab, transform.position + transform.forward + transform.up, transform.rotation);
        arrow.GetComponent<Projectile>().Initialize(target, _attackCharm, DealDamage(), transform.gameObject);

        yield return new WaitForSeconds(0.2f); // some extra space padding before we allow movement again so the animation doesnt feel weird 

        enemyMovement.AttackFinished(); // start moving again
        isCooldown = false;
        enemyMovement.StartChasing(target);
        enemyMovement.agent.updateRotation = true;
    }

    protected IEnumerator PunchAttackRoutine(Transform target)
    {
        isCooldown = true; // Set flag to prevent multiple coroutines
        float time = _cooldownInterval - timeToAttackInSeconds;
        yield return new WaitForSeconds(time);
        _circles.SetActive(true);

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
        float attackRadius = 2.0f;
        int hits = Physics.OverlapSphereNonAlloc(transform.position + attackPoint, attackRadius, _colliders, ~_excludedLayer); // TODO : layermask for damageable objects or enemies?
        for (int i = 0; i < hits; i++)
        {
            // Currently assuming the collider is on the same object as the HealthComponent
            if (_colliders[i].TryGetComponent(out HealthComponent healthComponent) &&
                _colliders[i].TryGetComponent(out PlayerCombat playerCombatComponent) &&
                _colliders[i].TryGetComponent(out PlayerMovement playerMovementComponent) &&
                !_colliders[i].isTrigger)
            {
                playerMovementComponent.RotatePlayerTowards(transform.position);
                if (enemyMovement.perfectParWasInitiated && playerCombatComponent.isBlocking)
                {
                    enemyMovement.Stun();
                    enemyMovement.perfectParWasInitiated = false;
                }
                else if (!playerCombatComponent.isBlocking)
                    healthComponent.SimpleDamage(new AttackData(gameObject, DealDamage(), _attackCharm));
            }
        }
        DebugExtension.DebugWireSphere(transform.position + attackPoint, Color.cyan, attackRadius, 7.0f);

        yield return new WaitForSeconds(0.2f); // some extra space padding before we allow movement again so the animation doesnt feel weird 

        enemyMovement.AttackFinished(); // start moving again
        isCooldown = false;
    }


    protected IEnumerator JumpAttackRoutine(Transform target)
    {
        isCooldown = true; // Set flag to prevent multiple coroutines
        float time = _cooldownInterval - timeToAttackInSeconds;
        yield return new WaitForSeconds(time);

        // start the circle animation
        _attackStrongTransform.pivot = new Vector2(_attackStrongTransform.pivot.x, 0f); // trzyma dó³
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

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(moveStopOffset); // wait a bit from the attack telegraph animation and stop moving

        enemyMovement.AttackStarted(); // stop moving

        yield return new WaitForSeconds(timeToAttackInSeconds - moveStopOffset);

        // Core attack logic
        int hits = Physics.OverlapSphereNonAlloc(transform.position, jumpDamageRadius, _colliders, ~_excludedLayer); // TODO : layermask for damageable objects or enemies?


        shockWave.Play();
        for (int i = 0; i < hits; i++)
        {
            // Currently assuming the collider is on the same object as the HealthComponent
            if (_colliders[i].TryGetComponent(out HealthComponent healthComponent) &&
                _colliders[i].TryGetComponent(out PlayerMovement playerMovementComponent)
                && !_colliders[i].isTrigger)
            {
                healthComponent.SimpleDamage(new AttackData(gameObject, 30, _attackCharm));

            }
        }
        DebugExtension.DebugWireSphere(transform.position, Color.red, jumpDamageRadius, 1f);

        yield return new WaitForSeconds(1.0f); // some extra space padding before we allow movement again so the animation doesnt feel weird 

        enemyMovement.AttackFinished(); // start moving again
        isCooldown = false;
    }
}
