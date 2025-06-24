using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialoguePlayer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI textField;

    private List<DialogueData> currentDialogue;
    private int currentIndex = 0;

    public bool IsPlaying => panel.activeSelf;

    public Action<int> OnDialogueAdvanced; // callback z indeksem aktualnej linijki

    private void Update()
    {
        // Jeœli dialog jest aktywny i wciœniêto Enter (Return) - przejdŸ do nastêpnego tekstu
        if (IsPlaying && Input.GetKeyDown(KeyCode.Return))
        {
            Next();
        }
    }

    public void StartDialogue(List<DialogueData> sequence)
    {
        if (sequence == null || sequence.Count == 0) return;

        currentDialogue = sequence;
        currentIndex = 0;

        panel.SetActive(true);
        ShowCurrentText();
        //PauseController.SetPause(true);
        TutorialController.disableInput = true;

        OnDialogueAdvanced?.Invoke(currentIndex);
    }

    public void Next()
    {
        currentIndex++;

        if (currentIndex >= currentDialogue.Count)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentText();
            OnDialogueAdvanced?.Invoke(currentIndex);
        }
    }

    private void ShowCurrentText()
    {
        textField.text = currentDialogue[currentIndex].fullText;
    }

    private void EndDialogue()
    {
        panel.SetActive(false);
        //PauseController.SetPause(false);
        TutorialController.disableInput = false;
    }
}
