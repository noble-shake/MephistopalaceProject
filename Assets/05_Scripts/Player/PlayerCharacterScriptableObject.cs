using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCharacterSO", menuName = "PalaceCharacter/PlayerScriptableObject", order = -1)]
public class PlayerCharacterScriptableObject : ScriptableObject
{
    public string Name;
    [TextArea] public string Description;
    public Sprite Portrait;
    public GameObject CharacterPrefab;

    public Sprite CharacterDescription;
    [Space]
    [Header("Stats")]
    [SerializeField] public int HP;
    [SerializeField] public int AP;
    [SerializeField] public int MinATK;
    [SerializeField] public int MaxATK;
    [SerializeField] public int SPD;
    [SerializeField] public int DEF;
    [SerializeField] public int CRT;

    [Space]
    [Header("Skills (TBD)")]
    public Sprite CharacterAbility;

    [Header("Ability")]
    [TextArea] public string CharacterAbillityDescription;

    public StatContainer GetStatChange()
    {
        return new StatContainer { HP = HP, AP = AP, minATK = MinATK, maxATK = MaxATK, CRT = CRT, SPD = SPD, DEF = DEF };
    }
}