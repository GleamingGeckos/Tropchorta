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
}
