using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 200f;
    public float slowdownRate = 1f;
    public float minSpeed = 195f;
    public float lifetime = 2f;
    private int _damage = 10;
    public string targetTag = "Enemy";
    private Transform _target;

    float currentSpeed;
    [SerializeField] private float motionScale = 1f;
    [SerializeField] private float motionSpeed = 1f;
    public CharmType charmType = CharmType.None;

    private float motionTime;

    public AnimationCurve motionCurveX;
    public AnimationCurve motionCurveY;
    public AnimationCurve motionCurveZ;


    [Header("For Enemy only")]
    [SerializeField] private GameObject _arrowForPar;

    public void Initialize(Transform target, CharmType charmType, int damage)
    {
        _target = target;
        _damage = damage;
        this.charmType = charmType;
    }

    void Start()
    {
        currentSpeed = speed;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (_target != null)
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
        }
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
            if (other.TryGetComponent(out PlayerHealthComponent playerHealthComponent) &&
                other.TryGetComponent(out PlayerCombat playerCombatComponent) &&
                other.TryGetComponent(out PlayerMovement playerMovementComponent) &&
                transform.parent.TryGetComponent(out EnemyMovement enemyMovement) &&
                !other.isTrigger)
            {
                playerMovementComponent.RotatePlayerTowards(transform.parent.position);
                
                if (enemyMovement.perfectParWasInitiated && playerCombatComponent.isBlocking && _arrowForPar != null)
                {
                    var revange = Instantiate(_arrowForPar, other.transform.position, transform.rotation * Quaternion.Euler(0, 180, 0), other.transform);
                    revange.GetComponent<Projectile>().charmType = charmType;
                    enemyMovement.perfectParWasInitiated = false;
                }
                else if (!playerCombatComponent.isBlocking)
                {
                    playerHealthComponent.SimpleDamage(new AttackData(transform.parent.gameObject, _damage, charmType));
                }
            }else if (other.TryGetComponent(out HealthComponent healthComponent) && 
                other.TryGetComponent(out EnemyCombat enemyCombat) && 
                other.CompareTag("Enemy") && 
                other.TryGetComponent(out EnemyMovement enemyMovement2) )
            {
                AttackData attackData;
                if (transform.parent != null && transform.parent.parent != null)
                    attackData = new AttackData(transform.parent.parent.gameObject, _damage, charmType);
                else
                    attackData = new AttackData(gameObject, _damage, charmType);
                healthComponent.SimpleDamage(Charm.CharmEffectOnWeapon(attackData, enemyCombat.WeakToCharm, Charm.weaponAmplificationMultiplier));
            }
            Destroy(gameObject);
        }
    }
}
