using UnityEngine;

[CreateAssetMenu(fileName = "Charm", menuName = "Inventory/Charm", order = 1)]


public class Charm : Item
{
    [SerializeField] private CharmType charmType;

    [SerializeField, Range(0f, 1f)]
    public float armorReductionProcent = 0.5f;

    [SerializeField, Range(1f, 5f)]
    public float weaponAmplificationMultiplier = 2.0f;

    public CharmType GetCharmType()
    {
        return charmType;
    }

    public AttackData CharmEffectOnWeapon(AttackData attackData)
    {
        AttackData newAttack = new AttackData(attackData);
     
        if (charmType == attackData.charmType && charmType != CharmType.None)
            newAttack.damage *= weaponAmplificationMultiplier;

        return newAttack;
    }

    public AttackData CharmEffectOnArmor(AttackData attackData)
    {
        AttackData newAttack = new AttackData(attackData);
        //Debug.Log(charmType + " " + newAttack.charmType);
        //Debug.Log("Old" + newAttack.damage);
        if (charmType == attackData.charmType && charmType != CharmType.None)
            newAttack.damage *= (1.0f - armorReductionProcent);
        //Debug.Log("New" + newAttack.damage);
        //Debug.Log("-----------------------------------");
        return newAttack;
    }
}
