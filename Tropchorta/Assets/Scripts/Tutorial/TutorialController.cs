using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public static bool disableInput = false;

    [SerializeField] private DialoguePlayer dialoguePlayer;
    [SerializeField] private List<DialogueData> startDialogue;
    [SerializeField] private GameObject swordObject;

    private bool swordActivated = false;

    private void Start()
    {
        if (startDialogue != null && startDialogue.Count > 0)
        {
            dialoguePlayer.OnDialogueAdvanced += OnDialogueProgressed;
            dialoguePlayer.StartDialogue(startDialogue);
        }
    }

    private void Update()
    {
        PauseController.DisableInput(disableInput);
    }

    private void OnDialogueProgressed(int index)
    {
        if (!swordActivated && index == 2 && swordObject != null)
        {
            swordObject.SetActive(true);
            swordActivated = true;
        }
    }
}
