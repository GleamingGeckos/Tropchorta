using UnityEngine;

public abstract class WeaponBehavior : ScriptableObject
{
    public abstract void Use(Transform user);

    public abstract void ClearData(Transform user); // for use when player switches weapons so that behaviours are fresh
}
