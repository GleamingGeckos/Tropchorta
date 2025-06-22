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
        //throw new System.NotImplementedException();
    }

    public override void UseStart(Transform user, Charm charm)
    {
        Vector3 rotatingOffset = playerCombat.GetRotatingRootForward() * 1.5f;
        int hits = Physics.OverlapSphereNonAlloc(user.position + rotatingOffset, 2f, colliders); // TODO : layermask for damageable objects or enemies?
        for (int i = 0; i < hits; i++)
        {
            // Currently assuming the collider is on the same object as the HealthComponent
            if (colliders[i].TryGetComponent(out HealthComponent healthComponent) && colliders[i].TryGetComponent(out EnemyCombat enemyCombat) && !colliders[i].CompareTag("Player") && !colliders[i].isTrigger)
            {
                AttackData attack = new AttackData(user.gameObject, damage, charm.GetCharmType());
                healthComponent.SimpleDamage(Charm.CharmEffectOnWeapon(attack, enemyCombat.WeakToCharm, Charm.weaponAmplificationMultiplier));
            }
        }
        DebugExtension.DebugWireSphere(user.position + rotatingOffset, Color.red, 2f, 1f);
    }

    public override void UseStop(Transform user)
    {

    }

    public override void UseStrongStart(Transform user, Charm charm)
    {
        Vector3 rotatingOffset = playerCombat.GetRotatingRootForward() * 1.5f;
        int hits = Physics.OverlapSphereNonAlloc(user.position + rotatingOffset, 2f, colliders); // TODO : layermask for damageable objects or enemies?
        for (int i = 0; i < hits; i++)
        {
            // Currently assuming the collider is on the same object as the HealthComponent
            if (colliders[i].TryGetComponent(out HealthComponent healthComponent) && colliders[i].TryGetComponent(out EnemyCombat enemyCombat) && !colliders[i].CompareTag("Player") && !colliders[i].isTrigger)
            {
                AttackData attack = new AttackData(user.gameObject, damage*2, charm.GetCharmType());
                healthComponent.SimpleDamage(Charm.CharmEffectOnWeapon(attack, enemyCombat.WeakToCharm, Charm.weaponAmplificationMultiplier));
            }
        }
        DebugExtension.DebugWireSphere(user.position + rotatingOffset, Color.magenta, 2f, 1f);
    }

    public override void UseStrongStop(Transform user)
    {

    }

    public override void AltUseStart(Transform user, Charm charm)
    {
        playerCombat.StartBlocking();
    }

    public override void AltUseStop(Transform user)
    {
        playerCombat.StopBlocking();
    }

    public override void UseSpecialAttack(Transform user, Charm charm)
    {
        float radius = attackRange * 2.0f;
        Collider[] hitColliders = Physics.OverlapSphere(user.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy") && hitCollider.TryGetComponent(out EnemyCombat enemyCombat) && hitCollider.TryGetComponent(out HealthComponent healthComponent))
            {
                AttackData attack = new AttackData(user.gameObject, damage, charm.GetCharmType());
                healthComponent.SimpleDamage(Charm.CharmEffectOnWeapon(attack, enemyCombat.WeakToCharm, Charm.weaponAmplificationMultiplier));
            }
        }
        DebugExtension.DebugWireSphere(user.position, Color.blue, radius, 1f);

    }

    public override bool IsDistance()
    {
        return false;
    }
}
