using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonCombo04 : ISkill
{
    EnemyManager enemyManager;
    int QTECount;
    int RequiredQTE;
    bool isSkillDone;

    public DragonCombo04(EnemyManager enemyManager)
    {
        this.enemyManager = enemyManager;
        isSkillDone = false;
        RequiredQTE = enemyManager.battler.CurrentRequireQTE;
    }

    public void Execute()
    {
        EventMessageManager.Instance.MessageQueueRegistry(new EventContainer() { eventType = ContextType.Battle, Context = "던전의 주인이 강력한 공격을 수행합니다." });
        enemyManager.battler.MoveToTarget(BattleSystemManager.Instance.CenterPoints[1], EnemyComboProcess());
    }

    public void Process(ProcessType _React)
    {
        bool Parryable = _React == ProcessType.Enable || _React == ProcessType.OnlyParry ? true : false;
        bool Evadable = _React == ProcessType.Enable || _React == ProcessType.OnlyEvade ? true : false;

        foreach (BattlePhase target in enemyManager.battler.CurrentTargets)
        {
            PlayerPhase enemyPhase = (PlayerPhase)target;

            if (enemyPhase.isParrying && Parryable)
            {
                enemyPhase.ParrySuccess(true);
                // QTECount++;
                //if (RequiredQTE == QTECount)
                //{
                //    enemyPhase.OnCounterAttack(enemyManager);
                //}
            }
            else if (enemyPhase.isEvading && Evadable)
            {
                enemyPhase.EvadeSuccess(true);
            }
            else
            {
                enemyPhase.ParrySuccess(false);
                GameObject VFX = ResourceManager.Instance.VFXResources[VFXName.SkeletonAttackEffect].GetVFXInstance();
                VFX.transform.position = target.transform.forward + target.transform.position + Vector3.up;
                int HitDamage = UnityEngine.Random.Range(enemyManager.status.aMinATK, enemyManager.status.aMaxATK + 1);
                Debug.Log(HitDamage);
                enemyPhase.playerManager.status.HPChange(-HitDamage);
                enemyPhase.playerManager.animator.animator.Play("Hit");
            }


        }
    }

    public void QTEAction()
    {
        // BattleSystemManager.Instance.QTEEngage(2);
    }

    public void Done()
    {
        BattleSystemManager.Instance.CoroutineRunner(EndEffect());
    }

    IEnumerator EnemyComboProcess()
    {
        // enemyManager.animator.animator.Play("BattleCombo04");
        DragonSkillManager manager = enemyManager.GetComponent<DragonSkillManager>();
        manager.playableDirector.playableAsset = manager.GroundSkillCombo;
        manager.playableDirector.Play();
        yield return null;
    }

    IEnumerator EndEffect()
    {

        enemyManager.phaser.CurrentPhase = PhaseType.Done;
        yield return new WaitForSeconds(2f);
        enemyManager.phaser.CurrentPhase = PhaseType.Wait;
        BattleSystemManager.Instance.UpdateEntry();

    }
}