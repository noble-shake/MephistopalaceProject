using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatPanel : MonoBehaviour
{
    [SerializeField] public List<EquipSlot> equips;

    [SerializeField] TMP_Text Level;
    [SerializeField] TMP_Text HP;
    [SerializeField] TMP_Text MaxHP;
    [SerializeField] TMP_Text AP;
    [SerializeField] TMP_Text MinATK;
    [SerializeField] TMP_Text MaxATK;
    [SerializeField] TMP_Text CRT;
    [SerializeField] TMP_Text DEF;
    [SerializeField] TMP_Text SPD;
    [SerializeField] TMP_Text EXP;
    [SerializeField] TMP_Text RequiredEXP;

    public void SetLevel(int _Value)
    { 
        Level.text = (_Value + 1).ToString();
    }

    public void SetAP(int _Value)
    {
        AP.text = _Value.ToString();
    }

    public void SetHP(int _Value)
    {
        HP.text = _Value.ToString();
    }

    public void SetMaxHP(int _Value)
    {
        MaxHP.text = _Value.ToString();
    }

    public void SetMinATK(int _Value)
    {
        MinATK.text = _Value.ToString();
    }

    public void SetMaxATK(int _Value)
    {
        MaxATK.text = _Value.ToString();
    }

    public void SetCRT(int _Value)
    {
        CRT.text = _Value.ToString();
    }

    public void SetDEF(int _Value)
    {
        DEF.text = _Value.ToString();
    }

    public void SetSPD(int _Value)
    {
        SPD.text = _Value.ToString();
    }

    public void SetEXP(int _Value)
    {
        EXP.text = _Value.ToString();
    }

    public void SetRequiredEXP(int _Value)
    {
        RequiredEXP.text = _Value.ToString();
    }

}