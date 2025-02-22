using UnityEngine;


[CreateAssetMenu(fileName = "SwordBehavior", menuName = "Inventory/WeaponBehaviors/Sword", order = 1)]
public class SwordBehavior : WeaponBehavior
{
    public int damage = 10;
    public float attackRange = 1.5f;

    public override void Use(Transform user)
    {
        Debug.Log("Swinging sword!");
        // Example attack logic (sphere cast to detect enemies)
        RaycastHit[] hits = Physics.SphereCastAll(user.position, attackRange, user.forward, 0f);
        foreach (var hit in hits)
        {
            Debug.Log("Hit " + hit.transform.name);
            // Apply damage to enemy here
        }
    }
}
