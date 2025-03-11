using UnityEngine;


[CreateAssetMenu(fileName = "SwordBehavior", menuName = "Inventory/WeaponBehaviors/Sword", order = 1)]
public class SwordBehavior : WeaponBehavior
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 1.5f;
    
    private PlayerCombat playerCombat;
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
        // TODO : add some kind of Start/Awake method to collect ref the first frame its worn/used
        Vector3 rotatingOffset = user.GetComponent<PlayerCombat>().GetRotatingRootForward() * 1.5f;
        int hits = Physics.OverlapSphereNonAlloc(user.position + rotatingOffset, 1f, colliders); // TODO : layermask for damageable objects or enemies?
        for (int i = 0; i < hits; i++)
        {
            // Currently assuming the collider is on the same object as the HealthComponent
            if (colliders[i].TryGetComponent(out HealthComponent healthComponent) && !colliders[i].isTrigger)
            {
                healthComponent.Damage(10);
            }
        }
        DebugExtension.DebugWireSphere(user.position + rotatingOffset, Color.red, 1f, 1f);
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
}
