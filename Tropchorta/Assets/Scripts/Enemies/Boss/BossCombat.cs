using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BossCombat : EnemyCombat
{
    public override void DistanceAttack(Transform target)
    {
        if (isCooldown) return;
        attackCoroutine = StartCoroutine(DistanceAttackRoutine(target));
        StartCoroutine(PerfectBlockWindow());
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
        GameObject arrow = Instantiate(arrowPrefab, transform.position + transform.forward + transform.up, transform.rotation, transform);
        arrow.GetComponent<Projectile>().Initialize(target, _attackCharm, DealDamage());

        yield return new WaitForSeconds(0.2f); // some extra space padding before we allow movement again so the animation doesnt feel weird 

        enemyMovement.AttackFinished(); // start moving again
        isCooldown = false;
    }
}
