using NUnit.Framework;
using System;
using System.Collections.Generic;
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

    [Header("Quest")]
    [SerializeField] private bool _bQuestActive;
    [SerializeField] private SpawnerForQuests _spawner;
    [SerializeField] private GameObject _enemyToSpawn;
    [SerializeField] private int _howManyToSpawn = 3;
    [SerializeField] private List<GameObject> _enemies;
    [SerializeField] private GameObject _reward;

    private void Awake()
    {
        _canvas.SetActive(false);
        _screenCanvas.SetActive(false);
        _isTalking = false;
    }

    private void Update()
    {
        if (_bQuestActive)
        {
            _enemies.RemoveAll(item => item == null);
            if(_enemies.Count == 0)
            {
                _bQuestActive = false;
                _spawner.SpawnObjects(_reward, 1);
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
            _bQuestActive = true;
            _screenCanvas.SetActive(true);
            _enemies = _spawner.SpawnObjects(_enemyToSpawn, _howManyToSpawn);
        }
        else
        {
            _isTalking = false;
            _screenCanvas.SetActive(false);
        }
    }
}
