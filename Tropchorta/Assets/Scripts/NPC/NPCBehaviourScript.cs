using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCBehaviourScript : MonoBehaviour
{

    [SerializeField] private GameObject _canvas;
    private bool _isTalking;

    [Header("Panel")]
    [SerializeField] private GameObject _screenCanvas;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _description;

    private void Awake()
    {
        _canvas.SetActive(false);
        _screenCanvas.SetActive(false);
        _isTalking = false;
    }

    void Start()
    {
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _canvas.SetActive(true);
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _canvas.SetActive(false);
            _screenCanvas.SetActive(false);
        } 
    }

    public void TalkBack()
    {
        if (!_isTalking)
        {
            _isTalking = true;
            _screenCanvas.SetActive(true);
        }
        else
        {
            _isTalking = false;
            _screenCanvas.SetActive(false);
        }
    }


}
