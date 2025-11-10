using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBuff : ISkill
{
    PlayerManager playerManager;

    bool isPlayer;
    bool isSkillDone;

    public KnightBuff(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
        isSkillDone = false;
        isPlayer = true;
    }

    public void Execute()
    {
        EventMessageManager.Instance.MessageQueueRegistry(new EventContainer() { eventType = ContextType.Battle, Context = "전투의 함성 [AP 회복]을 사용합니다." });
        GameObject VFX = ResourceManager.Instance.VFXResources[VFXName.BuffEffectA].GetVFXInstance();

        playerManager.animator.animator.Play("BuffExecute");
    }

    public void Process(ProcessType _React) 
    {
        foreach (BattlePhase target in playerManager.battler.CurrentTargets)
        {
            GameObject VFX = ResourceManager.Instance.VFXResources[VFXName.BuffEffectA].GetVFXInstance();
            VFX.transform.position = target.transform.forward + target.transform.position + Vector3.up * 0.5f;
            PlayerPhase playerPhase = (PlayerPhase)target;
            playerPhase.playerManager.status.GainAP(3);
        }

        BattleSystemManager.Instance.CoroutineRunner(TurnOverEffect());

    }

    public void QTEAction() { }

    public void Done() { }

    IEnumerator TurnOverEffect()
    {
        playerManager.phaser.CurrentPhase = PhaseType.Done;
        yield return new WaitForSeconds(2f);
        BattleSystemManager.Instance.UpdateEntry();
        playerManager.phaser.CurrentPhase = PhaseType.Wait;
    }
}