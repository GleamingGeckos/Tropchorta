using UnityEngine;

public class ClueController : MonoBehaviour
{
    [SerializeField] NotesUIController notesUIController;
    [SerializeField] string clueText;
    private bool isActivated = false;//true when the clue is activated and player cannot interact with it anymore
    private bool isReady = false;//true when the player is in close proximity and can use the clue
    [SerializeField] GameObject display;
    [SerializeField] GameObject before;
    [SerializeField] GameObject after;

    [SerializeField] bool needsClueItem; // whether player needs a special clue item to activate the clue
    [SerializeField] ClueItem clueItem;
    [SerializeField] private EquipmentController equipmentController;

    private void Start()
    {
        //equipmentController = GameObject.FindWithTag("Equipment").GetComponent<EquipmentController>();
        if (equipmentController == null)
        {
            equipmentController = GameObject.FindGameObjectWithTag("Equipment").GetComponent<EquipmentController>();
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnPlayerGetClue();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !isActivated)
        {
            OnPlayerEnter();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isActivated && isReady)
        {
            OnPlayerExit();
        }
    }
    void OnPlayerEnter()
    {
        isReady = true;
        display.SetActive(true);
    }
    void OnPlayerExit()
    {
        isReady = false;
        display.SetActive(false);
    }
    
    void OnPlayerGetClue() // activated when player gets the clue to the notes
    {
        //Debug.Log(needsClueItem);
        //Debug.Log(equipmentController.HasClueItem(clueItem));
        if (needsClueItem && equipmentController.HasClueItem(clueItem))
        {
            if (!isActivated && isReady)
            {
                display.SetActive(false);
                before.SetActive(false);
                after.SetActive(true);
                isActivated = true;
                isReady = false;
                notesUIController.AddNote(clueText);
            }
        }
        else if(!needsClueItem)
        {
            if (!isActivated && isReady)
            {
                display.SetActive(false);
                before.SetActive(false);
                after.SetActive(true);
                isActivated = true;
                isReady = false;
                notesUIController.AddNote(clueText);
            }
        }
    }

}
