using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotionManager : MonoBehaviour
{

    [Header("States")]
    public bool isSleep;
    public bool isStand;
    public bool isPatrol;
    public bool isTrace;
    public bool isAttack;
    public bool isHit;
    public bool isGroggy;
    
    [Space]
    [Header("Enemy Stat")]
    [SerializeField] private float CharacterSpeed;
    [SerializeField] private float TraceSpeed;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private float RotationLimit;

    [Space]
    [Header("Components")]

    private EnemyManager enemyManager;
    public CharacterController controller;
    public NavMeshAgent agent;
    public Rigidbody rigid;
    public Vector3 BornPos;

    [Space]
    [Header("Patrol")]
    [SerializeField] private List<Transform> PatrolPaths;
    [SerializeField] private List<Vector3> ListPaths;
    [SerializeField] private Queue<Vector3> QueuePaths;
    Vector3 CurrentDestination;

    [Space]
    [Header("Trace")]
    [HideInInspector] private EnemyTraceZone enemyTraceZone;
    [SerializeField] private float TraceTime;
    [SerializeField] private float CurTraceTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        enemyManager = GetComponent<EnemyManager>();
        enemyTraceZone = GetComponentInChildren<EnemyTraceZone>();
    }

    private void Start()
    {
        BornPos = transform.position;

        if (PatrolPaths == null) return;
        ListPaths = new List<Vector3>();
        foreach (Transform t in PatrolPaths)
        {
            ListPaths.Add(t.position);
        }

        if (ListPaths.Count == 0)
        {
            ListPaths = new List<Vector3>();
            ListPaths.Add(transform.position);

        }
        else if (ListPaths.Count == 1)
        {
            ListPaths.Insert(0, transform.position);
        }

        QueuePaths = new Queue<Vector3>(ListPaths);
        CurrentDestination = QueuePaths.Dequeue();
        

    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState == GameModeState.Battle) return;
        if (enemyManager != null) enemyManager.animator.MoveAnimation(agent.velocity.normalized.magnitude);
    }

    public void EnemyAttack()
    {
        if (enemyManager.locomotor.isHit) return;
        if (GameManager.Instance.CurrentState == GameModeState.Battle) return;
        isAttack = true;
        enemyManager.animator.OnEncounterAttackAnimation();
    }

    public INode.STATE AttackPlayerAction()
    {
        if (!isAttack)
        {
            return INode.STATE.FAILED;
        } 

        agent.isStopped = true;
        agent.ResetPath();

        return INode.STATE.SUCCESS;
    }

    public void EnemyTrace()
    {
        if (enemyManager.locomotor.isHit) return;
        if (GameManager.Instance.CurrentState == GameModeState.Battle) return;
        isTrace = true;
        enemyTraceZone.enabled = false;
        CurTraceTime = TraceTime;
        agent.speed = TraceSpeed;
    }

    public INode.STATE TracePlayerAction()
    {
        if (!isTrace)
        {
            agent.speed = CharacterSpeed;
            agent.destination = CurrentDestination;
            if (!enemyTraceZone.enabled) enemyTraceZone.enabled = true;
            return INode.STATE.FAILED;
        }

        CurTraceTime -= Time.deltaTime;
        if (CurTraceTime < 0f)
        {
            isTrace = false;
            CurTraceTime = TraceTime;
            enemyTraceZone.enabled = true;
            return INode.STATE.FAILED;
        }

        if(PlayerCharacterManager.Instance.CurrentPlayer == null) return INode.STATE.FAILED;

        agent.destination = PlayerCharacterManager.Instance.CurrentPlayer.transform.position;
        Vector3 mvVector = Time.deltaTime * new Vector3(1f, 0f, 1f);
        agent.Move(mvVector);
        var turnTowardNavSteeringTarget = agent.steeringTarget;

        Vector3 direction = (turnTowardNavSteeringTarget - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);

        return INode.STATE.SUCCESS;
    }



    public INode.STATE EnemyPatrolAction()
    {
        //if (isTrace) return INode.STATE.FAILED;
        //if (isSleep || isStand) return INode.STATE.FAILED;

        agent.Move(Time.deltaTime * new Vector3(1f, 0f, 1f));
        var turnTowardNavSteeringTarget = agent.steeringTarget;

        Vector3 direction = (turnTowardNavSteeringTarget - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
        return INode.STATE.SUCCESS;
    }

    public INode.STATE EnemyDestinationCheckAction()
    {

        if (Vector3.Distance(transform.position, agent.destination) < 0.1f)
        {
            if (QueuePaths.Count == 0)
            {
                ListPaths.Reverse();
                QueuePaths = new Queue<Vector3>(ListPaths);
            }
            CurrentDestination = QueuePaths.Dequeue();
            agent.destination = CurrentDestination;
            return INode.STATE.RUN;
        }

        return INode.STATE.RUN;
    }
    

}
