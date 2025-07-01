using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 1)]
public class Item : ScriptableObject
{
    public string itemName = "New Item";
    public Sprite icon = null;
    public string description = "Item Description";
    public GameObject itemPrefab;  // The actual prefab to instantiate when dropped
    public int itemPrice = 100 ;
    public bool dropOnBuy = false; // Whether the item should be dropped on the ground when bought or send through equipment pipeline
    public Sprite shopPanel = null; // temporary solution to remove in later iterations
}
