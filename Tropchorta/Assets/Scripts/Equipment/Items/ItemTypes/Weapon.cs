using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Inventory/Weapon", order = 3)]
public class Weapon : Item
{
    public WeaponBehavior[] weaponBehaviors;

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
}
