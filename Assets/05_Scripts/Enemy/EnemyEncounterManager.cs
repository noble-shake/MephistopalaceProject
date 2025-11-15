using System.Collections;
using UnityEngine;

public class EnemyEncounterManager : MonoBehaviour
{
    [SerializeField] public EnemyScriptableObject enemyInfo;
    [HideInInspector] public EnemyManager enemyManager;
    [SerializeField] public EnemyTriggerJudge judgeObejct;
    [SerializeField] public EnemyAttackZone attackZone;

    SelectorNode RootNode;
    SequenceNode InitializeSequence;
    SequenceNode SleepSequence;
    SequenceNode PatrolSequence;
    SequenceNode TraceSequence;
    SequenceNode AttackSequence;
    SequenceNode GroggySequence;

    ActionNode idleAction; // 대기 액션 노드
    ActionNode returnAction; // 귀환 노드

    /*
        ROOT NODE : SELECT
        
        1. INITALIZE : ACTION Initialize. Automatically Move To Sleep or Patrol
        2. SLEEP : No Response Stae untill Triggered.
        3. PATROL : Move and Wander.
        4. TRACE : Trace Player.
        5. ATTACK : Attack Player and Encount Battle.
        6. GROGGY : Hit by Magician Attack.


     */

    private void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        judgeObejct.EnemyAttackCollider.enabled = false;

        RootNode = new SelectorNode();

        if (enemyManager.locomotor.isSleep)
        {
            SleepSequenceBind();
            GroggySequenceBind();
            RootNode.Add(SleepSequence);
            RootNode.Add(GroggySequence);
        }
        else
        {
            AttackSequenceBind();
            TraceSequenceBind();
            PatrolSequenceBind();


            //GroggySequenceBind();
            RootNode.Add(AttackSequence);
            RootNode.Add(TraceSequence);
            RootNode.Add(PatrolSequence);


            //RootNode.Add(GroggySequence);
        }


    }

    void Update()
    {
        if (GameManager.Instance.CurrentState != GameModeState.Encounter) return;
        if (enemyManager.locomotor.isHit || enemyManager.locomotor.isGroggy)
        {
            return;
        }
        if (GameManager.Instance.isGamePause) return;

        RootNode.Evaluate();
    }

    #region Patrol Sequence

    private void PatrolSequenceBind()
    {
        PatrolSequence = new SequenceNode();
        PatrolSequence.Add(new ActionNode(enemyManager.locomotor.EnemyPatrolAction));
        PatrolSequence.Add(new ActionNode(enemyManager.locomotor.EnemyDestinationCheckAction));
    }

    #endregion

    #region Trace Sequence

    private void TraceSequenceBind()
    {
        TraceSequence = new SequenceNode();
        TraceSequence.Add(new ActionNode(enemyManager.locomotor.TracePlayerAction));
    }

    #endregion

    #region Attack Sequence

    private void AttackSequenceBind()
    {
        AttackSequence = new SequenceNode();
        AttackSequence.Add(new ActionNode(enemyManager.locomotor.AttackPlayerAction));
    }

    #endregion


    #region Groggy Sequence

    private void GroggySequenceBind()
    {
        GroggySequence = new SequenceNode();
        //GroggySequence.Add();
    }



    #endregion


    #region SLEEP Sequence

    private void SleepSequenceBind()
    {
        SleepSequence = new SequenceNode();
        SleepSequence.Add(new ActionNode(IdleActionNode));
    }

    INode.STATE IdleActionNode()
    {
        Debug.Log("Idle");
        return INode.STATE.RUN;
    }

    #endregion

    public void OnEncounterAttackExecute()
    {
        enemyManager.locomotor.isAttack = false;
    }

    public void OnEncounterAttackJudge()
    {
        judgeObejct.EnemyAttackCollider.enabled = true;
    }

    public void OffEncounterAttackJudge()
    {
        judgeObejct.EnemyAttackCollider.enabled = false;
    }

    public void OnHitEncounter()
    {
        GameManager.Instance.CurrentState = GameModeState.Battle;
        enemyManager.locomotor.isHit = true;
        enemyManager.locomotor.agent.isStopped = true;
        enemyManager.animator.animator.StopPlayback();
        enemyManager.animator.animator.Play("Hit");
        BattleSystemManager.Instance.SetReferenceEnemy(enemyManager, enemyInfo);
        BattleSystemManager.Instance.OnEngageSequence();
        StartCoroutine(AfterHitProcess());
    }

    IEnumerator AfterHitProcess()
    {
        yield return new WaitForSeconds(2f);
        transform.position = enemyManager.locomotor.BornPos;
        enemyManager.locomotor.isHit = false;
        enemyManager.locomotor.agent.isStopped = false;
        gameObject.SetActive(false);

    }





}