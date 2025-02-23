using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    [SerializeField] int goldAmount = 0;
    [SerializeField] Item lastInteractedItem;
    [SerializeField] List<Item> interactedItems;

    [SerializeField] Item usedWeapon;
    [SerializeField] Item inactiveWeapon;
    [SerializeField] Item helmet;
    [SerializeField] Item breastplate;
    [SerializeField] Item pants;
    [SerializeField] Item shoes;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Left Mouse Click
        {
            UseWeapon();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("triggered equimpent controller");
        if (other.CompareTag("Gold"))
        {
            //Debug.Log("equimpent controller got some gold");
            goldAmount += other.gameObject.GetComponent<GoldController>().OnPlayerActivate();
        }
    }

    public void SpawnItem(Item item, Vector3 position)
    {
        if (item.itemPrefab != null)
        {
            GameObject spawnedItem = Instantiate(item.itemPrefab, position, Quaternion.identity);
            ItemController controller = spawnedItem.GetComponent<ItemController>();
            controller.itemData = item;  // Assign the ScriptableObject data
        }
    }
    private void UseWeapon()
    {
        if (usedWeapon == null)
        {
            Debug.LogWarning("No weapon equipped.");
            return;
        }

        // Check if the equipped item is a Weapon
        if (usedWeapon is Weapon weapon)
        {
            //Debug.Log($"Using weapon: {weapon.itemName}");
            weapon.Use(transform);
        }
        else
        {
            //Debug.LogWarning($"{usedWeapon.itemName} is not a weapon.");
        }
    }

    public void SwitchWeapons()
    {
        Item tmpItem = usedWeapon;
        usedWeapon = inactiveWeapon;
        inactiveWeapon = tmpItem;
    }
}
