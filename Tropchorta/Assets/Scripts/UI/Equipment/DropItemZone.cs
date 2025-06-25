using UnityEngine;
using UnityEngine.EventSystems;

public class DropItemZone : MonoBehaviour, IDropHandler
{
    [SerializeField] EquipmentController equipmentController;

    private void Awake()
    {
        if (equipmentController == null)
        {
            equipmentController = GameObject.FindGameObjectWithTag("Equipment").GetComponent<EquipmentController>();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            DragDropItem draggedItem = eventData.pointerDrag.GetComponent<DragDropItem>();

            if (draggedItem != null)
            {
                equipmentController.DropItem(draggedItem.GetSlotId());
            }
        }
    }
}
