using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NotesUIController : MonoBehaviour
{
    [SerializeField] GameObject notePrefab;
    [SerializeField] List<GameObject> notes;
    [SerializeField] int maxNotesAmount;
    [SerializeField] GameObject cluePopup;
    private void Start()
    {
        AddNote("Bestia zostawia jaja w kopczykach");
        AddNote("Bestia zostawia œlady pazurów");
    }
    public void AddNote(string noteText)
    {
        cluePopup.SetActive(true);
        // tu powinno byc jeszcze zalaczenie dzwieku z kodu ale idk jak ten fmod dziala
        if (notes.Count+1 < maxNotesAmount)
        {
            GameObject newNote = Instantiate(notePrefab,transform);
            newNote.transform.SetAsLastSibling();
            notes.Add(newNote);
            newNote.GetComponent<TMP_Text>().text = noteText;
        }
    }
}
