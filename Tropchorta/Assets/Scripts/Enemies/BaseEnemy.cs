using DG.Tweening;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    private Material material;
    private Color originalColor;
    private Color flashColor = new Color(1f, 0f, 0f); // Red color
    private float transitionDuration = 1f;
    private Tween colorTween;
    private EnemyCombat enemyCombat;

    void Start()
    {
        material = GetComponentInChildren<Renderer>().sharedMaterial;
        enemyCombat = GetComponent<EnemyCombat>();
        originalColor = material.color; // Store the original color
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthComponent>().Damage(enemyCombat.DealDamage());
        }
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }

    public void OnTakeDamage()
    {
        if (material != null)
        {
            material.color = flashColor;
            if (colorTween.IsActive())
            {
                // Kill the tween before starting a new one
                colorTween.Kill();
            }
            colorTween = material.DOColor(originalColor, transitionDuration);
        }
    }
}
