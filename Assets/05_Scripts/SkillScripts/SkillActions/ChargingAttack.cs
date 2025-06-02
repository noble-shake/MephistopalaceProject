using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingAttack : ISkill
{
    PlayerManager playerManager;
    EnemyManager enemyManager;
    int RequiredQTE;

    bool isPlayer;
    bool isSkillDone;
    bool RequiredChargingSatisfied;
    bool ChargingHit;

    public ChargingAttack(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
        isSkillDone = false;
        isPlayer = true;
    }

    public void Execute()
    {
        if (playerManager.status.AP >= 3)
        {
            playerManager.status.UseAP(3);
            RequiredChargingSatisfied = true;
        }
        else
        {
            playerManager.status.GainAP(1);
            RequiredChargingSatisfied = false;
        }

        EventMessageManager.Instance.MessageQueueRegistry(new EventContainer() { eventType=ContextType.Battle, Context = "ChargingAttack 스킬을 사용합니다." });
        playerManager.battler.MoveToTarget(BattleSystemManager.Instance.CenterPoints[1], DoubleSlashProcess());
        // BattleSystemManager.Instance.CoroutineRunner(DoubleSlashEffect());
    }

    public void Process(ProcessType _React)
    {
        if (!RequiredChargingSatisfied)
        {
            if (ChargingHit) return;

            ChargingHit = true;
        }

        foreach(BattlePhase target in playerManager.battler.CurrentTargets)
        {
            GameObject VFX = ResourceManager.Instance.VFXResources[VFXName.KnightSlash].GetVFXInstance();
            VFX.transform.position = target.transform.forward + target.transform.position + Vector3.up;

            if (RequiredChargingSatisfied)
            {
                GameObject SlashVFX = ResourceManager.Instance.VFXResources[VFXName.KnightChargingBladeA].GetVFXInstance();
                SlashVFX.transform.position = target.transform.forward + target.transform.position + Vector3.up;

                GameObject SlashAddVFX = ResourceManager.Instance.VFXResources[VFXName.KnightChargingBladeB].GetVFXInstance();
                SlashVFX.transform.position = target.transform.forward + target.transform.position + Vector3.up;
            }
            else
            {
                GameObject SlashVFX = ResourceManager.Instance.VFXResources[VFXName.KnightChargingBladeA].GetVFXInstance();
                SlashVFX.transform.position = target.transform.forward + target.transform.position + Vector3.up;


            }

            EnemyPhase enemyPhase = (EnemyPhase)target;
            int HitDamage = UnityEngine.Random.Range(playerManager.status.aMinATK, playerManager.status.aMaxATK + 1);
            Debug.Log(HitDamage);
            enemyPhase.enemyManager.status.HPChange(-HitDamage);
            enemyPhase.enemyManager.animator.animator.Play("Hit");
        }
    }

    public void QTEAction()
    {
        BattleSystemManager.Instance.QTEEngage(3);
    }

    public void Done()
    {
        BattleSystemManager.Instance.CoroutineRunner(EndEffect());
    }

    IEnumerator DoubleSlashProcess()
    {
        playerManager.animator.animator.Play("ChargingAttack");
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