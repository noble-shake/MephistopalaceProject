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
        RequireEXP = 10;
    }

    public override void HPChange(int _value)
    {
        //TODO: Status Àû¿ë

        ResourceManager.Instance.GetDamageUI(_value, transform.position + transform.forward * 1.5f + Vector3.up * 2f);
        HP += _value;
        
        if (HP >= aMaxHP) HP = aMaxHP;
        if (HP <= 0)
        {
            HP = 0;
            playerManager.isDead = true;
            playerManager.animator.animator.SetBool("isDead", true);
            playerManager.status.isDead = true;
        }

        if (playerStatUI != null) playerStatUI.SetHPValue((float)HP, (float)aMaxHP);
    }



    public void GainEXP(int _value)
    {
        EXP += _value;
        if (EXP >= RequireEXP)
        {
            EXP -= RequireEXP;
            // LevelUp
            Level++;
            playerStatUI.SetLevel(Level + 1);
            GrowUp();

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

        PauseCanvas.Instance.statPanel.SetLevel(Level);
        PauseCanvas.Instance.statPanel.SetHP(HP);
        PauseCanvas.Instance.statPanel.SetMaxHP(aMaxHP);
        PauseCanvas.Instance.statPanel.SetMinATK(aMinATK);
        PauseCanvas.Instance.statPanel.SetMaxATK(aMaxATK);
        PauseCanvas.Instance.statPanel.SetSPD(aSpeedWeight);
        PauseCanvas.Instance.statPanel.SetAP(AP);
        PauseCanvas.Instance.statPanel.SetCRT(aCriticalWeight);
        PauseCanvas.Instance.statPanel.SetDEF(aDefenceWeight);
        PauseCanvas.Instance.statPanel.SetEXP(EXP);
        PauseCanvas.Instance.statPanel.SetRequiredEXP(RequireEXP);
    }

    public void AdjustConsumeItem(ItemScriptableObject _consume)
    {
        StatContainer sc = _consume.UseConsumeItem();
        if(sc.HP > 0) HPChange(sc.HP);
        if (sc.AP > 0) GainAP(sc.AP);

        PauseCanvas.Instance.statPanel.SetHP(HP);
        PauseCanvas.Instance.statPanel.SetAP(AP);

        if (playerStatUI != null) playerStatUI.SetHPValue((float)HP, (float)aMaxHP);
        if (playerStatUI != null) playerStatUI.SetAPValue(AP);
    }

    public void GrowUp()
    {
        MaxHPChange = (MaxHP + Level * 15);
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
