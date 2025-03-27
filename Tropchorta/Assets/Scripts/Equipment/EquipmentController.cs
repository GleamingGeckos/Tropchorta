using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    [SerializeField] int goldAmount = 0;
    [SerializeField] GameObject lastInteractedItem;
    [SerializeField] List<GameObject> interactedItems = new List<GameObject>();

    [SerializeField] Item usedWeapon;
    [SerializeField] Item inactiveWeapon;
    [SerializeField] Item helmet;
    [SerializeField] Item breastplate;
    [SerializeField] Item pants;
    [SerializeField] Item shoes;

    [SerializeField] EquipmentUIController equipmentUIController;
    private void Start()
    {
        DisplayItems();
    }
    public void Initialize(Transform playerTransform)
    {
        if (usedWeapon != null)
        {
            if (usedWeapon is Weapon weapon)
            {
                weapon.Initialize(playerTransform);
            }
        }
        if (inactiveWeapon != null)
        {
            if (inactiveWeapon is Weapon weapon)
            {
                weapon.Initialize(playerTransform);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            UseWeaponStart(transform);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            //Debug.Log("Picking up items!!!");
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
        else if (other.CompareTag("Item"))
        {
            //Item item = other.GetComponent<ItemController>().GetItem();
            interactedItems.Add(other.gameObject);
            lastInteractedItem = other.gameObject;
            equipmentUIController.ChangeLastInteractedItemDisplay(lastInteractedItem.GetComponent<ItemController>().GetItem());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            //Item item = other.GetComponent<ItemController>().GetItem();
            interactedItems.Remove(other.gameObject);

            if (lastInteractedItem == other.gameObject)
            {
                lastInteractedItem = null;
                equipmentUIController.HideLastInteractedItemDisplay();
                if (interactedItems.Count > 0)
                {
                    lastInteractedItem = interactedItems[interactedItems.Count - 1];
                    equipmentUIController.ChangeLastInteractedItemDisplay(lastInteractedItem.GetComponent<ItemController>().GetItem());
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

    public void UseWeaponStart(Transform playerTransform)
    {
        if (usedWeapon == null)
        {
            Debug.LogWarning("No weapon equipped.");
            return;
        }

        // Check if the equipped item is a Weapon
        if (usedWeapon is Weapon weapon)
        {
            weapon.UseStart(playerTransform);
        }
        else
        {
            Debug.LogWarning($"{usedWeapon.itemName} is not a weapon.");
        }
    }

    public void UseWeaponEnd(Transform playerTransform)
    {
        if (usedWeapon == null)
        {
            Debug.LogWarning("No weapon equipped.");
            return;
        }

        // Check if the equipped item is a Weapon
        if (usedWeapon is Weapon weapon)
        {
            weapon.UseEnd(playerTransform);
        }
        else
        {
            Debug.LogWarning($"{usedWeapon.itemName} is not a weapon.");
        }
    }

    public void UseWeaponAltStart(Transform playerTransform)
    {
        if (usedWeapon == null)
        {
            Debug.LogWarning("No weapon equipped.");
            return;
        }

        // Check if the equipped item is a Weapon
        if (usedWeapon is Weapon weapon)
        {
            weapon.AltUseStart(playerTransform);
        }
        else
        {
            Debug.LogWarning($"{usedWeapon.itemName} is not a weapon.");
        }
    }

    public void UseWeaponAltEnd(Transform playerTransform)
    {
        if (usedWeapon == null)
        {
            Debug.LogWarning("No weapon equipped.");
            return;
        }

        // Check if the equipped item is a Weapon
        if (usedWeapon is Weapon weapon)
        {
            weapon.AltUseEnd(playerTransform);
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
        equipmentUIController.ChangeWeapon1Image(usedWeapon);
        equipmentUIController.ChangeWeapon2Image(inactiveWeapon);
    }

    public void PickUpItem()
    {
        if (lastInteractedItem == null)
            return;

        var itemController = lastInteractedItem.GetComponent<ItemController>();
        // if item is null, this is also null
        var item = itemController?.GetItem();

        switch (item)
        {
            case Weapon weapon:
                DropWeapon();
                Debug.Log($"Picking up weapon: {weapon.itemName}");
                interactedItems.Remove(lastInteractedItem);
                usedWeapon = itemController.PickUpItem();
                equipmentUIController.ChangeWeapon1Image(usedWeapon);
                equipmentUIController.HideLastInteractedItemDisplay();
                break;

            case Helmet helm:
                DropHelmet();
                Debug.Log($"Picking up helmet: {helm.itemName}");
                interactedItems.Remove(lastInteractedItem);
                helmet = itemController.PickUpItem();
                equipmentUIController.ChangeHeadImage(helmet);
                equipmentUIController.HideLastInteractedItemDisplay();
                break;

            case Breastplate breastp:
                DropBreastplate();
                Debug.Log($"Picking up breastplate: {breastp.itemName}");
                interactedItems.Remove(lastInteractedItem);
                breastplate = itemController.PickUpItem();
                equipmentUIController.ChangeTorsoImage(breastplate);
                equipmentUIController.HideLastInteractedItemDisplay();
                break;

            case Pants pant:
                DropPants();
                Debug.Log($"Picking up pants: {pant.itemName}");
                interactedItems.Remove(lastInteractedItem);
                pants = itemController.PickUpItem();
                equipmentUIController.ChangePantsImage(pants);
                equipmentUIController.HideLastInteractedItemDisplay();
                break;

            case Shoes shoe:
                DropShoes();
                Debug.Log($"Picking up shoes: {shoe.itemName}");
                interactedItems.Remove(lastInteractedItem);
                shoes = itemController.PickUpItem();
                equipmentUIController.ChangeShoesImage(shoes);
                equipmentUIController.HideLastInteractedItemDisplay();
                break;

            default:
                Debug.Log("Item type is not supported");
                break;
        }
    }

    public void UseDefensiveItems(Transform playerTransform)
    {
        if (helmet != null)
        {
            if (helmet is Helmet helm)
            {
                helm.Use(playerTransform);
                Debug.Log($"Using helmet: {helm.itemName}");
            }
        }
        else
        {
            Debug.LogWarning("No helmet equipped.");
        }

        if (breastplate != null)
        {
            if (breastplate is Breastplate breastp)
            {
                breastp.Use(playerTransform);
                Debug.Log($"Using helmet: {breastp.itemName}");
            }
        }
        else
        {
            Debug.LogWarning("No breastplate equipped.");
        }

        if (pants != null)
        {
            if (pants is Pants pant)
            {
                pant.Use(playerTransform);
                Debug.Log($"Using helmet: {pant.itemName}");
            }
        }
        else
        {
            Debug.LogWarning("No pants equipped.");
        }

        if (shoes != null)
        {
            if (shoes is Shoes shoe)
            {
                shoe.Use(playerTransform);
                Debug.Log($"Using helmet: {shoe.itemName}");
            }
        }
        else
        {
            Debug.LogWarning("No shoes equipped.");
        }
    }
    void DropWeapon()
    {
        SpawnItem(usedWeapon, transform.position);
        usedWeapon = null;
    }
    void DropHelmet()
    {
        SpawnItem(helmet, transform.position);
        helmet = null;
    }

    void DropBreastplate()
    {
        SpawnItem(breastplate, transform.position);
        breastplate = null;
    }

    void DropPants()
    {
        SpawnItem(pants, transform.position);
        pants = null;
    }

    void DropShoes()
    {
        SpawnItem(shoes, transform.position);
        shoes = null;
    }
    public void SpawnItem(Item item, Vector3 position)
    {
        if (item != null)
        {
            if (item.itemPrefab != null)
            {
                GameObject spawnedItem = Instantiate(item.itemPrefab, position, Quaternion.identity);
                ItemController controller = spawnedItem.GetComponent<ItemController>();
                controller.SetItem(item); // Assign the ScriptableObject data
            }
        }
        else
        {
            Debug.Log("Item to drop is null");
        }
    }

    void DisplayItems()
    {
        equipmentUIController.ChangeHeadImage(helmet);
        equipmentUIController.ChangeTorsoImage(breastplate);
        equipmentUIController.ChangePantsImage(pants);
        equipmentUIController.ChangeShoesImage(shoes);
        equipmentUIController.ChangeWeapon1Image(usedWeapon);
        equipmentUIController.ChangeWeapon2Image(inactiveWeapon);
    }
}
