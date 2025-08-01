using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Inventory/Weapon", order = 3)]
public class Weapon : Item
{
    public WeaponBehavior[] weaponBehaviors;
    public Charm charm;

    public void Initialize(Transform user)
    {
        if(charm != null)
            charm.EnableCharmEffect(user);
        foreach (var behavior in weaponBehaviors)
        {
            behavior?.Initialize(user);
        }
    }

    public void UseStart(Transform user)
    {
        foreach (var behavior in weaponBehaviors)
        {
            behavior?.UseStart(user, charm);
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
            behavior?.UseStrongStart(user, charm);
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
            behavior?.UseSpecialAttack(user, charm);
        }
    }

    public void AltUseStart(Transform user)
    {
        foreach (var behavior in weaponBehaviors)
        {
            behavior?.AltUseStart(user, charm);
        }
    }

    public void AltUseEnd(Transform user)
    {
        foreach (var behavior in weaponBehaviors)
        {
            behavior?.AltUseStop(user);
        }
    }

    public void PerfectBlocked()
    {
        foreach (var behavior in weaponBehaviors)
        {
            behavior?.PlayPerfectBlockSound();
        }
    }

    public void NormalBlocked()
    {
        foreach (var behavior in weaponBehaviors)
        {
            behavior?.PlayNormalBlockSound();
        }
    }

    public void ClearData(Transform user)
    {
        if (charm != null)
        {
            charm.DisableCharmEffect();
        }
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
