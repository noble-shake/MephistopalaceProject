using UnityEngine;

[System.Serializable]
public struct StatContainer
{
    public int MinLevel;
    public int MaxLevel;
    public int HP;
    public int AP;
    public int minATK;
    public int maxATK;
    public int CRT;
    public int SPD;
    public int DEF;
}

[CreateAssetMenu(fileName = "ItemSO", menuName ="PalaceItem/ItemScriptableObject", order =-1)]
public class ItemScriptableObject : ScriptableObject
{
    [SerializeField] public int ItemID;
    [SerializeField] public int NumbOfItem;
    [SerializeField] public bool isUnique;
    [SerializeField] public bool isEquip;
    [SerializeField] public bool isConsume;
    [SerializeField] public ItemNames Name;
    [SerializeField] public string Description;
    [SerializeField] public ItemType itemType;
    [SerializeField] public Sprite itemImage;

    [SerializeField] public int LEVEL;
    [SerializeField] public int HP;
    [SerializeField] public int AP;
    [SerializeField] public int minATK;
    [SerializeField] public int maxATK;
    [SerializeField] public int CRT;
    [SerializeField] public int SPD;
    [SerializeField] public int DEF;
    [SerializeField] public int EXP;

    public StatContainer GetStatChange()
    {
        return new StatContainer { HP = HP, AP = AP, minATK = minATK, maxATK = maxATK, CRT = CRT, SPD = SPD, DEF = DEF };
    }
}