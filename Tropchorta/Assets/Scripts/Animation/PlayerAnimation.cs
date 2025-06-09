using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private PlayerCombat playerCombat;
    void Attack()
    {
        playerCombat.Attack();
    }
}
