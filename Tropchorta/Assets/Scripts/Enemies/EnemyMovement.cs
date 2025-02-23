using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public void RotateTowards(GameObject obj) 
    {
        if (obj == null) return; // Safety check  

        
        Vector3 direction = (obj.transform.position - transform.position).normalized; // Get direction  
        Quaternion lookRotation = Quaternion.LookRotation(direction); // Calculate rotation  

        Quaternion newRotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f); // Smooth rotation 
        transform.rotation = Quaternion.Euler(0.0f, newRotation.eulerAngles.y, 0.0f);
    }
}
