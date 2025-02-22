using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    public int goldAmount = 0;
    public Item lastInteractedItem;
    public List<Item> interactedItems;

    void Start()
    {
        
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
}
