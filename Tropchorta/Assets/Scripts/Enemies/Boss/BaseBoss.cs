using UnityEngine;

public class BaseBoss : BaseEnemy
{
    // Update is called once per frame
    override protected void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && _enemyMovement != null && !_enemyMovement.isStuned)
        {
            GameObject player = other.gameObject;
            float distanceSqr = (player.transform.position - transform.position).sqrMagnitude;

            int value = Random.Range(1, 2);
            if(value == 1 && distanceSqr > 6.0f) // Plucie
            {
                _enemyCombat.DistanceAttack(other.transform);
            }
            _enemyMovement.RotateTowards(player);
        }
    }
}
