using UnityEngine;
using System.Collections;
using DG.Tweening;

[CreateAssetMenu(fileName = "NewBehaviour", menuName = "Inventory/WeaponBehaviors/ColorChangeBehavior", order = 3)]
public class ColorChangeBehavior : WeaponBehavior
{
    public Color weaponColor = Color.red;
    public float duration = 2f;  // Duration in seconds before reverting

    private Coroutine activeCoroutine;  // Tracks the currently running coroutine
    private Color originalColor;        // Stores the original color persistently
    private bool originalColorStored = false;  // Ensure we store the original color only once

    private Tween tween;

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

            if (!tween.IsActive())
            {
                tween.Kill();
            }

            ChangeColorTemporary(weaponRenderer);
        }
    }

    public override void ClearData(Transform user)
    {
        throw new System.NotImplementedException();
    }
    private void ChangeColorTemporary(Renderer renderer)
    {
        // Change to the desired color
        renderer.material.color = weaponColor;

        tween = renderer.material.DOColor(originalColor, duration).SetEase(Ease.Linear).OnComplete(() => {
            // Revert to the original color
            renderer.material.color = originalColor;

            activeCoroutine = null;           // Reset coroutine reference after completion
            originalColorStored = false;      // Allow color storage for future uses
            tween = null;   // ensure tween is null after completion
        });
    }

    public override void StopUse(Transform user)
    {
        throw new System.NotImplementedException();
    }

    public override void AltUse(Transform user)
    {
        throw new System.NotImplementedException();
    }
}

