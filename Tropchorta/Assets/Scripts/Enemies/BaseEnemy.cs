using DG.Tweening;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    private Material material;
    private Color originalColor;
    private Color flashColor = new Color(1f, 0f, 0f); // Red color
    private float transitionDuration = 1f;
    private Tween colorTween;

    void Start()
    {
        material = GetComponentInChildren<Renderer>().sharedMaterial;
        originalColor = material.color; // Store the original color
    }

    private void OnCollisionEnter(Collision collision)
    {

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
