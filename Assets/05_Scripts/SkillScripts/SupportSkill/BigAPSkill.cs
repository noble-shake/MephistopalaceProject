using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigAPSkill : ISkill
{
    PlayerManager playerManager;

    bool isPlayer;
    bool isSkillDone;

    public BigAPSkill(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
        isSkillDone = false;
        isPlayer = true;
    }

    public void Execute()
    {
        EventMessageManager.Instance.MessageQueueRegistry(new EventContainer() { eventType = ContextType.Battle, Context = "대형 AP 물약을 사용합니다." });
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
            InventoryManager.Instance.ItemUse(SkillActions.BigAPConsume, playerPhase.playerManager); // 여기까지 온 이상, 인벤토리 안에 해당 아이템이 없을 이유가 없다.
        }

        playerManager.battler.DelegateRun(TurnOverEffect());

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