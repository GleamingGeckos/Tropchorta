using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EquipmentController : MonoBehaviour
{
    [SerializeField] int goldAmount = 0;
    [SerializeField] GameObject lastInteractedItem;
    [SerializeField] List<GameObject> interactedItems = new List<GameObject>();
    [SerializeField] public InputReader input;
    [SerializeField] private TextMeshProUGUI goldText;

    [Header("Possessed Items")]
    [SerializeField] Item usedWeapon;
    [SerializeField] Item inactiveWeapon;
    [SerializeField] Item weaponCharm;
    [SerializeField] Item helmet;//
    [SerializeField] Item breastplate;
    [SerializeField] Item pants;//
    [SerializeField] Item shoes;//
    [SerializeField] List<ClueItem> clueItems;
    [SerializeField] List<Item> additionalItems; // items held in the inventory(different from the ones that user is currently using)


    private bool canSwitch = true;

    [Header("UI Controllers")]
    [SerializeField] EquipmentUIController equipmentUIController;
    [SerializeField] EquipmentBackpackUIController equipmentBackpackUIController;

    private void Start()
    {
        DisplayItems();
        ClearGold(); ////////////////////////////////////////////////////////// just in case youre looking for it :)
        input.OnScrollEvent += SwitchWeapons;
        UpdateGoldDisplay(goldAmount);
    }

    public void UpdateGoldDisplay(int goldAmount)
    {
        goldText.text = goldAmount.ToString();
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
                //weapon.Initialize(playerTransform);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //UseWeaponStart(transform);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("Picking up items!!!");
            PickUpItem();
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
        UpdateGoldDisplay(goldAmount);
    }

    public void RemoveGold(int amount)
    {
        goldAmount -= amount;
        UpdateGoldDisplay(goldAmount);
    }

    public void ClearGold()
    {
        goldAmount = 0;
    }

    public void UseWeaponSpecialAttack(Transform playerTransform)
    {
        if (usedWeapon == null)
        {
            Debug.LogWarning("No weapon equipped.");
            return;
        }

        if (usedWeapon is Weapon weapon)
        {
            weapon.UseSpecialAttack(playerTransform);
        }
        else
        {
            Debug.LogWarning($"{usedWeapon.itemName} is not a weapon.");
        }
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

    public bool IsDistance()
    {
        if (usedWeapon == null)
        {
            Debug.LogWarning("No weapon equipped.");
            return false;
        }

        // Check if the equipped item is a Weapon
        if (usedWeapon is Weapon weapon)
        {
            return weapon.IsDistance();
        }
        else
        {
            Debug.LogWarning($"{usedWeapon.itemName} is not a weapon.");
        }
        return false;
    }

    public void UseWeaponStrongStart(Transform playerTransform)
    {
        if (usedWeapon == null)
        {
            Debug.LogWarning("No weapon equipped.");
            return;
        }

        // Check if the equipped item is a Weapon
        if (usedWeapon is Weapon weapon)
        {
            weapon.UseStrongStart(playerTransform);
        }
        else
        {
            Debug.LogWarning($"{usedWeapon.itemName} is not a weapon.");
        }
    }

    public void UseWeaponStrongEnd(Transform playerTransform)
    {
        if (usedWeapon == null)
        {
            Debug.LogWarning("No weapon equipped.");
            return;
        }

        // Check if the equipped item is a Weapon
        if (usedWeapon is Weapon weapon)
        {
            weapon.UseStrongEnd(playerTransform);
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

    public void SwitchWeapons(float scroll)
    {
        if (canSwitch && scroll != 0f)
        {
            Item tmpItem = usedWeapon;
            usedWeapon = inactiveWeapon;
            UpdateCharmInUsedWeapon();
            inactiveWeapon = tmpItem;
            equipmentUIController.ChangeWeapon1Image(usedWeapon);
            equipmentUIController.ChangeWeapon2Image(inactiveWeapon);
            equipmentBackpackUIController.ChangeWeapon1Image(usedWeapon);
            equipmentBackpackUIController.ChangeWeapon2Image(inactiveWeapon);
            canSwitch = false;
        }
        else if (scroll == 0f)
        {
            canSwitch = true;
        }
    }

    public void PickUpItem()
    {
        if (lastInteractedItem == null)
            return;

        var itemController = lastInteractedItem.GetComponent<ItemController>();
        // if item is null, this is also null
        var item = itemController?.GetItem();

        int additionalSpace = IsAdditionalItemSpaceAvailable();

        switch (item)
        {
            case Weapon weapon:
                Debug.Log($"Picking up weapon: {weapon.itemName}");
                if (additionalSpace >= 0)
                {
                    additionalItems[additionalSpace] = item;
                    itemController.PickUpItem();
                    interactedItems.Remove(lastInteractedItem);
                    equipmentUIController.HideLastInteractedItemDisplay(); 
                }
                else
                {
                    DropWeapon();
                    interactedItems.Remove(lastInteractedItem);
                    usedWeapon = itemController.PickUpItem();
                    equipmentUIController.ChangeWeapon1Image(usedWeapon);
                    equipmentBackpackUIController.ChangeWeapon1Image(usedWeapon);
                    equipmentUIController.HideLastInteractedItemDisplay();
                }
                break;

            case Helmet helm:
                if (additionalSpace >= 0)
                {
                    additionalItems[additionalSpace] = item;
                    itemController.PickUpItem();
                    interactedItems.Remove(lastInteractedItem);
                    equipmentUIController.HideLastInteractedItemDisplay();
                }
                else
                {
                    DropHelmet();
                    Debug.Log($"Picking up helmet: {helm.itemName}");
                    interactedItems.Remove(lastInteractedItem);
                    helmet = itemController.PickUpItem();
                    equipmentUIController.ChangeHeadImage(helmet);
                    equipmentBackpackUIController.ChangeHeadImage(helmet);
                    equipmentUIController.HideLastInteractedItemDisplay();
                }
                break;

            case Breastplate breastp:
                Debug.Log($"Picking up breastplate: {breastp.itemName}");
                if (additionalSpace >= 0)
                {
                    additionalItems[additionalSpace] = item;
                    itemController.PickUpItem();
                    interactedItems.Remove(lastInteractedItem);
                    equipmentUIController.HideLastInteractedItemDisplay();
                }
                else
                {
                    DropBreastplate();
                    interactedItems.Remove(lastInteractedItem);
                    breastplate = itemController.PickUpItem();
                    equipmentUIController.ChangeTorsoImage(breastplate);
                    equipmentBackpackUIController.ChangeTorsoImage(breastplate);
                    equipmentUIController.HideLastInteractedItemDisplay();
                }
                break;

            case Pants pant:
                if (additionalSpace >= 0)
                {
                    additionalItems[additionalSpace] = item;
                    itemController.PickUpItem();
                    interactedItems.Remove(lastInteractedItem);
                    equipmentUIController.HideLastInteractedItemDisplay();
                }
                else
                {
                    DropPants();
                    Debug.Log($"Picking up pants: {pant.itemName}");
                    interactedItems.Remove(lastInteractedItem);
                    pants = itemController.PickUpItem();
                    equipmentUIController.ChangePantsImage(pants);
                    equipmentBackpackUIController.ChangePantsImage(pants);
                    equipmentUIController.HideLastInteractedItemDisplay();
                }
                break;

            case Shoes shoe:
                if (additionalSpace >= 0)
                {
                    additionalItems[additionalSpace] = item;
                    itemController.PickUpItem();
                    interactedItems.Remove(lastInteractedItem);
                    equipmentUIController.HideLastInteractedItemDisplay();
                }
                else
                {
                    DropShoes();
                    Debug.Log($"Picking up shoes: {shoe.itemName}");
                    interactedItems.Remove(lastInteractedItem);
                    shoes = itemController.PickUpItem();
                    equipmentUIController.ChangeShoesImage(shoes);
                    equipmentBackpackUIController.ChangeShoesImage(shoes);
                    equipmentUIController.HideLastInteractedItemDisplay();
                }
                break;

            case Charm charm:
                if (additionalSpace >= 0)
                {
                    additionalItems[additionalSpace] = item;
                    itemController.PickUpItem();
                    interactedItems.Remove(lastInteractedItem);
                    equipmentUIController.HideLastInteractedItemDisplay();
                }
                else
                {
                    DropCharm();
                    interactedItems.Remove(lastInteractedItem);
                    weaponCharm = itemController.PickUpItem();
                    UpdateCharmInUsedWeapon();

                    equipmentUIController.ChangeCharmImage(charm);
                    equipmentBackpackUIController.ChangeCharmImage(charm);
                    equipmentUIController.HideLastInteractedItemDisplay();
                }
                break;

            case ClueItem clueItem:
                Debug.Log($"Picking up shoes: {clueItem.itemName}");
                interactedItems.Remove(lastInteractedItem);
                clueItems.Add(itemController.PickUpItem() as ClueItem);
                equipmentUIController.HideLastInteractedItemDisplay();
                break;

            default:
                Debug.Log("Item type is not supported");
                break;
        }
        DisplayItems();
    }

    public Charm GetDefensiveCharm()
    {
        if (breastplate != null)
        {
            if (breastplate is Breastplate breastp)
            {
                return breastp.GetDefensiveCharm();
            }
        }
        return null;    
    }

    public void UseDefensiveItems(Transform playerTransform)
    {
        //if (helmet != null)
        //{
        //    if (helmet is Helmet helm)
        //    {
        //        helm.Use(playerTransform);
        //        Debug.Log($"Using helmet: {helm.itemName}");
        //    }
        //}
        //else
        //{
        //    Debug.LogWarning("No helmet equipped.");
        //}

        if (breastplate != null)
        {
            if (breastplate is Breastplate breastp)
            {
                breastp.Use(playerTransform);
                //Debug.Log($"Using helmet: {breastp.itemName}");
            }
        }
        else
        {
            Debug.LogWarning("No breastplate equipped.");
        }

        //if (pants != null)
        //{
        //    if (pants is Pants pant)
        //    {
        //        pant.Use(playerTransform);
        //        Debug.Log($"Using helmet: {pant.itemName}");
        //    }
        //}
        //else
        //{
        //    Debug.LogWarning("No pants equipped.");
        //}
        //
        //if (shoes != null)
        //{
        //    if (shoes is Shoes shoe)
        //    {
        //        shoe.Use(playerTransform);
        //        Debug.Log($"Using helmet: {shoe.itemName}");
        //    }
        //}
        //else
        //{
        //    Debug.LogWarning("No shoes equipped.");
        //}
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

    void DropCharm()
    {
        SpawnItem(weaponCharm, transform.position);
        weaponCharm = null;
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
        //equipmentUIController.ChangeHeadImage(helmet);
        //equipmentUIController.ChangePantsImage(pants);
        //equipmentUIController.ChangeShoesImage(shoes);
        equipmentUIController.ChangeTorsoImage(breastplate);
        equipmentUIController.ChangeWeapon1Image(usedWeapon);
        equipmentUIController.ChangeWeapon2Image(inactiveWeapon);
        equipmentUIController.ChangeCharmImage(weaponCharm);
        //equipmentBackpackUIController.ChangeHeadImage(helmet);
        //equipmentBackpackUIController.ChangePantsImage(pants);
        //equipmentBackpackUIController.ChangeShoesImage(shoes);
        equipmentBackpackUIController.ChangeTorsoImage(breastplate);
        equipmentBackpackUIController.ChangeWeapon1Image(usedWeapon);
        equipmentBackpackUIController.ChangeWeapon2Image(inactiveWeapon);
        equipmentBackpackUIController.ChangeBackpackImages(additionalItems);
        equipmentBackpackUIController.ChangeCharmImage(weaponCharm);
        UpdateCharmInUsedWeapon();
    }

    public bool HasClueItem(ClueItem clueItem)
    {
        if(clueItems.Contains(clueItem))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Item SwitchItem(Item item) // used in shop to switch items from shop
    {
        Item tmpItem;
        switch (item)
        {
            case Weapon weapon:
                tmpItem = usedWeapon;
                if (tmpItem == null)
                {
                    usedWeapon = item; 
                    UpdateCharmInUsedWeapon();
                    equipmentUIController.ChangeWeapon1Image(usedWeapon);
                    equipmentBackpackUIController.ChangeWeapon1Image(usedWeapon);
                    return null;
                }
                else
                {
                    int backpackEmptySlotId = IsAdditionalItemSpaceAvailable();
                    if(backpackEmptySlotId != -1)
                    {
                        additionalItems[backpackEmptySlotId] = item;
                        equipmentBackpackUIController.ChangeBackpackImages(additionalItems);
                    }
                    else
                    {
                        Instantiate(item.itemPrefab, transform.position + Vector3.up + transform.forward, Quaternion.identity);
                        Debug.Log($"No free space in backpack for weapon : {weapon.itemName} dropping on the ground");
                    }
                    UpdateCharmInUsedWeapon();
                    equipmentUIController.ChangeWeapon1Image(usedWeapon);
                    equipmentBackpackUIController.ChangeWeapon1Image(usedWeapon);
                    return null;
                }
                //Debug.Log($"Switching weapon: {weapon.itemName}");
               // UpdateCharmInUsedWeapon();
                //equipmentUIController.ChangeWeapon1Image(usedWeapon);
                //equipmentBackpackUIController.ChangeWeapon1Image(usedWeapon);
                return null;
            //return tmpItem;

            case Helmet helm:
                tmpItem = helmet;
                helmet = item;
                Debug.Log($"Switching helmet: {helm.itemName}");
                equipmentUIController.ChangeHeadImage(helmet);
                equipmentBackpackUIController.ChangeHeadImage(helmet);
                return tmpItem;

            case Breastplate breastp:
                tmpItem = breastplate;
                if (tmpItem == null)
                {
                    breastplate = item;
                    equipmentUIController.ChangeTorsoImage(breastplate);
                    equipmentBackpackUIController.ChangeTorsoImage(breastplate);
                    return null;
                }
                else
                {
                    int backpackEmptySlotId = IsAdditionalItemSpaceAvailable();
                    if (backpackEmptySlotId != -1)
                    {
                        additionalItems[backpackEmptySlotId] = item;
                        equipmentBackpackUIController.ChangeBackpackImages(additionalItems);
                    }
                    else
                    {
                        Instantiate(item.itemPrefab, transform.position + Vector3.up + transform.forward, Quaternion.identity);
                        Debug.Log($"No free space in backpack for breastplate : {breastplate.itemName} dropping on the ground");
                    }
                    equipmentUIController.ChangeTorsoImage(breastplate);
                    equipmentBackpackUIController.ChangeTorsoImage(breastplate);
                    return null;
                }
                //equipmentUIController.ChangeTorsoImage(breastplate);
                //equipmentBackpackUIController.ChangeTorsoImage(breastplate);
                return null;
            //return tmpItem;

            case Pants pant:
                tmpItem = pants;
                pants = item;
                Debug.Log($"Switching pants: {pant.itemName}");
                equipmentUIController.ChangePantsImage(pants);
                equipmentBackpackUIController.ChangePantsImage(pants);
                return tmpItem;

            case Shoes shoe:
                tmpItem = shoes;
                shoes = item;
                Debug.Log($"Switching shoes: {shoe.itemName}");
                equipmentUIController.ChangeShoesImage(shoes);
                equipmentBackpackUIController.ChangeShoesImage(shoes);
                return tmpItem;

            case Charm charm:
                tmpItem = charm;
                if (tmpItem == null)
                {
                    weaponCharm = item;
                    UpdateCharmInUsedWeapon();
                    equipmentUIController.ChangeCharmImage(weaponCharm);
                    equipmentBackpackUIController.ChangeCharmImage(weaponCharm);
                    return null;
                }
                else
                {
                    int backpackEmptySlotId = IsAdditionalItemSpaceAvailable();
                    if (backpackEmptySlotId != -1)
                    {
                        additionalItems[backpackEmptySlotId] = item;
                        equipmentBackpackUIController.ChangeBackpackImages(additionalItems);
                    }
                    else
                    {
                        Instantiate(item.itemPrefab, transform.position + Vector3.up + transform.forward, Quaternion.identity);
                        Debug.Log($"No free space in backpack for charm : {charm.itemName} dropping on the ground");
                    }
                    UpdateCharmInUsedWeapon();
                    equipmentUIController.ChangeCharmImage(weaponCharm);
                    equipmentBackpackUIController.ChangeCharmImage(weaponCharm);
                    return null;
                }
                //UpdateCharmInUsedWeapon();
                //equipmentUIController.ChangeCharmImage(weaponCharm);
                //equipmentBackpackUIController.ChangeCharmImage(weaponCharm);
                return null;
            //return tmpItem;

            case ClueItem clueItem:
                Debug.Log($"Adding Clue Item: {clueItem.itemName}");
                clueItems.Add(item as ClueItem);
                return null;

            default:
                Debug.Log("Item type is not supported");
                return null;
        }
    }

    void UpdateCharmInUsedWeapon()
    {
        if (usedWeapon is Weapon used)
        {
            used.ClearData(transform.parent);
            used.charm = (Charm)weaponCharm;
            used.Initialize(transform.parent);
        }
    }

    Item GetEquippedItem(int index)
    {
        return index switch
        {
            0 => usedWeapon,
            1 => inactiveWeapon,
            2 => helmet,
            3 => breastplate,
            4 => pants,
            5 => shoes,
            6 => weaponCharm,
            _ => null
        };
    }

    void SetEquippedItem(int index, Item item)
    {
        switch (index)
        {
            case 0: usedWeapon = item; break;
            case 1: inactiveWeapon = item; break;
            case 2: helmet = item; break;
            case 3: breastplate = item; break;
            case 4: pants = item; break;
            case 5: shoes = item; break;
            case 6: weaponCharm = item;
                UpdateCharmInUsedWeapon();
                break;
        }
        UpdateCharmInUsedWeapon();
    }

    bool IsItemValidForSlot(int index, Item item)
    {
        if (item == null) return true; // Allow setting null (empty) into any slot
        return index switch
        {
            0 or 1 => item is Weapon,
            2 => item is Helmet,
            3 => item is Breastplate,
            4 => item is Pants,
            5 => item is Shoes,
            6 => item is Charm,
            _ => false
        };
    }

    public void SwitchAdditionalItem(int firstIndex,int secondIndex) 
    /*
     * first index is of the dragged item in the display
     * switches addition item either with one used by player or one within backpack itself
     * backpack items indexes start from 100 and go on from there
     * 0 - weapon 1, 1 - weapon 2, 2 - helmet, 3 - breastplate, 4 - pants, 5 - shoes, 6 - charm
     */
    {
        bool isFirstEquipped = firstIndex < 100;
        bool isSecondEquipped = secondIndex < 100;

        if (isFirstEquipped && isSecondEquipped)
        {
            // Equipped ↔ Equipped
            Item item1 = GetEquippedItem(firstIndex);
            Item item2 = GetEquippedItem(secondIndex);

            if (IsItemValidForSlot(firstIndex, item2) && IsItemValidForSlot(secondIndex, item1))
            {
                SetEquippedItem(firstIndex, item2);
                SetEquippedItem(secondIndex, item1);
            }
            else
            {
                Debug.LogWarning("Item type mismatch in equipped ↔ equipped switch.");
            }
        }
        else if (!isFirstEquipped && !isSecondEquipped)
        {
            // Backpack ↔ Backpack
            Item tmp = additionalItems[firstIndex - 100];
            additionalItems[firstIndex - 100] = additionalItems[secondIndex - 100];
            additionalItems[secondIndex - 100] = tmp;
        }
        else
        {
            // Equipped ↔ Backpack
            int equippedIndex = isFirstEquipped ? firstIndex : secondIndex;
            int backpackIndex = isFirstEquipped ? secondIndex - 100 : firstIndex - 100;

            Item equippedItem = GetEquippedItem(equippedIndex);
            Item backpackItem = (backpackIndex < additionalItems.Count) ? additionalItems[backpackIndex] : null;

            if (IsItemValidForSlot(equippedIndex, backpackItem))
            {
                // Make sure the backpack has room for the item we're placing back
                if (backpackIndex >= additionalItems.Count)
                {
                    // Extend the list to accommodate index
                    while (additionalItems.Count <= backpackIndex)
                    {
                        additionalItems.Add(null);
                    }
                }

                SetEquippedItem(equippedIndex, backpackItem);
                additionalItems[backpackIndex] = equippedItem;
                UpdateCharmInUsedWeapon();
            }
            else
            {
                Debug.LogWarning($"Cannot place item '{backpackItem?.itemName}' into equipped slot {equippedIndex}: incompatible type.");
            }
        }
        DisplayItems();
    }

    int IsAdditionalItemSpaceAvailable()
    {
        for (int i = 0; i < additionalItems.Count; i++)
        {
            if (additionalItems[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public void DropItem(int slotId)
    {
        if(slotId < 100)
        {
            Item itemToDrop = GetEquippedItem(slotId);
            if (itemToDrop != null)
            {
                SpawnItem(itemToDrop, transform.position);
                SetEquippedItem(slotId, null);
            }
            else
            {
                Debug.LogWarning("No item equipped in this slot.");
            }
        }
        else
        {
            int additionalSlotId = slotId - 100;  // Adjust the slotId for additional items
            if (additionalItems[additionalSlotId] != null)
            {
                Item itemToDrop = additionalItems[additionalSlotId];
                SpawnItem(itemToDrop, transform.position);
                additionalItems[additionalSlotId] = null;
            }
            else
            {
                Debug.LogWarning("This slot in the backpack is empty: " + additionalSlotId);
            }
        }
        DisplayItems();
    }

}
