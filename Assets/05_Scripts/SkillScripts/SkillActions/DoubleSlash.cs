using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSlash : ISkill
{
    PlayerManager playerManager;
    EnemyManager enemyManager;
    int RequiredQTE;

    bool isPlayer;
    bool isSkillDone;

    public DoubleSlash(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
        isSkillDone = false;
        isPlayer = true;
    }

    public DoubleSlash(EnemyManager enemyManager)
    {
        this.enemyManager = enemyManager;
        isSkillDone = false;
        isPlayer = false;
    }

    public void Execute()
    {

        if (isPlayer)
        {
            
            playerManager.status.GainAP(1);
            
        }
        EventMessageManager.Instance.MessageQueueRegistry(new EventContainer() { eventType=ContextType.Battle, Context = "Double Slash 스킬을 사용합니다." });
        playerManager.battler.MoveToTarget(playerManager.battler.CurrentTargets[0], DoubleSlashProcess());
        // BattleSystemManager.Instance.CoroutineRunner(DoubleSlashEffect());
    }

    public void Process(ProcessType _React)
    {
        foreach(BattlePhase target in playerManager.battler.CurrentTargets)
        {
            GameObject VFX = ResourceManager.Instance.VFXResources[VFXName.KnightSlash].GetVFXInstance();
            VFX.transform.position = target.transform.forward + target.transform.position + Vector3.up * 0.5f;
            EnemyPhase enemyPhase = (EnemyPhase)target;
            int HitDamage = UnityEngine.Random.Range(playerManager.status.aMinATK, playerManager.status.aMaxATK + 1);
            Debug.Log(HitDamage);
            enemyPhase.enemyManager.status.HPChange(-HitDamage);
            enemyPhase.enemyManager.animator.animator.Play("Hit");
        }
    }

    public void QTEAction()
    {
        BattleSystemManager.Instance.QTEEngage(2);
    }

    public void Done()
    {
        //BattleSystemManager.Instance.CoroutineRunner(EndEffect());
        BattleSystemManager.Instance.UpdateEntry();
        playerManager.phaser.CurrentPhase = PhaseType.Wait;
    }

    IEnumerator DoubleSlashProcess()
    {
        playerManager.animator.animator.Play("DoubleSlash");
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