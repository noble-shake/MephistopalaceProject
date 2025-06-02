using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class AttackCommand : BattleCommand
{
    public override void SetCommand(PlayerManager player, List<SkillScriptableObject> Skills)
    {
        foreach (CommandFrame frame in DisplayedList)
        {
            frame.OnHighlight(false);
        }
        OwnScritableObject = Skills;

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


        switch (CurrentIndex)
        {
            default:
                break;
            case 0:
                playerManager.battler.SetSkillExecution(AllocatedScritableObject[0].ActionScript);
                playerManager.phaser.CurrentPhase = PhaseType.Activate;
                BattleSystemManager.Instance.SelectTarget(playerManager.battler.SkillSet[AllocatedScritableObject[0].ActionScript].activateTarget);
                CameraManager.Instance.OnLiveCamera(CameraType.BattleCenter);
                CameraManager.Instance.GetCamera().Follow = playerManager.phaser.AllocatedPoint;
                CameraManager.Instance.GetCamera().GetComponent<CinemachineSplineDolly>().CameraPosition = 0.5f;
                Display(false);

                break;
            case 1:
                playerManager.battler.SetSkillExecution(AllocatedScritableObject[1].ActionScript);
                playerManager.phaser.CurrentPhase = PhaseType.Activate;
                BattleSystemManager.Instance.SelectTarget(playerManager.battler.SkillSet[AllocatedScritableObject[1].ActionScript].activateTarget);
                CameraManager.Instance.OnLiveCamera(CameraType.BattleCenter);
                CameraManager.Instance.GetCamera().Follow = playerManager.phaser.AllocatedPoint;
                CameraManager.Instance.GetCamera().GetComponent<CinemachineSplineDolly>().CameraPosition = 0.5f;
                Display(false);

                break;
            case 2:
                playerManager.battler.SetSkillExecution(AllocatedScritableObject[2].ActionScript);
                playerManager.phaser.CurrentPhase = PhaseType.Activate;
                BattleSystemManager.Instance.SelectTarget(playerManager.battler.SkillSet[AllocatedScritableObject[2].ActionScript].activateTarget);
                CameraManager.Instance.OnLiveCamera(CameraType.BattleCenter);
                CameraManager.Instance.GetCamera().Follow = playerManager.phaser.AllocatedPoint;
                CameraManager.Instance.GetCamera().GetComponent<CinemachineSplineDolly>().CameraPosition = 0.5f;
                Display(false);

                break;
        }

        return;
    }


    public override void CommandBack()
    {
        playerManager.phaser.PhaseCommand();
        playerManager.animator.animator.SetTrigger("MainCommand");
        return;
    }
}
