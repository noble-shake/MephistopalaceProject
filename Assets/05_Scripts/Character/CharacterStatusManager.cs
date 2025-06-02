using UnityEngine;
using System.Collections.Generic;
public class CharacterStatusManger : MonoBehaviour
{
    [Header("Ingame Status")]
    [SerializeField] public int HP;
    [SerializeField] public int AP;
    [SerializeField] public int Level;
    [SerializeField] public int aMaxHP;
    [SerializeField] public int aMinATK;
    [SerializeField] public int aMaxATK;
    [SerializeField] public int aCriticalWeight;
    [SerializeField] public int aSpeedWeight;
    [SerializeField] public int aDefenceWeight;
    [SerializeField] public int aEvadeWeight;

    [SerializeField] public bool isDead;

    [Header("Origin Status")]
    [SerializeField] public int MaxAP { get; private set; }
    [SerializeField] public int MaxHP { get; private set; }
    [SerializeField] public int MinATK { get; private set; }
    [SerializeField] public int MaxATK { get; private set; }
    [SerializeField] public int CriticalWeight { get; private set; }
    [SerializeField] public int SpeedWeight { get; private set; }
    [SerializeField] public int DefenceWeight { get; private set; }
    [SerializeField] public int EvadeWeight { get; private set; }
    public int MaxHPChange { set { MaxHP = value; } }
    public int MinATKChange { set { MinATK = value; } }
    public int MaxATKChange { set { MaxATK = value; } }
    public int CriticalChange { set { CriticalWeight = value; } }
    public int SpeedChange { set { SpeedWeight = value; } }
    public int DefenceChange { set { DefenceWeight = value; } }
    public int EvadeChange { set { EvadeWeight = value; } }

    public List<SkillScriptableObject> OwnSkillSet;

    public virtual void HPChange(int _value)
    {
        HP += _value;

        if (HP >= MaxHP) HP = MaxHP;
        if (HP <= 0)
        {
            HP = 0;
            Debug.Log("Dead");
        }
    }
    public virtual bool UseAP(int _value)
    {
        if (AP < _value) return false;
        AP -= _value;
        return true;
    }

    public virtual void GainAP(int _value)
    {
        AP += _value;
        if (AP >= MaxAP) AP = MaxAP;
    }

    public void StatInitialize(StatContainer _data)
    { 
        MaxHP = _data.HP;
        MaxAP = 6;
        HP = _data.HP;

        AP = _data.AP;
        DefenceWeight = _data.DEF;
        SpeedWeight = _data.SPD;
        CriticalWeight = _data.CRT;
        MinATK = _data.minATK;
        MaxATK = _data.maxATK;

        aMaxHP = MaxHP;
        aMinATK = MinATK;
        aMaxATK = MaxATK;
        aSpeedWeight = SpeedWeight;
        aDefenceWeight = DefenceWeight;
        aCriticalWeight = CriticalWeight;
    }

}