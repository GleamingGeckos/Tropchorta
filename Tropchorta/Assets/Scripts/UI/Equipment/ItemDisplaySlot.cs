using UnityEngine;
using UnityEngine.EventSystems;
public class ItemDisplaySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] DragDropItem dragDropItem;
    [SerializeField] EquipmentBackpackUIController equipmentBackpackUIController;
    private int slotId;

    private void Awake()
    {
        slotId = dragDropItem.GetSlotId();;
    }
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log(slotId);
        if(eventData.pointerDrag != null)
        {
            equipmentBackpackUIController.SwitchItems(eventData.pointerDrag.GetComponent<DragDropItem>().GetSlotId(), slotId);
        }
    }

}
