using System.Collections.Generic;
using UnityEngine;

public class TreeVisibilityManager : MonoBehaviour
{
    public Terrain terrain;
    public Transform player;

    [Header("Prefab drzewa przezroczystego")]
    public GameObject transparentTreePrefab;

    [Header("Parametry widocznoœci")]
    public float xTolerance = 2f;
    public float zMaxDistance = 30f;

    private List<TreeInstance> originalTrees = new List<TreeInstance>();
    private Dictionary<Vector3, GameObject> activeFakeTrees = new Dictionary<Vector3, GameObject>();

    void Start()
    {
        // Kopiujemy oryginalne drzewa do lokalnej listy
        foreach (TreeInstance tree in terrain.terrainData.treeInstances)
        {
            originalTrees.Add(tree);
        }
    }

    void Update()
    {
        UpdateTreeVisibility();
    }

    void UpdateTreeVisibility()
    {
        List<TreeInstance> currentTrees = new List<TreeInstance>(terrain.terrainData.treeInstances);
        List<TreeInstance> modifiedTrees = new List<TreeInstance>();

        HashSet<Vector3> treesToShowFake = new HashSet<Vector3>();

        foreach (TreeInstance tree in originalTrees)
        {
            Vector3 worldPos = Vector3.Scale(tree.position, terrain.terrainData.size) + terrain.transform.position;
            float dx = Mathf.Abs(worldPos.x - player.position.x);
            float dz = player.position.z - worldPos.z;

            if (dx <= xTolerance && dz > 0 && dz <= zMaxDistance)
            {
                // Zas³ania gracza – chowamy
                treesToShowFake.Add(worldPos);

                if (!activeFakeTrees.ContainsKey(worldPos))
                {
                    // Dodaj przezroczysty odpowiednik
                    GameObject fake = Instantiate(transparentTreePrefab, worldPos, Quaternion.identity);
                    fake.transform.localScale = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale);
                    activeFakeTrees[worldPos] = fake;
                }

                // Pomijamy dodanie tego drzewa do widocznych
                continue;
            }

            // Dodaj do zachowanych drzew (czyli widocznych w terrain)
            modifiedTrees.Add(tree);
        }

        // Aktualizujemy drzewa w terrain
        terrain.terrainData.treeInstances = modifiedTrees.ToArray();

        // Usuwamy niepotrzebne przezroczyste drzewa
        List<Vector3> toRemove = new List<Vector3>();
        foreach (var kvp in activeFakeTrees)
        {
            if (!treesToShowFake.Contains(kvp.Key))
            {
                Destroy(kvp.Value);
                toRemove.Add(kvp.Key);
            }
        }

        foreach (var pos in toRemove)
        {
            activeFakeTrees.Remove(pos);
        }
    }
}
