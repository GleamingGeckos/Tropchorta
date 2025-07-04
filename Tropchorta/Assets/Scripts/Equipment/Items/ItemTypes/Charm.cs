using UnityEngine;

[CreateAssetMenu(fileName = "Charm", menuName = "Inventory/Charm", order = 1)]


public class Charm : Item
{
    [SerializeField] private CharmType charmType;

    [SerializeField, Range(0f, 1f)]
    public static float armorReductionProcent = 0.5f;

    [SerializeField, Range(1f, 5f)]
    public static float weaponAmplificationMultiplier = 2.0f;

    public GameObject weaponSwardCharm;

    private GameObject _effect;

    public CharmType GetCharmType()
    {
        return charmType;
    }

    public void EnableCharmEffect(Transform trans)
    {
        if (trans == null) return;
        Transform effectTarget = FindScripts.FindChildWithTag(trans, "Effect");
        if (effectTarget != null)
        {
            _effect = Instantiate(weaponSwardCharm, effectTarget);
        }
    }

    public void DisableCharmEffect()
    {
        Destroy(_effect);
        _effect = null;
    }

    public AttackData CharmEffectOnWeapon(AttackData attackData)
    {
        AttackData newAttack = new AttackData(attackData);
     
        if (charmType == attackData.charmType && charmType != CharmType.None)
            newAttack.damage = newAttack.damage + 1;

        return newAttack;
    }

    public static AttackData CharmEffectOnWeapon(AttackData attackData, CharmType charmType, float weaponAmplificationMultiplier)
    {
        AttackData newAttack = new AttackData(attackData);

        if (charmType == attackData.charmType && attackData.charmType != CharmType.None && charmType != CharmType.None)
            newAttack.damage = newAttack.damage + 1;

        return newAttack;
    }

    public AttackData CharmEffectOnArmor(AttackData attackData)
    {
        AttackData newAttack = new AttackData(attackData);
        //Debug.Log(charmType + " " + newAttack.charmType);
        //Debug.Log("Old" + newAttack.damage);
        if (charmType == attackData.charmType && charmType != CharmType.None && attackData.charmType != CharmType.None)
            newAttack.damage = newAttack.damage - 1; 
        //Debug.Log("New" + newAttack.damage);
        //Debug.Log("-----------------------------------");
        return newAttack;
    }
}
