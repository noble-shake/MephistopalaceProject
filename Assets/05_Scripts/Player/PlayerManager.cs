using UnityEngine;

public class PlayerManager : CharacterManager
{
    public CharacterType characterType;

    [Space]
    [Header("Encounter States")]
    public bool isAttack;
    public bool isPause;
    public bool isItemEarnAction;
    public bool isGravityOff;
    public bool isJump;
    public bool isDead;

    [Space]
    [Header("Encounter States")]
    public bool isPlayerTurn;

    [HideInInspector] public PlayerStatusManager status;
    [HideInInspector] public PlayerAnimationManager animator;
    [HideInInspector] public PlayerEncounterManager encounter;
    [HideInInspector] public PlayerLocomotionManager locomotor;
    [HideInInspector] public PlayerBattleManager battler;


    private void Start()
    {
        status = GetComponent<PlayerStatusManager>();
        animator = GetComponent<PlayerAnimationManager>();
        encounter = GetComponent<PlayerEncounterManager>();
        locomotor = GetComponent<PlayerLocomotionManager>();
        battler = GetComponent<PlayerBattleManager>();
    }
}