using UnityEngine;
[CreateAssetMenu(fileName = "NewBehaviour", menuName = "Inventory/WeaponBehaviors/AnimationBehavior", order = 4)]
public class AnimationBehavior : WeaponBehavior
{
    public string animationTrigger = "Attack";

    public override void UseStart(Transform user)
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

    public override void AltUseStart(Transform user)
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

    public override void UseSpecialAttack(Transform user)
    {
        throw new System.NotImplementedException();
    }
}
