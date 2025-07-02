using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BeastListController : MonoBehaviour
{
    [Header("Lista A – reaguje na zle")]
    [SerializeField] private List<ImageCycler> traitsListIndex2;

    [Header("Lista B – reaguje na dobre")]
    [SerializeField] private List<ImageCycler> traitsListIndex1;

    [Header("Obrazek do zmiany przezroczystoœci")]
    [SerializeField] private Image targetImage;

    [Header("Ustawienia przezroczystoœci")]
    [Range(0f, 1f)] public float fadedAlpha = 0.3f;
    public float normalAlpha = 1f;

    private void OnEnable()
    {
        foreach (var cycler in traitsListIndex2)
        {
            if (cycler != null)
                cycler.OnIndexChanged += OnAnyTraitIndexChanged;
        }

        foreach (var cycler in traitsListIndex1)
        {
            if (cycler != null)
                cycler.OnIndexChanged += OnAnyTraitIndexChanged;
        }
    }

    private void OnDisable()
    {
        foreach (var cycler in traitsListIndex2)
        {
            if (cycler != null)
                cycler.OnIndexChanged -= OnAnyTraitIndexChanged;
        }

        foreach (var cycler in traitsListIndex1)
        {
            if (cycler != null)
                cycler.OnIndexChanged -= OnAnyTraitIndexChanged;
        }
    }

    private void OnAnyTraitIndexChanged(ImageCycler changedCycler, int newIndex)
    {
        UpdateAlphaState();
    }

    private void UpdateAlphaState()
    {
        bool shouldFade = false;

        foreach (var cycler in traitsListIndex2)
        {
            if (cycler != null && cycler.currentIndex == 2)
            {
                shouldFade = true;
                break;
            }
        }

        if (!shouldFade)
        {
            foreach (var cycler in traitsListIndex1)
            {
                if (cycler != null && cycler.currentIndex == 1)
                {
                    shouldFade = true;
                    break;
                }
            }
        }

        if (targetImage != null)
        {
            Color c = targetImage.color;
            c.a = shouldFade ? fadedAlpha : normalAlpha;
            targetImage.color = c;

            //Debug.Log($"[BeastListController] Zmieniono przezroczystoœæ na {(shouldFade ? "FADED" : "NORMAL")} (a={c.a})");
        }
    }
}
