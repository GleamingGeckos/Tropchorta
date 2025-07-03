using NUnit.Framework.Internal;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    GameObject _parent;

    private float motionTime;

    public AnimationCurve motionCurveX;
    public AnimationCurve motionCurveY;
    public AnimationCurve motionCurveZ;

    [Header("Effects")]
    [SerializeField] private GameObject _effectFire;
    [SerializeField] private GameObject _effectPoison;
    [SerializeField] private GameObject _effectLightning;

    [Header("For Enemy only")]
    [SerializeField] private GameObject _arrowForPar;

    public void Initialize(Transform target, CharmType charmType, int damage, GameObject parent)
    {
        _target = target;
        _damage = damage;
        this.charmType = charmType;
        _parent = parent;
    }

    public void EnableCharmEffect()
    {
        GameObject effect = charmType switch
        {
            CharmType.Fire => _effectFire,
            CharmType.Poison => _effectPoison,
            CharmType.Lightning => _effectLightning,
            _ => null
        };

        if (effect != null)
            effect.SetActive(true);

    }

    void Start()
    {
        EnableCharmEffect();
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
                _parent.transform.TryGetComponent(out EnemyMovement enemyMovement) &&
                other is CapsuleCollider &&
                !other.isTrigger)
            {
                playerMovementComponent.RotatePlayerTowards(_parent.transform.position);
                
                if (enemyMovement.perfectParWasInitiated && playerCombatComponent.isBlocking && _arrowForPar != null)
                {
                    Quaternion rot = Quaternion.LookRotation(_parent.transform.position + transform.up - other.transform.position);
                    var revange = Instantiate(_arrowForPar, other.transform.position, rot, other.transform);
                    revange.GetComponent<Projectile>().Initialize(null, charmType, 1, other.gameObject);
                    enemyMovement.perfectParWasInitiated = false;
                }
                else if (!playerCombatComponent.isBlocking)
                {
                    playerHealthComponent.SimpleDamage(new AttackData(_parent.transform.gameObject, _damage, charmType));
                }
            }else if (other.TryGetComponent(out HealthComponent healthComponent) && 
                other.TryGetComponent(out EnemyCombat enemyCombat) && 
                other.CompareTag("Enemy"))
            {
                AttackData attackData;
                attackData = new AttackData(_parent, _damage, charmType);
                healthComponent.SimpleDamage(Charm.CharmEffectOnWeapon(attackData, enemyCombat.WeakToCharm, Charm.weaponAmplificationMultiplier));
            }
            Destroy(gameObject);
        }
    }
}
