using UnityEngine;

public class EnemyManager : CharacterManager
{
    public EnemyType enemyType;

    [HideInInspector] public EnemyStatusManager status;
    [HideInInspector] public EnemyAnimationManager animator;
    [HideInInspector] public EnemyEncounterManager encounter;
    [HideInInspector] public EnemyLocomotionManager locomotor;
    [HideInInspector] public EnemyBattleManager battler;
    [HideInInspector] public CharacterSoundManager speaker;

    [SerializeField] public Vector3 OriginPos;


    private void Awake()
    {
        status = GetComponent<EnemyStatusManager>();
        animator = GetComponent<EnemyAnimationManager>();
        encounter = GetComponent<EnemyEncounterManager>();
        locomotor = GetComponent<EnemyLocomotionManager>();
        battler = GetComponent<EnemyBattleManager>();
        speaker = GetComponent<CharacterSoundManager>();
    }

    private void Start()
    {
        OriginPos = transform.position;
    }
}