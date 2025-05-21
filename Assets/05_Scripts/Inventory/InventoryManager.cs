using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [SerializeField] private List<EquipSlot> Equips;
    [SerializeField] private List<SlotUI> Slots;
    [SerializeField] private List<ItemScriptableObject> Items;

    public bool UseKeyItem(ItemScriptableObject _item)
    {
        foreach (SlotUI slot in Slots)
        {
            if (slot.itemInfo == null) continue;
            if (slot.itemInfo.ItemID == _item.ItemID)
            {
                EventMessageManager.Instance.MessageQueueRegistry(new EventContainer { eventType = ContextType.Inform, Context = $"{_item.Name} 을/를 사용 했습니다." });
                slot.UnEqup();
                return true;
            }
        }

        return false;
    }


    public bool EarnItem(ItemScriptableObject _item)
    {
        if (_item.isUnique)
        {
            // 빈 공간에 있으면 넣자.
            foreach (SlotUI slot in Slots)
            {
                if (!slot.isItemExist)
                {
                    slot.itemInfo = _item;
                    EventMessageManager.Instance.MessageQueueRegistry(new EventContainer { eventType = ContextType.Inform, Context = $"{_item.Name} 을/를 획득 했습니다." });
                    slot.Equip();
                    return true;
                }
            }

            EventMessageManager.Instance.MessageQueueRegistry(new EventContainer { eventType = ContextType.Inform, Context = "가방이 가득 찼습니다!!" });
            return false;
        }
        else if (_item.isConsume)
        {
            // 먼저 중첩될 수 있는지를 체크하고, 빈 공간에 넣자.
            foreach (SlotUI slot in Slots)
            {
                if (slot.itemInfo == null) continue;
                if (slot.itemInfo.ItemID == _item.ItemID)
                {
                    int itemNumb = _item.NumbOfItem;
                    slot.NumbOfItem += itemNumb;
                    slot.itemInfo.NumbOfItem = slot.NumbOfItem;
                    slot.OnItemNumberChanged();
                    slot.Equip();
                    EventMessageManager.Instance.MessageQueueRegistry(new EventContainer { eventType = ContextType.Inform, Context = $"{_item.Name} 을/를 {itemNumb}개 획득 했습니다." });
                    return true;
                }
            }

            // Empty 체크
            foreach (SlotUI slot in Slots)
            {
                if (!slot.isItemExist)
                {
                    slot.itemInfo = _item;
                    slot.Equip();
                    EventMessageManager.Instance.MessageQueueRegistry(new EventContainer { eventType = ContextType.Inform, Context = $"{_item.Name} 을/를 획득 했습니다." });
                    return true;
                }
            }
        }

        EventMessageManager.Instance.MessageQueueRegistry(new EventContainer { eventType = ContextType.Inform, Context = "가방이 가득 찼습니다!!" });
        return false;
    }

    public bool EarnItem(int _itemID)
    {
        foreach (ItemScriptableObject s in Items)
        {
            if(s.ItemID == _itemID) return EarnItem(s);
        }

        Debug.LogWarning($"EarnItem Function Cannot Be Matching Item ID :: {_itemID}");
        return false;
    }

    public bool EarnItem(string _itemName)
    {
        foreach (ItemScriptableObject s in Items)
        {
            if (s.Name.Equals(_itemName)) return EarnItem(s);
        }

        Debug.LogWarning($"EarnItem Function Cannot Be Matching Item Name :: {_itemName}");
        return false;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //for (int idx = 0; idx < Items.Count; idx++)
        //{
        //    Items[idx].ItemID = idx;
        //}
    }



    



}