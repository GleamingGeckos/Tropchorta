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
    [SerializeField] private float motionScale = 1f;
    [SerializeField] private float motionSpeed = 1f;

    private float motionTime;

    public AnimationCurve motionCurveX;
    public AnimationCurve motionCurveY;
    public AnimationCurve motionCurveZ;

    void Start()
    {
        currentSpeed = speed;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        motionTime += Time.deltaTime * motionSpeed;

        float offsetX = motionCurveX.Evaluate((motionTime)%1) * motionScale;
        float offsetY = motionCurveY.Evaluate((motionTime)%1) * motionScale;
        float offsetZ = motionCurveZ.Evaluate((motionTime)%1) * motionScale;

        Vector3 lateralOffset = Vector3.right * offsetX + Vector3.up * offsetY + Vector3.forward * offsetZ;

        currentSpeed = Mathf.Max(currentSpeed - slowdownRate * Time.deltaTime, minSpeed);
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime + lateralOffset * Time.deltaTime);
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
