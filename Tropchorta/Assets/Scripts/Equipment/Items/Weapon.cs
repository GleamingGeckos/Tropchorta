using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Inventory/Weapon", order = 3)]
public class Weapon : Item
{
    public WeaponBehavior[] weaponBehaviors;

    public void Use(Transform user)
    {
        foreach (var behavior in weaponBehaviors)
        {
            behavior?.Use(user);
        }
    }
}
