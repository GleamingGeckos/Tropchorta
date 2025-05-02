using UnityEngine;

public class CluePopupController : MonoBehaviour
{
    [SerializeField] float visibilityTime = 2.0f;
    private float time = 0f;
    void Update()
    {
        time += Time.deltaTime; 
        if(time > visibilityTime)
        {
            time = 0;
            gameObject.SetActive(false);
        }
    }
}
