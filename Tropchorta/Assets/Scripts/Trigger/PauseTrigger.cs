using System.Collections.Generic;
using UnityEngine;

public class PauseTrigger : MonoBehaviour
{
    [Header("Dialogi do wyœwietlenia")]
    [SerializeField] private List<DialogueData> dialogueSequence;
    [SerializeField] private DialoguePlayer dialoguePlayer;

    private bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasBeenTriggered = true;
            dialoguePlayer.StartDialogue(dialogueSequence);
        }
    }
}
