using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthComponent : HealthComponent
{
    [Header("UI References")]
    [SerializeField] HealthBar healthBar;

    void Start()
    {
        var uiObjects = FindObjectsByType<PlayerUIController>(FindObjectsSortMode.None);
        if (uiObjects.Length > 0)
        {
            healthBar = uiObjects[0].healthBar;
        }

        onHeal.AddListener(UpdateHealth);
        onDamageTaken.AddListener(UpdateHealth);
    }

    public override void SimpleDamage(AttackData ad)
    {
        onAttacked.Invoke(ad);
    }
    public void BaseSimpleDamage(AttackData ad)
    {
        base.SimpleDamage(ad);
    }

    public void UpdateHealth(float value, float currentValue)
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth / MaxHealth);
        }
        
    }

}
