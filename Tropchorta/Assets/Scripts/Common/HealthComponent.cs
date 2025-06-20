using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;

    protected bool isDead = false;
    
    public bool isInvulnerable = false;

    // Event holds (damageTaken, currentHealth)
    public UnityEvent<float, float> onDamageTaken;
    public UnityEvent onDeath;

    // Event holds (healValue, currentHealth)
    public UnityEvent<float, float> onHeal;

    public UnityEvent<AttackData> onAttacked;

    public UnityEvent<AttackData> afterAttack;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    // this is for unavoidable dmaage like bows, traps, etc.
    public virtual void SimpleDamage(AttackData ad)
    {
        if (isInvulnerable)
            return;
        if (ad.damage < 0)
        {
            Debug.LogError("Damage cannot be negative, if you meant to heal use Heal method");
            return;
        }
        currentHealth -= ad.damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        // Do we want to invoke both events on death?
        // Should onDamageTaken be invoked on death?
        onDamageTaken.Invoke(ad.damage, currentHealth);
        // only invoke onDeath the first time a unit dies
        if (currentHealth <= 0 && !isDead)
        {
            onDeath.Invoke();
            isDead = true;
            return;
        }
        afterAttack.Invoke(ad);
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
        //Debug.Log("Heal " + heal);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHeal.Invoke(heal, currentHealth);
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
        onHeal.Invoke(newHealth, currentHealth);
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
