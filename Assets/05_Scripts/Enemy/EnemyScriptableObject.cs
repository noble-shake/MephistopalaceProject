using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCharacterSO", menuName = "PalaceCharacter/EnemyScriptableObject", order = -1)]
public class EnemyScriptableObject : ScriptableObject
{
    public string Name;
    [TextArea] public string Description;
    public Sprite Portrait;
    public GameObject CharacterPrefab;

    public int minPool;
    public int maxPool;

    public Sprite CharacterDescription;
    [Space]
    [Header("Stats")]
    [SerializeField] public int MinLevel;
    [SerializeField] public int MaxLevel;
    [SerializeField] public int HP;
    [SerializeField] public int AP;
    [SerializeField] public int MinATK;
    [SerializeField] public int MaxATK;
    [SerializeField] public int SPD;
    [SerializeField] public int DEF;
    [SerializeField] public int CRT;

    [Space]
    [Header("Rewards")]
    public List<ItemScriptableObject> RewardList;
    public int EXP;

    [Space]
    [Header("Unique Check")]
    public bool isUnique;

    public StatContainer GetEnemyStatChange()
    {
        return new StatContainer { HP = HP, AP = AP, minATK = MinATK, maxATK = MaxATK, CRT = CRT, SPD = SPD, DEF = DEF };
    }

}