using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimbCombo02 : ISkill
{
    EnemyManager enemyManager;
    int RequiredQTE;
    bool isSkillDone;

    public SlimbCombo02(EnemyManager enemyManager)
    {
        this.enemyManager = enemyManager;
        isSkillDone = false;
    }

    public void Execute()
    {
        EventMessageManager.Instance.MessageQueueRegistry(new EventContainer() { eventType=ContextType.Battle ,Context = "슬라임이 전체 공격을 사용합니다." });
        enemyManager.battler.MoveToTarget(enemyManager.battler.CurrentTargets[0], SlimbComboProcess());
        // BattleSystemManager.Instance.CoroutineRunner(DoubleSlashEffect());
    }

    public void Process(ProcessType _React)
    {
        bool Parryable = _React == ProcessType.Enable || _React == ProcessType.OnlyParry ? true : false;
        bool Evadable = _React == ProcessType.Enable || _React == ProcessType.OnlyEvade ? true : false;

        foreach (BattlePhase target in enemyManager.battler.CurrentTargets)
        {
            PlayerPhase enemyPhase = (PlayerPhase)target;

            if (enemyPhase.isEvading && Evadable)
            {
                enemyPhase.EvadeSuccess(true);
            }
            else
            {
                enemyPhase.EvadeSuccess(false);
                GameObject VFX = ResourceManager.Instance.VFXResources[VFXName.KnightSlash].GetVFXInstance();
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

    IEnumerator SlimbComboProcess()
    {
        enemyManager.animator.animator.Play("SlimbCombo02");
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