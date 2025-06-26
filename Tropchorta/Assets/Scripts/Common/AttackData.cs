using UnityEngine;

public struct AttackData
{
    public int damage;
    public CharmType charmType;
    public GameObject attacker;

    public AttackData(GameObject attacker, int damage, CharmType charmType = CharmType.None)
    {
        this.attacker = attacker;
        this.damage = damage;
        this.charmType = charmType;
    }

    public AttackData(AttackData attackData)
    {
        this.attacker = attackData.attacker;
        this.damage = attackData.damage;
        this.charmType = attackData.charmType;
    }
}
