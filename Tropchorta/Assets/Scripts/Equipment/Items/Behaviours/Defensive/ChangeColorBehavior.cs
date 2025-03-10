using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewBehaviour", menuName = "Inventory/DefensiveBehaviors/ColorChangeBehavior", order = 1)]
public class ChangeColorBehavior : DefensiveBehaviour 
{ 

    public Color weaponColor = Color.red;
    public float duration = 2f;  // Duration in seconds before reverting

    private Coroutine activeCoroutine;  // Tracks the currently running coroutine
    private Color originalColor;        // Stores the original color persistently
    private bool originalColorStored = false;  // Ensure we store the original color only once

    public override void Use(Transform user)
    {
        Renderer weaponRenderer = user.GetComponentInChildren<Renderer>();
        if (weaponRenderer != null)
        {
            // Store the original color only once
            if (!originalColorStored)
            {
                originalColor = weaponRenderer.material.color;
                originalColorStored = true;
            }

            // If coroutine is running, stop it to reset the timer
            MonoBehaviour userMono = user.GetComponent<MonoBehaviour>();
            if (activeCoroutine != null)
            {
                userMono.StopCoroutine(activeCoroutine);
            }

            // Start the coroutine again with a fresh timer
            activeCoroutine = userMono.StartCoroutine(ChangeColorTemporary(weaponRenderer));
        }
    }

    public override void ClearData(Transform user)
    {
        throw new System.NotImplementedException();
    }
    private IEnumerator ChangeColorTemporary(Renderer renderer)
    {
        // Change to the desired color
        renderer.material.color = weaponColor;

        yield return new WaitForSeconds(duration);  // Wait for the specified duration

        // Revert to the original color
        renderer.material.color = originalColor;

        activeCoroutine = null;           // Reset coroutine reference after completion
        originalColorStored = false;      // Allow color storage for future uses
    }
}
