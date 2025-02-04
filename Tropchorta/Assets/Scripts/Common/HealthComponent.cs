using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] float maxHealth;
    float currentHealth;

    bool isDead = false;

    public UnityEvent onDamageTaken;
    public UnityEvent onDeath;
    public UnityEvent onHeal;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float damage)
    {
        if (damage < 0)
        {
            Debug.LogError("Damage cannot be negative, if you meant to heal use Heal method");
            return;
        }
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        // Do we want to invoke both events on death?
        // Should onDamageTaken be invoked on death?
        onDamageTaken.Invoke();
        // only invoke onDeath the first time a unit dies
        if (currentHealth <= 0 && !isDead)
        {
            onDeath.Invoke();
            isDead = true;
        }
    }

    public void Heal(float heal)
    {
        if (heal < 0)
        {
            Debug.LogError("Heal cannot be negative, if you meant to damage use Damage method");
            return;
        }
        if (isDead) return;
        currentHealth += heal;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHeal.Invoke();
    }

    public void Revive(float newHealth)
    {
        if (!isDead) return;
        if (newHealth < 0)
        {
            Debug.LogError("New health cannot be negative");
            return;
        }
        currentHealth = newHealth;
        isDead = false;
        onHeal.Invoke();
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        maxHealth = Mathf.Clamp(newMaxHealth, 0, float.MaxValue);
        if (maxHealth == 0)
        {
            currentHealth = 0;
            onDeath.Invoke();
            return;
        }
        maxHealth = newMaxHealth;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
