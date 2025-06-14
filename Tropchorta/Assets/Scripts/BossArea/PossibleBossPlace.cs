using UnityEngine;

public class PossibleBossPlace : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(GetComponent<Renderer>());
        Destroy(GetComponent<MeshFilter>());
    }
}
