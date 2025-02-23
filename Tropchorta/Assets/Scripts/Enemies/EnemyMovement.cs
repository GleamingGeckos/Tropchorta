using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public void RotateTowards(GameObject obj) 
    {
        if (obj == null) return; // Safety check  

        Vector3 direction = (obj.transform.position - transform.position).normalized; // Get direction  
        Quaternion lookRotation = Quaternion.LookRotation(direction); // Calculate rotation  

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f); // Smooth rotation 
    }
}
