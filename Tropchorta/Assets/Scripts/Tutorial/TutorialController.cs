using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private GameObject uiCombo3Attacks;
    [SerializeField] private GameObject uiUseShift;
    [SerializeField] private GameObject uiPerfectBlock;
    [SerializeField] private GameObject uiDodge;
    [SerializeField] private GameObject uiBestiary;
    [SerializeField] private GameObject uiNotebook;
    [SerializeField] private GameObject uiTrail;
    [SerializeField] private GameObject uiEq;
    [SerializeField] private GameObject uiMap;

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
            else if (checkNotebook)
            {
                CheckNotebook();
            }
            else if (checkTrail)
            {
                CheckTrail();
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

    private void Advance()              //Przechodzimy dalej
    {
        currentStep++;
        // Decyzja co dalej zaleøna od etapu
        switch (currentStep)
        {
            case 3:
                swordModel.SetActive(true);
                PauseController.DisableInput(false);
                ShowDialogue(); 
                break;

            case 5:
                uiCombo3Attacks.SetActive(true);
                canMove = false;
                checkCombo = true;
                ShowDialogue();
                break;

            case 6:
                uiUseShift.SetActive(true);
                canMove = false;
                checkShift = true;
                ShowDialogue();
                break;

            case 8:
                uiPerfectBlock.SetActive(true);
                canMove = false;
                checkBlock = true;
                ShowDialogue();
                break;

            case 9:
                uiDodge.SetActive(true);
                canMove = false;
                checkDodge = true;
                ShowDialogue();
                break;

            case 11:
                uiBestiary.SetActive(true);
                canMove = false;
                checkBestiary = true;
                ShowDialogue();
                break;

            case 12:
                uiNotebook.SetActive(true);
                canMove = false;
                checkNotebook = true;
                ShowDialogue();
                break;

            case 13:
                uiTrail.SetActive(true);
                canMove = false;
                checkTrail = true;
                ShowDialogue();
                break;

            case 15:
                uiEq.SetActive(true);
                canMove = false;
                checkEq = true;
                ShowDialogue();
                break;

            case 16:
                uiMap.SetActive(true);
                canMove = false;
                checkMap = true;
                ShowDialogue();
                break;

            default:
                ShowDialogue(); // od razu przejdü dalej, jeúli nie wymaga akcji
                break;
        }

    }

    private void CheckShift()         //Sprawdzamy czy gracz naciúnie shifta, jesli tak moze przejsc dalej
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            uiUseShift.SetActive(false);
            checkShift = false;
            canMove = true;
        }
    }
    private void CheckCombo()
    {

        uiCombo3Attacks.SetActive(false);
        checkCombo = false;
        canMove = true;
    }
    private void CheckBlock()
    {
        uiPerfectBlock.SetActive(false);
        checkBlock = false;
        canMove = true;
    }
    private void CheckDodge()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            uiDodge.SetActive(false);
            checkDodge = false;
            canMove = true;
        }

    }
    private void CheckBestiary()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            uiBestiary.SetActive(false);
            checkBestiary = false;
            canMove = true;
        }
    }
    private void CheckNotebook()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            uiNotebook.SetActive(false);
            checkNotebook = false;
            canMove = true;
        }
    }
    private void CheckTrail()
    {
        uiTrail.SetActive(false);
        checkTrail = false;
        canMove = true;

    }
    private void CheckEq()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            uiEq.SetActive(false);
            checkEq = false;
            canMove = true;
        }
    }
    private void CheckMap()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            uiMap.SetActive(false);
            checkMap = false;
            canMove = true;
        }
    }

    private void ExitTutorial()
    {

    }

    private void ShowDialogue()
    {
        dialoguePlayer.ShowCurrentText(dialogueList[currentStep-1]);
    }

    private void OnDestroy()
    {
        playerCombat.OnCombo3Attacks -= CheckCombo;
        playerCombat.OnPerfectParry -= CheckBlock;
    }
}
