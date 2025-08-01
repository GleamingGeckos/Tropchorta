using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private PlayerCombat playerCombat;
    public Animator weaponAnimator;
    [SerializeField] private bool isHolding;

    void Attack()
    {
        playerCombat.Attack();
    }

    void EndAttack()
    {
        playerCombat.EndAttack();
        
        if (playerCombat.doNextAttack)
        {
            playerCombat.doNextAttack = false;
            playerCombat.StartAttackAnim();
        }
    }
}
