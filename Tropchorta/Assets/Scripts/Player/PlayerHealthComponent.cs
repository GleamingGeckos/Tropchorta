using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthComponent : HealthComponent
{
    public override void BlockableDamage(AttackData ad)
    {
        if (ad.damage < 0)
        {
            Debug.LogError("Damage cannot be negative, if you meant to heal use Heal method");
            return;
        }
        onAttacked.Invoke(ad);
    }
}
