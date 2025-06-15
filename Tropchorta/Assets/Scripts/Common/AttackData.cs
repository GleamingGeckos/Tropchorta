using UnityEngine;

public struct AttackData
{
    public float damage;
    public CharmType charmType;

    public AttackData(float damage, CharmType charmType = CharmType.None)
    {
        this.damage = damage;
        this.charmType = charmType;
    }

    public AttackData(AttackData attackData)
    {
        this.damage = attackData.damage;
        this.charmType = attackData.charmType;
    }
}
