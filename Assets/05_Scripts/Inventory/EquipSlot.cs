using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : SlotObject
{
    [Space]
    [Header("Equip Header")]
    [SerializeField] public bool isLocked;
    [SerializeField] private ItemType equipType;

    public ItemType GetEquipType { get { return equipType; } set { equipType = value; } }

    private void Start()
    {
        itemImage.raycastTarget = false;
        dragged = itemImage.GetComponent<Draggeditem>();
        if (isItemExist) Equip();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (!isItemExist) return;
        if (itemInfo == null) return;
        isDragged = true;
        dragged.transform.SetParent(PauseCanvas.Instance.transform);
        dragged.transform.SetAsLastSibling();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (!isItemExist) return;
        Vector2 pointVector = InputManager.Instance.PointInput;

        if (isDragged)
        {
            dragged.transform.position = pointVector;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (isDragged)
        {
            isDragged = false;
            dragged.transform.position = transform.position;
            dragged.transform.SetParent(transform);
            dragged.transform.SetAsLastSibling();
        }

    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        // Item Description.
        if (!isItemExist) return;
        InventoryUI.Instance.OnPanel(true);
        InventoryUI.Instance.itemSprite = itemInfo.itemImage;
        InventoryUI.Instance.itemDescription = itemInfo.Description;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (isDragged)
        {
            isDragged = false;
            dragged.transform.position = transform.position;
            dragged.transform.SetParent(transform);
            dragged.transform.SetAsLastSibling();
        }
        if (eventData.pointerDrag != null && eventData.pointerDrag.CompareTag("DropableItem"))
        {
            Debug.Log("Dropped object was: " + eventData.pointerDrag);

            SlotObject slot = eventData.pointerDrag.GetComponent<SlotObject>();
            if (!slot.isItemExist) return;

            if (slot.itemInfo.itemType != equipType) return;

            // isItemExist == true => exchange each other.
            if (isItemExist)
            {
                var item = slot.itemInfo;
                slot.itemInfo = this.itemInfo;
                slot.Equip();
                Equip();
            }
            else
            {
                var item = slot.itemInfo;
                this.itemInfo = item;
                slot.UnEqup();
                Equip();
            }
            isItemExist = true;
            PlayerCharacterManager.Instance.CurrentPlayer.status.Equip(itemInfo);


            //EquipRegistry(eventData.pointerDrag.GetComponent<Item>());

            //DataManager.instance.EquipAdjust(eventData.pointerDrag.GetComponent<Item>());
        }
    }


}
