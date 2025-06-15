using UnityEngine;

[CreateAssetMenu(fileName = "Charm", menuName = "Inventory/Charm", order = 1)]


public class Charm : Item
{
    [SerializeField] private CharmType charmType;

    public CharmType GetCharmType()
    {
        return charmType;
    }

    public AttackData CharmEffectOnWeapon(AttackData attackData)
    {
        AttackData newAttack = new AttackData(attackData);
     
        if (charmType == attackData.charmType && charmType != CharmType.None)
            newAttack.damage *= 2.0f;

        return newAttack;
    }

    public AttackData CharmEffectOnArmor(AttackData attackData)
    {
        AttackData newAttack = new AttackData(attackData);
       // Debug.Log(charmType + " " + newAttack.charmType);
        //Debug.Log("Old" + newAttack.damage);
        if (charmType == attackData.charmType && charmType != CharmType.None)
            newAttack.damage *= 0.5f;
        //Debug.Log("New" + newAttack.damage);
        return newAttack;
    }
}
