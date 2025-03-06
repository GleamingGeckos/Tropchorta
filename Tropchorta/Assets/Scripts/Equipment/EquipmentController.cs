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
            //UseWeapon(transform);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            UseWeapon(transform);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("Picking up items!!!");
            PickUpItem();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Switching weapons!!!");
            SwitchWeapons();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("triggered equimpent controller");
        if (other.CompareTag("Gold"))
        {
            //Debug.Log("equimpent controller got some gold");
            AddGold(other.gameObject.GetComponent<GoldController>().OnPlayerActivate());
        }
        else if(other.CompareTag("Item"))
        {
            Item item = other.GetComponent<ItemController>().GetItem();
            interactedItems.Add(item);
            lastInteractedItem = item;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Item item = other.GetComponent<ItemController>().GetItem();
            interactedItems.Remove(item);

            if (lastInteractedItem == item)
            {
                lastInteractedItem = null;
                if(interactedItems.Count > 0)
                {
                    lastInteractedItem = interactedItems[interactedItems.Count - 1];
                }
            }
        }
    }

    public int GetGold()
    {
        return goldAmount;
    }

    public void AddGold(int amount)
    {
        goldAmount += amount;
    }

    public void RemoveGold(int amount)
    {
        goldAmount -= amount;
    }

    public void ClearGold()
    {
        goldAmount = 0;
    }

    public void UseWeapon(Transform playerTransform)
    {
        if (usedWeapon == null)
        {
            Debug.LogWarning("No weapon equipped.");
            return;
        }

        // Check if the equipped item is a Weapon
        if (usedWeapon is Weapon weapon)
        {
            Debug.Log($"Using weapon: {weapon.itemName}");
            weapon.Use(playerTransform);
        }
        else
        {
            Debug.LogWarning($"{usedWeapon.itemName} is not a weapon.");
        }
    }

    public void SwitchWeapons()
    {
        Item tmpItem = usedWeapon;
        usedWeapon = inactiveWeapon;
        inactiveWeapon = tmpItem;
    }

    public void PickUpItem()
    {
        if (lastInteractedItem is Weapon weapon)
        {
            Debug.Log($"Picking up weapon: {weapon.itemName}");

        }
    }

    public void SpawnItem(Item item, Vector3 position)
    {
        if (item.itemPrefab != null)
        {
            GameObject spawnedItem = Instantiate(item.itemPrefab, position, Quaternion.identity);
            ItemController controller = spawnedItem.GetComponent<ItemController>();
            controller.SetItem(item); // Assign the ScriptableObject data
        }
    }
}
