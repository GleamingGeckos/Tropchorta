using UnityEngine;

public class GoldController : MonoBehaviour
{
    public int goldAmount;
    public bool isUsed = false;//in case we want animations or something later
    public int OnPlayerActivate()
    {
        if(!isUsed)
        {
            isUsed = true; 
            Destroy(gameObject);
            return goldAmount;
        }
        else
        {
            Destroy(gameObject);
            return 0;
        }
    }
}
