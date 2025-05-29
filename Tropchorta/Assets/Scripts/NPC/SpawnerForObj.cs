using System.Collections.Generic;
using UnityEngine;

public class SpawnerForObj : MonoBehaviour
{
    [SerializeField] private SphereCollider spawnArea;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    //Todo fix spawning hight
    public List<GameObject> SpawnObjects(GameObject objToSpawn, int spawnCount)
    {
        spawnedObjects.Clear();

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomPosition = GetRandomPointInSphere(spawnArea);
            randomPosition.y = spawnArea.transform.position.y; // Ustawienie wysokoœci na wysokoœæ spawnera
            GameObject spawnedObj = Instantiate(objToSpawn, randomPosition, Quaternion.identity);
            spawnedObjects.Add(spawnedObj);
        }

        return spawnedObjects;
    }

    private Vector3 GetRandomPointInSphere(SphereCollider collider)
    {
        Vector3 randomPoint = Random.insideUnitSphere * collider.radius;
        randomPoint += collider.transform.position; // Adjust based on collider's position
        return randomPoint;
    }
}
