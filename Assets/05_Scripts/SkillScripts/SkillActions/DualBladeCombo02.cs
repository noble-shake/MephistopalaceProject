using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualBladeCombo02 : ISkill
{
    PlayerManager playerManager;
    EnemyManager enemyManager;
    int RequiredQTE;

    bool isPlayer;
    bool isSkillDone;
    bool ChargingHit;

    public DualBladeCombo02(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
        isSkillDone = false;
        isPlayer = true;
    }

    public void Execute()
    {
        if (playerManager.status.AP < 3)
        {
            return;
        }
        else
        {
            playerManager.status.UseAP(3);
        }

        EventMessageManager.Instance.MessageQueueRegistry(new EventContainer() { eventType = ContextType.Battle, Context = "날아 베기 스킬을 사용합니다." });
        playerManager.battler.MoveToTarget(BattleSystemManager.Instance.CenterPoints[1], DoubleSlashProcess());
        // BattleSystemManager.Instance.CoroutineRunner(DoubleSlashEffect());
    }

    public void Process(ProcessType _React)
    {

        foreach (BattlePhase target in playerManager.battler.CurrentTargets)
        {
            GameObject VFX = ResourceManager.Instance.VFXResources[VFXName.KnightSlash].GetVFXInstance();
            VFX.transform.position = target.transform.forward + target.transform.position + Vector3.up;

            GameObject SlashVFX = ResourceManager.Instance.VFXResources[VFXName.KnightChargingBladeA].GetVFXInstance();
            SlashVFX.transform.position = target.transform.forward + target.transform.position + Vector3.up;

            GameObject SlashAddVFX = ResourceManager.Instance.VFXResources[VFXName.KnightChargingBladeB].GetVFXInstance();
            SlashVFX.transform.position = target.transform.forward + target.transform.position + Vector3.up;

            EnemyPhase enemyPhase = (EnemyPhase)target;
            int success = BattleSystemManager.Instance.SuccessQTE;
            int total = BattleSystemManager.Instance.TotalQTE;
            int actualMinATK = (int)(playerManager.status.aMinATK * 1.5f) + (int)(playerManager.status.aMinATK * 1.5f * (success / total));
            int actualMaxATK = (int)(playerManager.status.aMaxATK * 1.5f) + (int)(playerManager.status.aMaxATK * 1.5f * (success / total));
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
        BattleSystemManager.Instance.CoroutineRunner(EndEffect());
    }

    IEnumerator DoubleSlashProcess()
    {
        playerManager.animator.animator.Play("BattleIdle");
        playerManager.GetComponent<PlayerSkillManager>().playerableDirector.playableAsset = playerManager.GetComponent<PlayerSkillManager>().Move02;
        playerManager.GetComponent<PlayerSkillManager>().playerableDirector.Play();
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