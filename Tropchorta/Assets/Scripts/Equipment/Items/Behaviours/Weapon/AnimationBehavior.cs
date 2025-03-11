using UnityEngine;
[CreateAssetMenu(fileName = "NewBehaviour", menuName = "Inventory/WeaponBehaviors/AnimationBehavior", order = 4)]
public class AnimationBehavior : WeaponBehavior
{
    public string animationTrigger = "Attack";

    public override void Use(Transform user)
    {
        Animator animator = user.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger(animationTrigger);
            Debug.Log("Triggered animation: " + animationTrigger);
        }
    }

    public override void ClearData(Transform user)
    {
        throw new System.NotImplementedException();
    }

    public override void StopUse(Transform user)
    {
        throw new System.NotImplementedException();
    }

    public override void AltUse(Transform user)
    {
        throw new System.NotImplementedException();
    }
}
