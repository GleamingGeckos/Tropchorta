using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] Item itemData;  // Reference to ScriptableObject

    private void Start()
    {
        if (itemData != null)
        {
            //Debug.Log($"Item Loaded: {itemData.itemName}");
            HandleItemData();
        }
    }

    private void HandleItemData()
    {
        if (itemData is Weapon weapon)
        {
            //Debug.Log($"Weapon Loaded: {weapon.itemName}");
            //weapon.Use(transform);
        }
        else
        {
           // Debug.Log($"Item: {itemData.itemName} - {itemData.description}");
        }
    }

    public Item PickUpItem()
    {
        Destroy(gameObject);
        return itemData;
    }

    public Item GetItem()
    {
        return itemData;
    }

    public void SetItem(Item item)
    {
        itemData = item;
    }
}
