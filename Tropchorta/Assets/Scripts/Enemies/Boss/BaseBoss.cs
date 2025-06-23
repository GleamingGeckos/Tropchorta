using UnityEngine;

public class BaseBoss : BaseEnemy
{
    // Update is called once per frame
    override protected void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && _enemyMovement != null && !_enemyMovement.isStuned)
        {
            BossCombat bossCombat = (BossCombat)_enemyCombat;
            GameObject player = other.gameObject;
            float distanceSqr = (player.transform.position - transform.position).sqrMagnitude;

            int value = Random.Range(1, 4);
            if (value == 1 && distanceSqr > 6.0f) // Plucie
            {
                Debug.Log("Plucie attack");
                bossCombat.DistanceAttack(other.transform);
            }
            else if (value == 2)
            {
                Debug.Log("Punch attack");
                bossCombat.PunchAttack(other.transform);
            }
            else
            {
                Debug.Log("Jump attack");
                bossCombat.JumpAttack(other.transform);
            }
        }
    }
}
