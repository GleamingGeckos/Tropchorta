using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    private Material material;
    private Color originalColor;
    private Color flashColor = new Color(1f, 0f, 0f); // Red color
    private float transitionDuration = 1f;
    private Tween colorTween;
    private Tween shakeTween;
    [SerializeField] GameObject moneyPrefab;

    //Components
    private EnemyCombat _enemyCombat;
    private EnemyMovement _enemyMovement;

    void Start()
    {
        material = new Material(GetComponentInChildren<Renderer>().sharedMaterial);
        GetComponentInChildren<Renderer>().material = material;
        _enemyCombat = GetComponent<EnemyCombat>();
        _enemyMovement = GetComponent<EnemyMovement>();
        originalColor = material.color; // Store the original color
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if (collision.gameObject.tag == "Player")
        // {
        //     collision.gameObject.GetComponent<HealthComponent>().Damage(_enemyCombat.DealDamage());
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !_enemyMovement.isStuned)
        {
            _enemyMovement.StartChasing(other.gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _enemyMovement.StopChasing();   
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !_enemyMovement.isStuned)
        {
            GameObject player = other.gameObject;
            float distanceSqr = (player.transform.position - transform.position).sqrMagnitude;

            if (distanceSqr < 6.0f)
            {
                if (Random.value < 0.7f)
                    _enemyCombat.Attack(); // czasami bli¿szy
                else
                    _enemyCombat.StrongAttack(); // czasami dalszy
            }
            _enemyMovement.RotateTowards(player);
        }
    }
  
    public void OnDeath()
    {
        Instantiate(moneyPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void OnTakeDamage()
    {
        if (material != null)
        {
            Color change = flashColor;
            material.color = flashColor;
            if (colorTween.IsActive())
            {
                // Kill the tween before starting a new one
                colorTween.Kill();
            }
            colorTween = DOTween.To(() => change, c =>
            {
                change = c;
                material.SetColor("_BaseColor", c);
            }, originalColor, transitionDuration);
            //material.DOColor(originalColor, transitionDuration);
        }

        // Shake pozycji
        if (shakeTween.IsActive())
        {
            shakeTween.Kill(true);
        }

        // Shake obiektu (np. transform this)
        shakeTween = transform.DOShakePosition(0.3f, 0.2f, 10, 90, false, true);
    }
}
