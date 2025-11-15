using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianBuff : ISkill
{
    PlayerManager playerManager;

    bool isPlayer;
    bool isSkillDone;

    public MagicianBuff(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
        isSkillDone = false;
        isPlayer = true;
    }

    public void Execute()
    {
        EventMessageManager.Instance.MessageQueueRegistry(new EventContainer() { eventType = ContextType.Battle, Context = "(마법사) 전투의 함성을 사용합니다." });
        GameObject VFX = ResourceManager.Instance.VFXResources[VFXName.BuffEffectA].GetVFXInstance();

        playerManager.animator.animator.Play("BuffExecute");
    }

    public void Process(ProcessType _React)
    {
        playerManager.status.GainAP(2);
        GameObject VFX = ResourceManager.Instance.VFXResources[VFXName.BuffEffectA].GetVFXInstance();
        VFX.transform.position = playerManager.transform.forward + playerManager.transform.position + Vector3.up * 0.5f;

        foreach (BattlePhase target in playerManager.battler.CurrentTargets)
        {
            GameObject VFX2 = ResourceManager.Instance.VFXResources[VFXName.BuffEffectA].GetVFXInstance();
            VFX2.transform.position = target.transform.forward + target.transform.position + Vector3.up * 0.5f;
            PlayerPhase playerPhase = (PlayerPhase)target;
            playerPhase.playerManager.status.HPChange((int)(playerPhase.playerManager.status.aMaxHP * 0.3f));
        }

        playerManager.battler.DelegateRun(TurnOverEffect());

    }

    public void QTEAction() { }

    public void Done() { }

    IEnumerator TurnOverEffect()
    {
        playerManager.phaser.CurrentPhase = PhaseType.Done;
        yield return new WaitForSeconds(3f);
        BattleSystemManager.Instance.UpdateEntry();
        playerManager.phaser.CurrentPhase = PhaseType.Wait;
    }
}