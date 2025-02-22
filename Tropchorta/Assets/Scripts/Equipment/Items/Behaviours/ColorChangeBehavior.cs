using UnityEngine;

[CreateAssetMenu(fileName = "NewBehaviour", menuName = "Inventory/WeaponBehaviors/ColorChangeBehavior", order = 3)]
public class ColorChangeBehavior : WeaponBehavior
{
    public Color weaponColor = Color.red;

    public override void Use(Transform user)
    {
        Renderer weaponRenderer = user.GetComponentInChildren<Renderer>();
        if (weaponRenderer != null)
        {
            weaponRenderer.material.color = weaponColor;
            Debug.Log("Weapon color changed to " + weaponColor);
        }
    }
}
