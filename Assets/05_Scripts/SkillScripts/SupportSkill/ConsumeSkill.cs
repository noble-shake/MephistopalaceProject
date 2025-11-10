using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeSkill : ISkill
{
    PlayerManager playerManager;

    bool isPlayer;
    bool isSkillDone;

    public ConsumeSkill(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
        isSkillDone = false;
        isPlayer = true;
    }

    public void Execute()
    {
        EventMessageManager.Instance.MessageQueueRegistry(new EventContainer() { eventType = ContextType.Battle, Context = "턴 넘기기를 사용합니다." });
        GameObject VFX = ResourceManager.Instance.VFXResources[VFXName.BuffEffectA].GetVFXInstance();
        if (isPlayer)
        {
            playerManager.status.HPChange((int)(playerManager.status.MaxHP * 0.1f));
            playerManager.status.GainAP(1);
            VFX.transform.position = playerManager.transform.position + Vector3.up * 0.5f;

            BattleSystemManager.Instance.CoroutineRunner(TurnOverEffect());
        }
    }

    public void Process(ProcessType _React) { }

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