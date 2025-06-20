using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField] float damageValue;
    [SerializeField] float cooldown;
    [SerializeField] string whoToDamage;
    [SerializeField] CharmType charmType;

    private float lastHitTime = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == whoToDamage)
        {
            if (Time.time >= lastHitTime + cooldown)
            {
                var health = other.GetComponent<HealthComponent>();
                if (health)
                {
                    health.SimpleDamage(new AttackData(gameObject, damageValue, charmType));
                    lastHitTime = Time.time;
                }
            }
        }
    }
}
