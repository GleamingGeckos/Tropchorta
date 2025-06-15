using UnityEngine;
[CreateAssetMenu(fileName = "NewBehaviour", menuName = "Inventory/WeaponBehaviors/AnimationBehavior", order = 4)]
public class AnimationBehavior : WeaponBehavior
{
    public string animationTrigger = "Attack";

    public override void UseStart(Transform user, Charm charm)
    {
        Animator animator = user.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger(animationTrigger);
            Debug.Log("Triggered animation: " + animationTrigger);
        }
    }

    public override void UseStop(Transform user)
    {

    }
    public override void UseStrongStart(Transform user, Charm charm)
    {
        Animator animator = user.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger(animationTrigger);
            Debug.Log("Triggered animation: " + animationTrigger);
        }
    }

    public override void UseStrongStop(Transform user)
    {

    }

    public override void AltUseStart(Transform user, Charm charm)
    {

    }

    public override void AltUseStop(Transform user)
    {

    }

    public override void ClearData(Transform user)
    {
        
    }

    public override void Initialize(Transform user)
    {
        
    }

    public override void UseSpecialAttack(Transform user, Charm charm)
    {
        throw new System.NotImplementedException();
    }

    public override bool IsDistance()
    {
        return true;
    }
}
