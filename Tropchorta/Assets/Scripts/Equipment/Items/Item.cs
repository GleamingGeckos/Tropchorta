using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 1)]
public class Item : ScriptableObject
{
    public string itemName = "New Item";
    public Sprite icon = null;
    public string description = "Item Description";
    public GameObject itemPrefab;  // The actual prefab to instantiate when dropped
    public int itemPrice = 100 ;
}
