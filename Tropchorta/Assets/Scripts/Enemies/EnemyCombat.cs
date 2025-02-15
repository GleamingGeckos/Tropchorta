using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] int _maxDamage;//inclusive
    [SerializeField] int _minDamage;//inclusive

    public int Attack()
    {
        int damage = Random.Range(_minDamage, _maxDamage + 1);
        //Debug.Log($"Attack dealt {damage} damage.");
        return damage;
    }
}
