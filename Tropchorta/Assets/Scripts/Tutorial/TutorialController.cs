using FMODUnity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [Header("Core")]
    [SerializeField] private DialoguePlayer dialoguePlayer;
    [SerializeField] private List<DialogueData> dialogueList;

    [Header("References")]
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerInteract playerInteract;

    [Header("Tutorial Visuals")]
    [SerializeField] private GameObject swordModel;
    [SerializeField] private GameObject uiMustDoPanel;
    [SerializeField] private TextMeshProUGUI mustTextField;

    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject trace;
    [SerializeField] private GameObject traceMask;
    [SerializeField] private List<ImageCycler> imageCyclers;

    [Header("FMOD Sounds")]
    [SerializeField] private List<EventReference> soundList;
    private FMOD.Studio.EventInstance currentTutorialDialogue;

    public int currentStep;

    private bool canMove = true;

    private bool checkCombo;
    private bool checkShift;
    private bool checkBlock;
    private bool checkDodge;
    private bool checkBestiary;
    private bool checkNotebook;
    private bool checkTrail;
    private bool checkEq;
    private bool checkMap;

    private void Start()
    {
        swordModel.SetActive(false);
        currentStep = 1;

        // Subskrybuj event combo
        playerCombat.OnCombo3Attacks += CheckCombo;
        playerCombat.OnPerfectParry += CheckBlock;

        foreach (var cycler in imageCyclers)
        {
            cycler.OnIndexChanged += OnCyclerIndexChanged;
        }
        BestiaryUIController.OnCluesPanelShown += CheckTrail;


        ShowDialogue();
    }

    private void Update()
    {
        if (currentStep < 3)
        {
            PauseController.DisableInput(true);
        }

        if (currentStep > dialogueList.Count)
        {
            dialoguePlayer.HidePanel();
            ExitTutorial();
        }
        else
        {
            if (canMove)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Advance();
                }
            }
            else
            {
                if (checkShift)
                {
                    CheckShift();
                }
                else if (checkDodge)
                {
                    CheckDodge();
                }
                else if (checkBestiary)
                {
                    CheckBestiary();
                }
                else if (checkEq)
                {
                    CheckEq();
                }
                else if (checkMap)
                {
                    CheckMap();
                }
            }
        }

    }

    private void Advance()              //Przechodzimy dalej
    {
        currentStep++;
        // Decyzja co dalej zale�na od etapu
        switch (currentStep)
        {
            case 3:
                swordModel.SetActive(true);
                PauseController.DisableInput(false);
                ShowDialogue();
                break;

            case 4:
                mustTextField.text = "Attack 3 times [LMB]";
                uiMustDoPanel.SetActive(true);
                canMove = false;
                checkCombo = true;
                ShowDialogue();
                break;

            case 6:
                mustTextField.text = "Use [SHIFT] to run";
                uiMustDoPanel.SetActive(true);
                canMove = false;
                checkShift = true;
                ShowDialogue();
                break;

            case 8:
                mustTextField.text = "Use [RMB] to block";
                uiMustDoPanel.SetActive(true);
                canMove = false;
                checkBlock = true;
                enemy.SetActive(true);
                ShowDialogue();
                break;

            case 9:
                mustTextField.text = "Use [SPACE] to dash";
                uiMustDoPanel.SetActive(true);
                enemy.SetActive(false);
                canMove = false;
                checkDodge = true;

                ShowDialogue();
                break;

            case 11:
                mustTextField.text = "Use [B] to open Bestiary";

                uiMustDoPanel.SetActive(true);
                canMove = false;
                checkBestiary = true;
                ShowDialogue();
                break;

            case 12:
                mustTextField.text = "Click one of the Known traits";
                uiMustDoPanel.SetActive(true);
                canMove = false;
                checkNotebook = true;
                ShowDialogue();
                break;

            case 13:
                mustTextField.text = "Open Traits panel in Bestiary";
                uiMustDoPanel.SetActive(true);
                canMove = false;
                checkTrail = true;
                trace.SetActive(true);
                traceMask.SetActive(true);
                ShowDialogue();
                break;

            case 15:
                mustTextField.text = "Use [I] to open Equipment";
                uiMustDoPanel.SetActive(true);
                canMove = false;
                checkEq = true;
                ShowDialogue();
                break;

            case 16:
                mustTextField.text = "Use [M] to open Map";
                uiMustDoPanel.SetActive(true);
                canMove = false;
                checkMap = true;
                ShowDialogue();
                break;
            case 17:
                ShowDialogue();
                break;
            default:
                ShowDialogue(); // od razu przejd� dalej, je�li nie wymaga akcji
                break;
        }

    }

    private void CheckShift()         //Sprawdzamy czy gracz naci�nie shifta, jesli tak moze przejsc dalej
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            uiMustDoPanel.SetActive(false);
            checkShift = false;
            canMove = true;
            Advance();
        }
    }
    private void CheckCombo()
    {

        uiMustDoPanel.SetActive(false);
        checkCombo = false;
        canMove = true;
        Advance();
        OnDestroyCombo();
    }
    private void CheckBlock()
    {
        uiMustDoPanel.SetActive(false);
        checkBlock = false;
        canMove = true;
        Advance();
        OnDestroyBlock();
    }
    private void CheckDodge()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            uiMustDoPanel.SetActive(false);
            checkDodge = false;
            canMove = true;
            Advance();

        }

    }
    private void CheckBestiary()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            uiMustDoPanel.SetActive(false);
            checkBestiary = false;
            canMove = true;
            Advance();
        }
    }
    private void CheckIfKnownTraitsClicked()
    {
            uiMustDoPanel.SetActive(false);
            checkNotebook = false;
            canMove = true;
            Advance();
    }
    private void CheckTrail()
    {
        if (!checkTrail) return;

        uiMustDoPanel.SetActive(false);
        checkTrail = false;
        canMove = true;
        Advance();

        OnDestroyTrail(); // odsubskrybuj po użyciu
    }

    private void CheckEq()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            uiMustDoPanel.SetActive(false);
            checkEq = false;
            canMove = true;
            Advance();
        }
    }
    private void CheckMap()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            uiMustDoPanel.SetActive(false);
            checkMap = false;
            canMove = true;
            Advance();
        }
    }

    private void ExitTutorial()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void ShowDialogue()
    {
        dialoguePlayer.ShowCurrentText(dialogueList[currentStep - 1]);
        // stop previous dialogue if it is still running (aka is valid)
        if (currentTutorialDialogue.isValid())
        {
            currentTutorialDialogue.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            currentTutorialDialogue.release();
            currentTutorialDialogue.clearHandle();
        }
        if (currentStep < 17)
        {
            // start new dialogue
            currentTutorialDialogue = RuntimeManager.CreateInstance(soundList[currentStep - 1]);
            currentTutorialDialogue.start();
        }

    }

    private void OnDestroyCombo()
    {
        playerCombat.OnCombo3Attacks -= CheckCombo;
    }

    private void OnDestroyBlock()
    {
        playerCombat.OnPerfectParry -= CheckBlock;
    }

    private void OnCyclerIndexChanged(ImageCycler sender, int newIndex)
    {
        if (checkNotebook)
        {
            CheckIfKnownTraitsClicked();
        }
    }


    private void OnDestroy()
    {
        if (currentTutorialDialogue.isValid())
        {
            currentTutorialDialogue.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            currentTutorialDialogue.release();
            currentTutorialDialogue.clearHandle();
        }

        foreach (var cycler in imageCyclers)
        {
            cycler.OnIndexChanged -= OnCyclerIndexChanged;
        }
    }

    private void OnDestroyTrail()
    {
        BestiaryUIController.OnCluesPanelShown -= CheckTrail;
    }

}
