using UnityEngine;

public class WorldSpaceBillboard : MonoBehaviour
{
    Transform cam;
    private void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = cam.forward;    
    }
}
