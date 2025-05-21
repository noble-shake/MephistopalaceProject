using UnityEngine;

public class EnemyManager : CharacterManager
{
    [HideInInspector] public EnemyStatusManager status;
    [HideInInspector] public EnemyAnimationManager animator;
    [HideInInspector] public EnemyEncounterManager encounter;
    [HideInInspector] public EnemyLocomotionManager locomotor;

    [SerializeField] public Vector3 OriginPos;


    private void Awake()
    {
        status = GetComponent<EnemyStatusManager>();
        animator = GetComponent<EnemyAnimationManager>();
        encounter = GetComponent<EnemyEncounterManager>();
        locomotor = GetComponent<EnemyLocomotionManager>();
    }

    private void Start()
    {
        OriginPos = transform.position;
    }
}