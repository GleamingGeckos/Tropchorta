using UnityEngine;


[CreateAssetMenu(fileName = "SwordBehavior", menuName = "Inventory/WeaponBehaviors/Sword", order = 1)]
public class SwordBehavior : WeaponBehavior
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 1.5f;
    public int Damage
    {
        get => damage;
        set
        {
            if (value >= 0)
                damage = value;
            else
                Debug.LogWarning("Damage cannot be negative.");
        }
    }
    public float AttackRange
    {
        get => attackRange;
        set
        {
            if (value >= 0)
                attackRange = value;
            else
                Debug.LogWarning("Attack Range cannot be negative.");
        }
    }
    public override void Use(Transform user)
    {
        // Debug.Log("Swinging sword!");
        // Example attack logic (sphere cast to detect enemies)
        RaycastHit[] hits = Physics.SphereCastAll(user.position, attackRange, user.forward, 0f);
        foreach (var hit in hits)
        {
            //Debug.Log("Hit " + hit.transform.name);
            DebugExtension.DebugWireSphere(hit.transform.position, Color.yellow, 1f, 1f);
            // Apply damage to enemy here
        }
    }
}
