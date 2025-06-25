using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI textField;



    public void ShowCurrentText(DialogueData currentDialogue)
    {
        panel.SetActive(true);
        textField.text = currentDialogue.fullText;
    }

    public void HidePanel()
    {
        panel.SetActive(false);
    }


}
