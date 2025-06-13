using UnityEngine;


[CreateAssetMenu(fileName = "SwordBehavior", menuName = "Inventory/WeaponBehaviors/Sword", order = 1)]
public class SwordBehavior : WeaponBehavior
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 1.5f;
    private PlayerCombat playerCombat; // will this only be used by player or by enemies as well?

    public override void Initialize(Transform user)
    {
       // Debug.Log("Setting player in SwordBehavior");
        playerCombat = user.GetComponent<PlayerCombat>();
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

    public override void ClearData(Transform user)
    {
        throw new System.NotImplementedException();
    }

    public override void UseStart(Transform user)
    {
        Vector3 rotatingOffset = playerCombat.GetRotatingRootForward() * 1.5f;
        int hits = Physics.OverlapSphereNonAlloc(user.position + rotatingOffset, 2f, colliders); // TODO : layermask for damageable objects or enemies?
        for (int i = 0; i < hits; i++)
        {
            // Currently assuming the collider is on the same object as the HealthComponent
            if (colliders[i].TryGetComponent(out HealthComponent healthComponent) && !colliders[i].CompareTag("Player") && !colliders[i].isTrigger)
            {
                Debug.Log("Hit");
                healthComponent.SimpleDamage(10);
            }
        }
        DebugExtension.DebugWireSphere(user.position + rotatingOffset, Color.red, 2f, 1f);
    }

    public override void UseStop(Transform user)
    {

    }

    public override void UseStrongStart(Transform user)
    {
        Vector3 rotatingOffset = playerCombat.GetRotatingRootForward() * 1.5f;
        int hits = Physics.OverlapSphereNonAlloc(user.position + rotatingOffset, 2f, colliders); // TODO : layermask for damageable objects or enemies?
        for (int i = 0; i < hits; i++)
        {
            // Currently assuming the collider is on the same object as the HealthComponent
            if (colliders[i].TryGetComponent(out HealthComponent healthComponent) && !colliders[i].CompareTag("Player") && !colliders[i].isTrigger)
            {
                healthComponent.SimpleDamage(20);
            }
        }
        DebugExtension.DebugWireSphere(user.position + rotatingOffset, Color.magenta, 2f, 1f);
    }

    public override void UseStrongStop(Transform user)
    {

    }

    public override void AltUseStart(Transform user)
    {
        playerCombat.StartBlocking();
    }

    public override void AltUseStop(Transform user)
    {
        playerCombat.StopBlocking();
    }

    public override void UseSpecialAttack(Transform user)
    {
        float radius = attackRange * 2.0f;
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

    public override bool IsDistance()
    {
        return false;
    }
}
