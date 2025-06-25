using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private GameObject uiMustDoPanel;
    [SerializeField] private TextMeshProUGUI mustTextField;

    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject trace;

    //[SerializeField] private GameObject uiMustDoPanel;
    //[SerializeField] private GameObject uiMustDoPanel;
    //[SerializeField] private GameObject uiMustDoPanel;
    //[SerializeField] private GameObject uiMustDoPanel;
    //[SerializeField] private GameObject uiMustDoPanel;
    //[SerializeField] private GameObject uiMustDoPanel;
    //[SerializeField] private GameObject uiMustDoPanel;
    //[SerializeField] private GameObject uiMustDoPanel;
    //[SerializeField] private GameObject uiMustDoPanel;

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
                mustTextField.text = "Use [N] to open Notes";
                uiMustDoPanel.SetActive(true);
                canMove = false;
                checkNotebook = true;
                ShowDialogue();
                break;

            case 13:
                mustTextField.text = "Use [E] to collect clue";
                uiMustDoPanel.SetActive(true);
                canMove = false;
                checkTrail = true;
                trace.SetActive(true);
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

            default:
                ShowDialogue(); // od razu przejdü dalej, jeúli nie wymaga akcji
                break;
        }

    }

    private void CheckShift()         //Sprawdzamy czy gracz naciúnie shifta, jesli tak moze przejsc dalej
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            uiMustDoPanel.SetActive(false);
            checkShift = false;
            canMove = true;
        }
    }
    private void CheckCombo()
    {

        uiMustDoPanel.SetActive(false);
        checkCombo = false;
        canMove = true;
    }
    private void CheckBlock()
    {
        uiMustDoPanel.SetActive(false);
        checkBlock = false;
        canMove = true;
    }
    private void CheckDodge()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            uiMustDoPanel.SetActive(false);
            checkDodge = false;
            canMove = true;
        }

    }
    private void CheckBestiary()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            uiMustDoPanel.SetActive(false);
            checkBestiary = false;
            canMove = true;
        }
    }
    private void CheckNotebook()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            uiMustDoPanel.SetActive(false);
            checkNotebook = false;
            canMove = true;
        }
    }
    private void CheckTrail()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            uiMustDoPanel.SetActive(false);
            checkTrail = false;
            canMove = true;
        }
    }
    private void CheckEq()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            uiMustDoPanel.SetActive(false);
            checkEq = false;
            canMove = true;
        }
    }
    private void CheckMap()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            uiMustDoPanel.SetActive(false);
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
