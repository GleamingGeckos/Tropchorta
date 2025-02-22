using UnityEngine;

public abstract class WeaponBehavior : ScriptableObject
{
    public abstract void Use(Transform user);
}
