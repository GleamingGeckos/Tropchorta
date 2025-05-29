using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] SpawnerForObj spawner;
    [SerializeField] GameObject enemyType;
    [SerializeField] int minNumber;
    [SerializeField] int maxNumber;
    [SerializeField] float respawnDelayMin = 3f;
    [SerializeField] float respawnDelayMax = 5f;

    private List<GameObject> currentEnemyList;
    private bool isWaiting = false;
    private float timer = 0f;

    private void Start()
    {
        currentEnemyList = spawner.SpawnObjects(enemyType, Random.Range(minNumber, maxNumber + 1));
    }

    private void Update()
    {
        currentEnemyList.RemoveAll(e => e == null);

        if (currentEnemyList.Count == 0 && !isWaiting)
        {
            isWaiting = true;
            timer = Random.Range(respawnDelayMin, respawnDelayMax);
        }

        if (isWaiting)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                currentEnemyList = spawner.SpawnObjects(enemyType, Random.Range(minNumber, maxNumber + 1));
                isWaiting = false;
            }
        }
    }
}
