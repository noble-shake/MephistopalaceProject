using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualBladeCombo01 : ISkill
{
    PlayerManager playerManager;
    EnemyManager enemyManager;
    int RequiredQTE;

    bool isPlayer;
    bool isSkillDone;

    public DualBladeCombo01(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
        isSkillDone = false;
        isPlayer = true;
    }

    public void Execute()
    {

        if (isPlayer)
        {

            playerManager.status.GainAP(1);

        }
        EventMessageManager.Instance.MessageQueueRegistry(new EventContainer() { eventType = ContextType.Battle, Context = "마구 베기 스킬을 사용합니다." });
        playerManager.battler.MoveToTarget(playerManager.battler.CurrentTargets[0], DoubleSlashProcess());
        // BattleSystemManager.Instance.CoroutineRunner(DoubleSlashEffect());
    }

    public void Process(ProcessType _React)
    {
        foreach (BattlePhase target in playerManager.battler.CurrentTargets)
        {
            GameObject VFX = ResourceManager.Instance.VFXResources[VFXName.KnightSlash].GetVFXInstance();
            VFX.transform.position = target.transform.forward + target.transform.position + Vector3.up * 0.5f;
            EnemyPhase enemyPhase = (EnemyPhase)target;
            int success = BattleSystemManager.Instance.SuccessQTE;
            int total = BattleSystemManager.Instance.TotalQTE;
            int actualMinATK = (int)(playerManager.status.aMinATK * 0.5f) + (int)(playerManager.status.aMinATK * 0.5f * (success / total));
            int actualMaxATK = (int)(playerManager.status.aMaxATK * 0.5f) + (int)(playerManager.status.aMinATK * 0.5f * (success / total));
            int HitDamage = UnityEngine.Random.Range(actualMinATK, actualMaxATK + 1);
            Debug.Log(HitDamage);
            enemyPhase.enemyManager.status.HPChange(-HitDamage);
            enemyPhase.enemyManager.animator.animator.Play("Hit");
        }
    }

    public void QTEAction()
    {
        BattleSystemManager.Instance.QTEEngage(4);
    }

    public void Done()
    {
        //BattleSystemManager.Instance.CoroutineRunner(EndEffect());
        BattleSystemManager.Instance.UpdateEntry();
        playerManager.phaser.CurrentPhase = PhaseType.Wait;
    }

    IEnumerator DoubleSlashProcess()
    {
        playerManager.animator.animator.Play("Combo01");
        yield return null;
    }

    IEnumerator EndEffect()
    {

        playerManager.phaser.CurrentPhase = PhaseType.Done;
        yield return new WaitForSeconds(2f);
        BattleSystemManager.Instance.UpdateEntry();
        playerManager.phaser.CurrentPhase = PhaseType.Wait;
    }
}