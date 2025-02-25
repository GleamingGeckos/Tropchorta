using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] Animator staffAnimator;
    [SerializeField] HealthBar healthBar;
    [SerializeField] Transform rotatingRootTransform;
    HealthComponent health;

    // array of colliders so that SphereCast doesn't allocate everytime it's called
    Collider[] colliders = new Collider[16];

    void Start()
    {
        GetComponent<PlayerMovement>().input.OnLeftMouseClickEvent += OnAttack;
        health = GetComponent<HealthComponent>();
        health.onDamageTaken.AddListener(onDamageTaken);
    }

    public void onDamageTaken(float damage, float currentHealth)
    {
        healthBar.SetHealth(currentHealth / health.MaxHealth);
    }

    void Attack()
    {
        // everything below this line should be later on handled by a Weapon of some sorts, this is for testing only
        Vector3 rotatingOffset = rotatingRootTransform.forward * 1.5f;
        int hits = Physics.OverlapSphereNonAlloc(transform.position + rotatingOffset, 1f, colliders); // TODO : layermask for damageable objects or enemies?
        for (int i = 0; i < hits; i++)
        {
            // Currently assuming the collider is on the same object as the HealthComponent
            if (colliders[i].TryGetComponent(out HealthComponent healthComponent))
            {
                healthComponent.Damage(10);
            }
        }
        DebugExtension.DebugWireSphere(transform.position + rotatingOffset, Color.red, 1f, 1f);
    }

    void OnAttack()
    {

        // check if the clip is already playing, if it is simply reset it
        if (staffAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            // reset the clip
            staffAnimator.Play("Attack", -1, 0);
        }
        else
        {
            // play the clip, if it's not already playing
            staffAnimator.SetTrigger("Attack");
        }

        Attack();
    }
}
