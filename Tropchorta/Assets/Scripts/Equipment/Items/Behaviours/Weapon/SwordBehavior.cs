using FMODUnity;
using UnityEngine;


[CreateAssetMenu(fileName = "SwordBehavior", menuName = "Inventory/WeaponBehaviors/Sword", order = 1)]
public class SwordBehavior : WeaponBehavior
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackRadius = 1.5f;
    [SerializeField] private float specialAttackRadius = 2.0f;
    [SerializeField] private float attackDistanceFromPlayer = 2.5f;
    [SerializeField] private float specialAttackDistanceFromPlayer = .5f;
    [SerializeField] private EventReference attackSound;
    [SerializeField] private EventReference perfectBlockSound;
    [SerializeField] private EventReference normalBlockSound;
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
        get => specialAttackRadius;
        set
        {
            if (value >= 0)
                specialAttackRadius = value;
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
        Vector3 rotatingOffset = playerCombat.GetRotatingRootForward() * attackRadius * attackDistanceFromPlayer;
        int hits = Physics.OverlapSphereNonAlloc(user.position + rotatingOffset, attackRadius, colliders); // TODO : layermask for damageable objects or enemies?
        for (int i = 0; i < hits; i++)
        {
            // Currently assuming the collider is on the same object as the HealthComponent
            if (colliders[i].TryGetComponent(out HealthComponent healthComponent) && colliders[i].TryGetComponent(out EnemyCombat enemyCombat) && !colliders[i].CompareTag("Player") && !colliders[i].isTrigger)
            {
                AttackData attack;
                if (charm)
                    attack = new AttackData(user.gameObject, damage, charm.GetCharmType());
                else
                    attack = new AttackData(user.gameObject, damage, CharmType.None);
                healthComponent.SimpleDamage(Charm.CharmEffectOnWeapon(attack, enemyCombat.WeakToCharm, Charm.weaponAmplificationMultiplier));
            }
        }
        RuntimeManager.PlayOneShot(attackSound, user.position);
        DebugExtension.DebugWireSphere(user.position + rotatingOffset, Color.red, attackRadius, 1f);
    }

    public override void UseStop(Transform user)
    {

    }

    public override void UseStrongStart(Transform user, Charm charm)
    {
        Vector3 rotatingOffset = playerCombat.GetRotatingRootForward() * attackRadius * attackDistanceFromPlayer;
        int hits = Physics.OverlapSphereNonAlloc(user.position + rotatingOffset, attackRadius, colliders); // TODO : layermask for damageable objects or enemies?
        for (int i = 0; i < hits; i++)
        {
            // Currently assuming the collider is on the same object as the HealthComponent
            if (colliders[i].TryGetComponent(out HealthComponent healthComponent) && colliders[i].TryGetComponent(out EnemyCombat enemyCombat) && !colliders[i].CompareTag("Player") && !colliders[i].isTrigger)
            {
                AttackData attack;
                if (charm)
                    attack = new AttackData(user.gameObject, damage * 2, charm.GetCharmType());
                else
                    attack = new AttackData(user.gameObject, damage * 2, CharmType.None);
                healthComponent.SimpleDamage(Charm.CharmEffectOnWeapon(attack, enemyCombat.WeakToCharm, Charm.weaponAmplificationMultiplier));
            }
        }
        RuntimeManager.PlayOneShot(attackSound, user.position);
        DebugExtension.DebugWireSphere(user.position + rotatingOffset, Color.magenta, attackRadius, 1f);
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
        Vector3 rotatingOffset = playerCombat.GetRotatingRootForward() * specialAttackDistanceFromPlayer;
        float radius = specialAttackRadius;
        Collider[] hitColliders = Physics.OverlapSphere(user.position + rotatingOffset, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy") && hitCollider.TryGetComponent(out EnemyCombat enemyCombat) && hitCollider.TryGetComponent(out HealthComponent healthComponent) && !hitCollider.isTrigger)
            {
                AttackData attack;
                if (charm)
                    attack = new AttackData(user.gameObject, damage * 3, charm.GetCharmType());
                else
                    attack = new AttackData(user.gameObject, damage * 3, CharmType.None);
                healthComponent.SimpleDamage(Charm.CharmEffectOnWeapon(attack, enemyCombat.WeakToCharm, Charm.weaponAmplificationMultiplier));
            }
        }
        RuntimeManager.PlayOneShot(attackSound, user.position + rotatingOffset);
        DebugExtension.DebugWireSphere(user.position, Color.blue, radius, 1f);
    }

    public override bool IsDistance()
    {
        return false;
    }

    public override void PlayPerfectBlockSound()
    {
        RuntimeManager.PlayOneShot(perfectBlockSound);
    }

    public override void PlayNormalBlockSound()
    {
        RuntimeManager.PlayOneShot(normalBlockSound);
    }
}
