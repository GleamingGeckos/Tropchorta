using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToBossFight : MonoBehaviour
{
    public string _levelName;
    private PlayerMovement _playerMovement;
    private bool _isIn;

    [Header("Panel")]
    [SerializeField] private GameObject _screenCanvas;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    private void Start()
    {
        _screenCanvas.SetActive(false);
        _yesButton.onClick.AddListener(OnYesClicked);
        _noButton.onClick.AddListener(OnNoClicked);
    }

    private void OnYesClicked()
    {
        if (_playerMovement != null)
        {
            _playerMovement.playerState.state = PlayerState.Normal;
            _playerMovement.gameObject.GetComponent<CharacterController>().enabled = false;
            Vector3 position = _playerMovement.gameObject.transform.position;
            position.x = 0;
            position.z = 0;
            _playerMovement.gameObject.transform.position = position;
            _playerMovement.gameObject.GetComponent<CharacterController>().enabled = true;
        }
        SceneManager.LoadScene(_levelName);
    }

    private void OnNoClicked()
    {
        _screenCanvas.SetActive(false);
        if (_playerMovement != null)
        {
            _playerMovement.playerState.state = PlayerState.Normal;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !_isIn)
        {
            _playerMovement = other.GetComponent<PlayerMovement>();
            if (_playerMovement != null)
            {
                _playerMovement.playerState.state = PlayerState.DisableInput;
                _isIn = true;
            }
            _screenCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            _isIn = false;
    }


}
