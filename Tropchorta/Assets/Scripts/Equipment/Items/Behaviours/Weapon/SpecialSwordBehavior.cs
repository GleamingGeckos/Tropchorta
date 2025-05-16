using NUnit.Framework.Constraints;
using UnityEngine;


[CreateAssetMenu(fileName = "SwordBehavior", menuName = "Inventory/WeaponBehaviors/Special", order = 1)]
public class SpecialSwordBehavior : WeaponBehavior
{
    [SerializeField] private int damage = 20;
    [SerializeField] private float attackRadius = 1.5f;

    public override void Initialize(Transform user)
    {
        Debug.Log("Setting player in SwordBehavior");
    }

    Collider[] colliders = new Collider[16];
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

    public override void ClearData(Transform user)
    {
    }

    public override void UseStart(Transform user)
    {
    }

    public override void UseStop(Transform user)
    {

    }
    
    public override void UseStrongStart(Transform user)
    {
    }

    public override void UseStrongStop(Transform user)
    {

    }

    public override void AltUseStart(Transform user)
    {
    }

    public override void AltUseStop(Transform user)
    {
    }

    public override void UseSpecialAttack(Transform user)
    {
        float radius = attackRadius;
        Collider[] hitColliders = Physics.OverlapSphere(user.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy") && hitCollider.GetComponent<HealthComponent>())
            {
                hitCollider.GetComponent<HealthComponent>().SimpleDamage(damage);
            }
        }
        DebugExtension.DebugWireSphere(user.position, Color.blue, radius, 1f);
        
    }

}
