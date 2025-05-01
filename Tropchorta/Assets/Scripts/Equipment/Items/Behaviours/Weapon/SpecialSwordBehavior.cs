using UnityEngine;


[CreateAssetMenu(fileName = "SwordBehavior", menuName = "Inventory/WeaponBehaviors/Special", order = 1)]
public class SpecialSwordBehavior : WeaponBehavior
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField, Range(0.0f, 1.0f)] private float blockPower = 0.2f;

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
    }

    public override void UseStart(Transform user)
    {
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

    public override void UseSpecialAttack(Transform user)
    {
        //AAAAAAAAAAAAAAAAAaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa TODO
    }
}
