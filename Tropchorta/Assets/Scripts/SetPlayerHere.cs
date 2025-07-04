using UnityEngine;

public class SetPlayerHere : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Debug.Log("Setting player position to: " + transform.position);
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false;
                player.transform.position = transform.position;
                cc.enabled = true;
            }

        }
    }
}
