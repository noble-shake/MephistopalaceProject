using UnityEngine;
public class ItemObject : InteractObject
{
    [SerializeField] public ItemScriptableObject ItemInfo;
    [SerializeField] public int ItemNumb;
    [SerializeField] public int ItemID;
    [SerializeField] public string ItemName;
    [SerializeField] public EarnActionType actionType;

    protected override void Start()
    {
        base.Start();
        ItemInfo.NumbOfItem = ItemNumb;
        ItemID = ItemInfo.ItemID;
    }

    public override void InteractEvent()
    { 
        // Check Slot remained.
        // From Inventory Check.

        // Interact Message Queue + Event Subscribe.
        base.InteractEvent();

        PlayerCharacterManager.Instance.CurrentPlayer.animator.ItemEarnAnimation(this.transform, actionType);
    }

    public void GetItemInfo(ItemScriptableObject item, EarnActionType actionType)
    {
        ItemInfo = item;
        ItemNumb = item.NumbOfItem;
        ItemID = item.NumbOfItem;
        ItemName = item.Name.ToString();
        this.actionType = actionType;
    }

}