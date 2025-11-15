using UnityEngine;
using UnityEngine.EventSystems;

public class SlotUI : SlotObject
{
    [SerializeField] public int slotID;
    [SerializeField] public int NumbOfItem;


    private void Start()
    {
        itemImage.raycastTarget = false;
        dragged = itemImage.GetComponent<Draggeditem>();
        if (isItemExist) Equip();
        
    }

    public override void Equip()
    {
        base.Equip();
        OnItemNumberChanged();
    }

    public override void UnEqup()
    {
        base.UnEqup();
        OnItemNumberChanged();
    }

    public void OnItemNumberChanged()
    {
        if (itemInfo == null)
        {
            numbText.text = "";
            return;
        }

        NumbOfItem = itemInfo.NumbOfItem;

        if (NumbOfItem > 1) 
        {
            numbText.text = NumbOfItem.ToString();
        }
        else
        {
            numbText.text = "";
        }
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
            numbText.transform.SetAsLastSibling();
        }

    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Item Description.
            if (!isItemExist) return;
            if (itemInfo == null) return;
            InventoryUI.Instance.OnPanel(true);
            InventoryUI.Instance.itemSprite = itemInfo.itemImage;
            InventoryUI.Instance.itemDescription = itemInfo.Description;
        }
        else
        {
            if (!isItemExist) return;
            if (itemInfo == null) return;
            if (itemInfo.itemType != ItemType.Consume) return;

            if (itemInfo.NumbOfItem <= 1) // 1개 남은걸 사용한다.
            {
                PlayerCharacterManager.Instance.CurrentPlayer.status.AdjustConsumeItem(itemInfo);
                isItemExist = false;
                UnEqup();
            }
            else
            {
                // 아이템 효과 적용.
                itemInfo.NumbOfItem--;
                PlayerCharacterManager.Instance.CurrentPlayer.status.AdjustConsumeItem(itemInfo);
                OnItemNumberChanged();
            }
        }
    }

    public void UseConsumeItem(PlayerManager _target)
    {
        if (!isItemExist) return;
        if (itemInfo == null) return;
        if (itemInfo.itemType != ItemType.Consume) return;

        if (itemInfo.NumbOfItem <= 1) // 1개 남은걸 사용한다.
        {
            _target.status.AdjustConsumeItem(itemInfo);
            isItemExist = false;
            UnEqup();
        }
        else
        {
            // 아이템 효과 적용.
            itemInfo.NumbOfItem--;
            _target.status.AdjustConsumeItem(itemInfo);
            OnItemNumberChanged();
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (isDragged)
        {
            isDragged = false;
            dragged.transform.position = transform.position;
            dragged.transform.SetParent(transform);
            dragged.transform.SetAsLastSibling();
            numbText.transform.SetAsLastSibling();
        }
        if (eventData.pointerDrag != null && eventData.pointerDrag.CompareTag("DropableItem"))
        {
            Debug.Log("Dropped object was: " + eventData.pointerDrag);
            SlotObject slot = eventData.pointerDrag.GetComponent<SlotObject>();
            if (!slot.isItemExist) return;

            if (!isItemExist)
            {
                if (eventData.pointerDrag.TryGetComponent<EquipSlot>(out EquipSlot eSlot))
                {
                    var item = eSlot.itemInfo;
                    this.itemInfo = item;
                    eSlot.UnEqup();
                    Equip();
                    PlayerCharacterManager.Instance.CurrentPlayer.status.UnEquip(item.itemType);
                    return;
                }
                else
                {
                    var item = slot.itemInfo;
                    this.itemInfo = item;
                    slot.UnEqup();
                    Equip();
                    isItemExist = true;
                    return;
                }


            }
            else
            {
                switch (itemInfo.itemType)
                {
                    // 가지고 있는 아이템의 타입이 이큅일 경우.
                    case ItemType.Equip_Head:
                    case ItemType.Equip_Weapon:
                    case ItemType.Equip_Armor:
                    case ItemType.Equip_Accessory:
                    case ItemType.Equip_Boots:
                        // 이큅 슬롯으로부터 아이템을 입력 받았다면
                        if (eventData.pointerDrag.TryGetComponent<EquipSlot>(out EquipSlot eSlot))
                        {
                            // 이큅 슬롯이 매칭 된다면,
                            if (eSlot.itemInfo.itemType == itemInfo.itemType)
                            {
                                // Equip Exchange
                                var item = slot.itemInfo;
                                slot.itemInfo = this.itemInfo;
                                this.itemInfo = item;
                                slot.Equip();
                                Equip();
                                PlayerCharacterManager.Instance.CurrentPlayer.status.Equip(slot.itemInfo);
                                return;
                            }
                            else
                            {
                                // Not Equip Slot Matched.
                                Debug.Log("Equip Type Not Matched");
                                return;
                            }
                        }
                        else // 일반 슬롯으로부터 입력 받은거라면..
                        {
                            var item = slot.itemInfo;
                            slot.itemInfo = this.itemInfo;
                            this.itemInfo = item;
                            slot.Equip();
                            Equip();
                            return;
                        }
                    default:
                        // 아이템 타입이 이큅이 아니라 걱정할 필요 없다면..
                        // 1. 같은 아이템일 경우,
                        if (itemInfo.ItemID == slot.itemInfo.ItemID)
                        {
                            if (itemInfo.isUnique)
                            {
                                // 바꿀 필요 없음.
                                return;
                            }
                            else
                            {
                                // 갯수를 증가 시키고, 들어온 친구를 해제한다.
                                var item = slot.itemInfo;
                                this.itemInfo.NumbOfItem += item.NumbOfItem;
                                slot.UnEqup();
                                Equip();
                                return;
                            }
                        }
                        else
                        {
                            if (eventData.pointerDrag.TryGetComponent<EquipSlot>(out EquipSlot eqSlot))
                            {
                                return;
                            }
                            else
                            {
                                var item = slot.itemInfo;
                                slot.itemInfo = this.itemInfo;
                                this.itemInfo = item;
                                slot.Equip();
                                Equip();
                                return;
                            }
                        }
                }
            }
        }
    }
}
