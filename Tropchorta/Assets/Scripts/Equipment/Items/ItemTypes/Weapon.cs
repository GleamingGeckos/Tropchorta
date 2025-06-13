using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Inventory/Weapon", order = 3)]
public class Weapon : Item
{
    public WeaponBehavior[] weaponBehaviors;

    public void Initialize(Transform user)
    {
        foreach (var behavior in weaponBehaviors)
        {
            behavior?.Initialize(user);
        }
    }

    public void UseStart(Transform user)
    {
        foreach (var behavior in weaponBehaviors)
        {
            behavior?.UseStart(user);
        }
    }

    public void UseEnd(Transform user)
    {
        foreach (var behavior in weaponBehaviors)
        {
            behavior?.UseStop(user);
        }
    }
    public void UseStrongStart(Transform user)
    {
        foreach (var behavior in weaponBehaviors)
        {
            behavior?.UseStrongStart(user);
        }
    }

    public void UseStrongEnd(Transform user)
    {
        foreach (var behavior in weaponBehaviors)
        {
            behavior?.UseStrongStop(user);
        }
    }

    public void UseSpecialAttack(Transform user)
    {
        foreach (var behavior in weaponBehaviors)
        {
            behavior?.UseSpecialAttack(user);
        }
    }

    public void AltUseStart(Transform user)
    {
        foreach (var behavior in weaponBehaviors)
        {
            behavior?.AltUseStart(user);
        }
    }

    public void AltUseEnd(Transform user)
    {
        foreach (var behavior in weaponBehaviors)
        {
            behavior?.AltUseStop(user);
        }
    }

    public void ClearData(Transform user)
    {
        foreach (var behavior in weaponBehaviors)
        {
            behavior?.ClearData(user);
        }
    }

    public bool IsDistance()
    {
        bool isDistance = false;
        foreach (var behavior in weaponBehaviors)
        {
            if (behavior != null && !isDistance)
            {
                isDistance = behavior.IsDistance();
                if(isDistance)
                    return true;
            }
        }
        return isDistance;
    }
}
