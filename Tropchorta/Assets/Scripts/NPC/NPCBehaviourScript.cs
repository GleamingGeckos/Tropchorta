using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCBehaviourScript : MonoBehaviour
{

    [SerializeField] private GameObject _canvas;
    [SerializeField] private float delay = 5f;
    private bool _isTalking;

    [Header("Panel")]
    [SerializeField] private GameObject _screenCanvas;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _description;

    [Header("Quest")]
    [SerializeField] private bool _isQuestActive;
    [SerializeField] private SpawnerForObj _spawner;
    [SerializeField] private GameObject _enemyToSpawn;
    [SerializeField] private int _howManyToSpawn = 3;
    [SerializeField] private List<GameObject> _enemies;
    [SerializeField] private GameObject _reward;
    [SerializeField] private string _QuestMassage;
    [SerializeField] private string _FinishedMassage;


    private void Awake()
    {
        _canvas.SetActive(false);
        _screenCanvas.SetActive(false);
        _isTalking = false;
    }

    private void Update()
    {
        if (_isQuestActive)
        {
            _enemies.RemoveAll(item => item == null);
            if(_enemies.Count == 0)
            {
                _isQuestActive = false;

                if(_reward != null)
                    _spawner.SpawnObjects(_reward, 1);
                _screenCanvas.SetActive(true);
                _description.text = _FinishedMassage;
                StartCoroutine(DestroyAfterDelay());
            }
        }
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
            _isQuestActive = true;
            _description.text = _QuestMassage;
            _screenCanvas.SetActive(true);
            _enemies = _spawner.SpawnObjects(_enemyToSpawn, _howManyToSpawn);
        }
        else
        {
            _isTalking = false;
            _screenCanvas.SetActive(false);
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
