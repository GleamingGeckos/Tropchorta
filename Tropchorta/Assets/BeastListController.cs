using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BeastListController : MonoBehaviour
{
    [Header("Wskazówki – konkretne ImageCyclery")]
    [SerializeField] private List<ImageCycler> traitsList;

    [Header("Obrazek do zmiany przezroczystoœci")]
    [SerializeField] private Image targetImage;

    [Header("Ustawienia przezroczystoœci")]
    [Range(0f, 1f)] public float fadedAlpha = 0.3f;
    public float normalAlpha = 1f;

    private void OnEnable()
    {
        foreach (var cycler in traitsList)
        {
            if (cycler != null)
                cycler.OnIndexChanged += OnTraitIndexChanged;
        }
    }

    private void OnDisable()
    {
        foreach (var cycler in traitsList)
        {
            if (cycler != null)
                cycler.OnIndexChanged -= OnTraitIndexChanged;
        }
    }

    private void OnTraitIndexChanged(ImageCycler changedCycler, int newIndex)
    {
        bool anyHasIndex2 = false;

        foreach (var cycler in traitsList)
        {
            if (cycler != null && cycler.currentIndex == 2)
            {
                anyHasIndex2 = true;
                break;
            }
        }

        if (targetImage != null)
        {
            Color c = targetImage.color;
            c.a = anyHasIndex2 ? fadedAlpha : normalAlpha;
            targetImage.color = c;

            Debug.Log("BeastListController: Zmieniono przezroczystoœæ na " + c.a);
        }
    }
}
