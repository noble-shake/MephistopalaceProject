using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusManager : CharacterStatusManger
{
    private PlayerManager playerManager;
    public PlayerStatUI playerStatUI;


    [SerializeField] public int EXP;
    [SerializeField] public int RequireEXP;

    [Space]
    [Header("Equipments")]
    [SerializeField] Dictionary<ItemType, ItemScriptableObject> Equips;
    
    public Dictionary<ItemType, ItemScriptableObject> GetEquips { get { return Equips; } }

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        Equips = new Dictionary<ItemType, ItemScriptableObject>();
        AdjustInGameStat();

        if(playerStatUI != null) playerStatUI.SetHPValue((float)HP, (float)aMaxHP);
        RequireEXP = 30;
    }

    public override void HPChange(int _value)
    {
        HP += _value;

        if (HP >= MaxHP) HP = aMaxHP;
        if (HP <= 0)
        {
            HP = 0;
            Debug.Log("Dead");
        }

        if (playerStatUI != null) playerStatUI.SetHPValue((float)HP, (float)aMaxHP);
    }



    public void GainEXP(int _value)
    {
        EXP += _value;
        if (EXP > RequireEXP)
        {
            // LevelUp
            EXP = EXP + _value - RequireEXP;
            Level++;
            playerStatUI.SetLevel(Level + 1);
        }
    }

    public void Equip(ItemScriptableObject _item)
    {
        Equips[_item.itemType] = _item;
        AdjustInGameStat();
    }

    public void UnEquip(ItemType _type)
    {
        if (Equips.ContainsKey(_type))
        {
            Equips[_type] = null;
            AdjustInGameStat();
        }

    }

    public void AdjustInGameStat()
    {
        //[SerializeField] public int HP;
        //[SerializeField] public int aMaxHP;
        //[SerializeField] public int aMinATK;
        //[SerializeField] public int aMaxATK;
        //[SerializeField] public int aCriticalWeight;
        //[SerializeField] public int aSpeedWeight;
        //[SerializeField] public int aDefenceWeight;
        //[SerializeField] public int aEvadeWeight;

        aMaxHP = MaxHP;
        aMinATK = MinATK;
        aMaxATK = MaxATK;
        aSpeedWeight = SpeedWeight;
        aDefenceWeight = DefenceWeight;
        aCriticalWeight = CriticalWeight;
        foreach (ItemScriptableObject o in Equips.Values)
        {
            if (o == null) continue;
            StatContainer sc = o.GetStatChange();
            aMaxHP += sc.HP;
            aMinATK += sc.minATK;
            aMaxATK += sc.maxATK;
            aSpeedWeight += sc.SPD;
            aDefenceWeight += sc.DEF;
            aCriticalWeight += sc.CRT;
        }

        if (playerStatUI != null) playerStatUI.SetHPValue((float)HP, (float)aMaxHP);
    }

    public void GrowUp()
    {
        MaxHPChange = (MaxHP * Level * 15);
        HP = MaxHP;

        MinATKChange = MinATK + Level;
        MaxATKChange = MaxATK + Level;

        DefenceChange = DefenceWeight * (int)Mathf.Floor(Level * 0.5f);

        RequireEXP += (int)(RequireEXP * 0.1f);
        AdjustInGameStat();

    }

    public override bool UseAP(int _value)
    {
        if (AP < _value) return false;
        AP -= _value;
        playerStatUI.SetAPValue(AP);
        return true;
    }

    public override void GainAP(int _value)
    {
        AP += _value;
        if (AP >= MaxAP) AP = MaxAP;
        playerStatUI.SetAPValue(AP);
    }
}
