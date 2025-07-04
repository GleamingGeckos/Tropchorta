using UnityEngine;

public abstract class WeaponBehavior : ScriptableObject
{
    public abstract void Initialize(Transform user);
    // the most basic weapon behavior this can be either a simple attack OR treated as beginning of a hold-to-use attack like a bow or something
    public abstract void UseStart(Transform user, Charm charm);

    public abstract void UseSpecialAttack(Transform user, Charm charm);

    // this is for when the player releases the button or stops using the weapon
    public abstract void UseStop(Transform user);
    public abstract void UseStrongStart(Transform user, Charm charm);

    // this is for when the player releases the button or stops using the weapon
    public abstract void UseStrongStop(Transform user);

    // alt use. For something like right-click block with a sword
    public abstract void AltUseStart(Transform user, Charm charm);

    public abstract void AltUseStop(Transform user);

    public abstract void ClearData(Transform user); // for use when player switches weapons so that behaviours are fresh

    public abstract bool IsDistance();

    public abstract void PlayPerfectBlockSound();
    public abstract void PlayNormalBlockSound();
}
