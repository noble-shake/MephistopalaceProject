using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOver : ISkill
{
    PlayerManager playerManager;
    EnemyManager enemyManager;

    bool isPlayer;
    bool isSkillDone;

    public TurnOver(PlayerManager playerManager) 
    {
        this.playerManager = playerManager;
        isSkillDone = false;
        isPlayer = true;
    }

    public TurnOver(EnemyManager enemyManager)
    {
        this.enemyManager = enemyManager;
        isSkillDone = false;
        isPlayer = false;
    }

    public void Execute()
    {
        GameObject VFX = ResourceManager.Instance.VFXResources[VFXName.BuffEffectA].GetVFXInstance();
        if (isPlayer)
        {
            playerManager.status.HPChange((int)(playerManager.status.MaxHP * 0.1f));
            playerManager.status.GainAP(1);
            VFX.transform.position = playerManager.transform.position + Vector3.up * 0.5f;

            BattleSystemManager.Instance.CoroutineRunner(TurnOverEffect());
        }
        else
        {
            enemyManager.status.HPChange((int)(enemyManager.status.MaxHP * 0.1f));
            VFX.transform.position = enemyManager.transform.position + Vector3.up * 0.5f;

            BattleSystemManager.Instance.CoroutineRunner(TurnOverEnemyEffect());
        }

    }

    public void Process(ProcessType _React) {}

    public void QTEAction() {}

    public void Done() { }

    IEnumerator TurnOverEffect()
    {
        playerManager.phaser.CurrentPhase = PhaseType.Done;
        yield return new WaitForSeconds(2f);
        BattleSystemManager.Instance.UpdateEntry();
        playerManager.phaser.CurrentPhase = PhaseType.Wait;
    }

    IEnumerator TurnOverEnemyEffect()
    {
        enemyManager.phaser.CurrentPhase = PhaseType.Done;
        yield return new WaitForSeconds(2f);
        BattleSystemManager.Instance.UpdateEntry();
        enemyManager.phaser.CurrentPhase = PhaseType.Wait;
    }
}