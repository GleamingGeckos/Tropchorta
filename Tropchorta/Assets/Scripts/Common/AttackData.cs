using UnityEngine;

public struct AttackData
{
    public float damage;
    public float blockTime;
    public CharmType charmType;

    public AttackData(float damage, float blockTime = 0.2f, CharmType charmType = CharmType.None)
    {
        this.damage = damage;
        this.blockTime = blockTime;
        this.charmType = charmType;
    }

    public AttackData(AttackData attackData)
    {
        this.damage = attackData.damage;
        this.blockTime = attackData.blockTime;
        this.charmType = attackData.charmType;
    }
}
