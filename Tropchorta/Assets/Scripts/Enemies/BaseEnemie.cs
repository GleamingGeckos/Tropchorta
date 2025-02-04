using System.Collections;
using UnityEngine;

public class BaseEnemie : MonoBehaviour
{
    // React to damge
    private Renderer rend;
    private Color originalColor;
    private Color flashColor = new Color(1f, 0f, 0f); // Red color
    private float transitionDuration = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color; // Store the original color
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
        if (rend != null)
        {
            StartCoroutine(FlashColor());
        }
    }

    IEnumerator FlashColor()
    {
        // Zmieniaj kolor na czerwony
        rend.material.color = flashColor;

        // P³ynnie przejœcie z czerwonego na oryginalny kolor
        float timeElapsed = 0f;
        while (timeElapsed < transitionDuration)
        {
            rend.material.color = Color.Lerp(flashColor, originalColor, timeElapsed / transitionDuration);
            timeElapsed += Time.deltaTime;
            yield return null; // Czekaj do nastêpnej klatki
        }

        // Gwarantujemy, ¿e skoñczymy na oryginalnym kolorze
        rend.material.color = originalColor;
    }
}
