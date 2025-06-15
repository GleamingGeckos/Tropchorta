using UnityEngine;

public class DefensiveItem : Item
{
    public DefensiveBehaviour[] defensiveBehaviours;
    public Charm charm;

    public Charm GetDefensiveCharm()
    {
        return charm;
    }

    public void Use(Transform user)
    {
        foreach (var behavior in defensiveBehaviours)
        {
            behavior?.Use(user);
        }
    }

    public void ClearData(Transform user)
    {
        foreach (var behavior in defensiveBehaviours)
        {
            behavior?.ClearData(user);
        }
    }
}
