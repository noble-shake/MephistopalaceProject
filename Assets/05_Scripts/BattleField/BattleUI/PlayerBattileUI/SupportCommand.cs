using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class SupportCommand : BattleCommand
{
    public override void SetCommand(PlayerManager player, List<SkillScriptableObject> Skills)
    {
        foreach (CommandFrame frame in DisplayedList)
        {
            frame.OnHighlight(false);
        }

        List<SkillScriptableObject> ActiveSkills = new List<SkillScriptableObject>();
        foreach (SkillScriptableObject targetSkill in Skills)
        {
            if (targetSkill.isConsumeRequire == false)
            {
                ActiveSkills.Add(targetSkill);
                continue;
            }

            //SmallHPConsume,
            //MiddleHPConsume,
            //BigHPConsume,
            //SmallAPConsume,
            //MiddleAPConsume,
            //BigAPConsume,

            if (InventoryManager.Instance.ItemExist(targetSkill.ActionScript) == true)
            {
                ActiveSkills.Add(targetSkill);
            }

        }

        OwnScritableObject = ActiveSkills;

        CurrentCommand = DisplayedList[0];
        playerManager = player;
        TotalSkills = OwnScritableObject.Count;
        CurrentIndex = 0;
        CurrentPage = 0;
        DisplayedList[0].OnHighlight(true);
        if (TotalSkills > 3)
        {
            TotalIndex = 2;
            TotalPage = TotalSkills / 3 + 1;
            AllocatedScritableObject = OwnScritableObject.GetRange(0, 3);
            for (int idx = 0; idx < 3; idx++)
            {
                DisplayedList[idx].Name = AllocatedScritableObject[idx].Name;
                DisplayedList[idx].Description = AllocatedScritableObject[idx].Description;
            }
        }
        else
        {
            if (TotalSkills != 0)
            {
                TotalIndex = TotalSkills - 1;
                TotalPage = 1;
                AllocatedScritableObject = OwnScritableObject.GetRange(0, TotalSkills);
            }
            else
            {
                TotalIndex = 0;
                TotalPage = 1;
            }

            int count = 0;
            for (int idx = 0; idx < TotalSkills; idx++)
            {
                DisplayedList[idx].Name = AllocatedScritableObject[idx].Name;
                DisplayedList[idx].Description = AllocatedScritableObject[idx].Description;
                count++;
            }

            for (int idx = count; idx < 3; idx++)
            {
                DisplayedList[idx].Name = "";
                DisplayedList[idx].Description = "";
            }

        }
    }

    public override void CommandExecute()
    {
        if (playerManager.status.AP < AllocatedScritableObject[CurrentIndex].RequiredAP)
        {
            EventMessageManager.Instance.MessageQueueRegistry(new EventContainer() { eventType = ContextType.Battle, Context = "AP가 부족합니다!" });
            if (AllocatedScritableObject[CurrentIndex].APMustSatisfied) return;
        }

        playerManager.battler.SetSkillExecution(AllocatedScritableObject[CurrentIndex].ActionScript);
        playerManager.phaser.CurrentPhase = PhaseType.Activate;
        BattleSystemManager.Instance.SelectTarget(playerManager.battler.SkillSet[AllocatedScritableObject[CurrentIndex].ActionScript].activateTarget);
        CameraManager.Instance.OnLiveCamera(CameraType.BattleCenter);
        CameraManager.Instance.GetCamera().Follow = playerManager.phaser.AllocatedPoint;
        CameraManager.Instance.GetCamera().GetComponent<CinemachineSplineDolly>().CameraPosition = 0.5f;
        Display(false);

        return;
    }

    public override void CommandBack()
    {
        playerManager.phaser.PhaseCommand();
        playerManager.animator.animator.SetTrigger("MainCommand");
        return;
    }
}
