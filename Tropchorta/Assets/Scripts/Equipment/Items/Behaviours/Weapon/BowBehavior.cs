using UnityEngine;

[CreateAssetMenu(fileName = "BowBehavior", menuName = "Inventory/WeaponBehaviors/Bow", order = 2)]
public class BowBehavior : WeaponBehavior
{
    public GameObject arrowPrefab;
    public float arrowSpeed = 20f;

    public override void UseStart(Transform user)
    {
        Debug.Log("Shooting an arrow!");
        GameObject arrow = Instantiate(arrowPrefab, user.position + user.forward, Quaternion.identity);
        arrow.GetComponent<Rigidbody>().linearVelocity = user.forward * arrowSpeed;
    }

    public override void UseStop(Transform user)
    {

    }

    public override void AltUseStart(Transform user)
    {

    }

    public override void AltUseStop(Transform user)
    {

    }

    public override void ClearData(Transform user)
    {
    }

    public override void Initialize(Transform user)
    {
    }
}
