using UnityEngine;
using static UnityEngine.GraphicsBuffer;
[CreateAssetMenu(fileName = "BowBehavior", menuName = "Inventory/WeaponBehaviors/Bow", order = 2)]
public class BowBehavior : WeaponBehavior
{
    public GameObject arrowPrefab; 
    private PlayerCombat playerCombat;
    [SerializeField] private int damage = 1;

    public override void Initialize(Transform user)
    {
        // Debug.Log("Setting player in SwordBehavior");
        playerCombat = user.GetComponent<PlayerCombat>();

    }

    public override void UseStart(Transform user, Charm charm)
    {
        GameObject arrow = Instantiate(arrowPrefab, user.position + user.forward, user.rotation);
        if(charm)
            arrow.GetComponent<Projectile>().Initialize(null, charm.GetCharmType(), damage, user.gameObject);
        else
            arrow.GetComponent<Projectile>().Initialize(null,CharmType.None, damage, user.gameObject);
    }

    public override void UseStop(Transform user)
    {

    }

    public override void UseStrongStart(Transform user, Charm charm)
    {
        float spreadAngle = 5f; // k�t mi�dzy strza�ami
        int arrowCount = 3;
        int middle = arrowCount / 2;

        for (int i = 0; i < arrowCount; i++)
        {
            float angleOffset = (i - middle) * spreadAngle;
            Quaternion rotation = user.rotation * Quaternion.Euler(0, angleOffset, 0);
            Vector3 spawnPos = user.position + rotation * Vector3.forward;
            GameObject arrow = Instantiate(arrowPrefab, spawnPos, rotation);
            if (charm)
                arrow.GetComponent<Projectile>().Initialize(null, charm.GetCharmType(), damage * 2, user.gameObject);
            else
                arrow.GetComponent<Projectile>().Initialize(null, CharmType.None, damage * 2, user.gameObject);
        }
    }

    public override void UseStrongStop(Transform user)
    {

    }

    public override void AltUseStart(Transform user, Charm charm)
    {
        playerCombat.StartBlocking();
    }

    public override void AltUseStop(Transform user)
    {
        playerCombat.StopBlocking();
    }

    public override void ClearData(Transform user)
    {
    }

    public override void UseSpecialAttack(Transform user, Charm charm)
    {
        float spreadAngle = 15f; // k�t mi�dzy strza�ami
        int arrowCount = 5;
        int middle = arrowCount / 2;

        for (int i = 0; i < arrowCount; i++)
        {
            float angleOffset = (i - middle) * spreadAngle;
            Quaternion rotation = user.rotation * Quaternion.Euler(0, angleOffset, 0);
            Vector3 spawnPos = user.position + rotation * Vector3.forward;
            GameObject arrow = Instantiate(arrowPrefab, spawnPos, rotation);
            if (charm)
                arrow.GetComponent<Projectile>().Initialize(null, charm.GetCharmType(), damage * 3, user.gameObject);
            else
                arrow.GetComponent<Projectile>().Initialize(null, CharmType.None, damage * 3, user.gameObject);
        }
    }

    public override bool IsDistance()
    {
        return true;
    }

    public override void PlayPerfectBlockSound()
    {
    }

    public override void PlayNormalBlockSound()
    {
    }
}
