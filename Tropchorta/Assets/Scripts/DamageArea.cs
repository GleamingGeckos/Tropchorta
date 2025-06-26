using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField] float damageValue;
    [SerializeField] float cooldown;
    [SerializeField] string whoToDamage;
    [SerializeField] CharmType charmType;

    private float lastHitTime = 0f;
    public void Initialize(float damage, float cd, string targetTag, CharmType type)
    {
        damageValue = damage;
        cooldown = cd;
        whoToDamage = targetTag;
        charmType = type;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == whoToDamage)
        {
            if (Time.time >= lastHitTime + cooldown)
            {
                var healthPlayer = other.GetComponent<PlayerHealthComponent>();
                var health = other.GetComponent<HealthComponent>();
                if (healthPlayer)
                {
                    healthPlayer.UnblockableDamage(new AttackData(gameObject, damageValue, charmType));
                    lastHitTime = Time.time;
                }else if (health != null)
                {
                    health.SimpleDamage(new AttackData(gameObject, damageValue, charmType));
                    lastHitTime = Time.time;
                }
            }
        }
    }
}
