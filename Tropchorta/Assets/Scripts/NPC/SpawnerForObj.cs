using System.Collections.Generic;
using UnityEngine;

public class SpawnerForObj : MonoBehaviour
{
    [SerializeField] private Collider spawnArea;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    private Transform parent;

    private void Awake()
    {
        parent = GameObject.Find("Enemies").transform;
    }
    //Todo fix spawning hight
    public List<GameObject> SpawnObjects(GameObject objToSpawn, int spawnCount)
    {
        spawnedObjects.Clear();

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomPosition = transform.position;
            if (spawnArea is SphereCollider sphere)
            {
                randomPosition = GetRandomPointInSphere(sphere);
            }
            else if (spawnArea is BoxCollider box)
            {
                randomPosition = GetRandomPointInBox(box);
            }
            randomPosition.y = spawnArea.transform.position.y; // Ustawienie wysokoœci na wysokoœæ spawnera
            GameObject spawnedObj;
            if (parent)
                spawnedObj = Instantiate(objToSpawn, randomPosition, Quaternion.identity, parent);
            else
                spawnedObj = Instantiate(objToSpawn, randomPosition, Quaternion.identity);
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

    private Vector3 GetRandomPointInBox(BoxCollider collider)
    {
        Vector3 localPoint = new Vector3(
            Random.Range(-0.5f, 0.5f),
            Random.Range(-0.5f, 0.5f),
            Random.Range(-0.5f, 0.5f)
        );
        Vector3 worldPoint = collider.transform.TransformPoint(Vector3.Scale(localPoint, collider.size));
        return worldPoint;
    }
}
