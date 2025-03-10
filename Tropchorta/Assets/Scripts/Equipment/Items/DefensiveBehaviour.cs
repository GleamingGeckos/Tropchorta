using UnityEngine;

public abstract class DefensiveBehaviour : ScriptableObject
{
    public abstract void Use(Transform user);

    public abstract void ClearData(Transform user); // for use when player switches items so that behaviours are fresh
}
