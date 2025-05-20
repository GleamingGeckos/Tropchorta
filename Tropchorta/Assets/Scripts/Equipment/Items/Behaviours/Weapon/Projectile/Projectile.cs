using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 200f;
    public float slowdownRate = 1f;
    public float minSpeed = 195f;
    public float lifetime = 2f;
    public int damage = 10;
    public string targetTag = "Enemy";

    float currentSpeed;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        currentSpeed = Mathf.Max(currentSpeed - slowdownRate * Time.deltaTime, minSpeed);
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && !other.isTrigger)
        {
            if (other.TryGetComponent(out HealthComponent healthComponent))
            {
                healthComponent.SimpleDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
