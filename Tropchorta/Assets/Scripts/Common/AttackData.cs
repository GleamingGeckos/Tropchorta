using UnityEngine;

public struct AttackData
{
    public float damage;
    public float blockTime;

    public AttackData(float damage, float blockTime = 0.2f)
    {
        this.damage = damage;
        this.blockTime = blockTime;
    }
}
