using UnityEngine;

[CreateAssetMenu(fileName = "Charm", menuName = "Inventory/Charm", order = 1)]


public class Charm : Item
{
    private CharmType charmType;

    public CharmType GetCharmType()
    {
        return charmType;
    }

    public AttackData CharmEffectOnWeapon(AttackData attackData)
    {
        AttackData newAttack = new AttackData(attackData);
        if (charmType == attackData.charmType)
            newAttack.damage *= 2.0f;
        return newAttack;
    }

    public AttackData CharmEffectOnArmor(AttackData attackData)
    {
        AttackData newAttack = new AttackData(attackData);
        if (charmType == attackData.charmType)
            newAttack.damage *= 0.5f;
        return newAttack;
    }
}
