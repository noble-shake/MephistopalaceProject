using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;


public class CommandMainPanel: BattleCommand
{
    [SerializeField] private CinemachineCamera PoseCamera;
    [SerializeField] private CommandFrame AttackGroupCommand;
    [SerializeField] private CommandFrame SupportGroupCommand;
    [SerializeField] private CommandFrame TurnOverCommand;

    public override void SetCommand(PlayerManager player, List<SkillScriptableObject> Skills)
    {
        foreach (CommandFrame frame in DisplayedList)
        {
            frame.OnHighlight(false);
        }

        CurrentCommand = DisplayedList[0];
        playerManager = player;
        TotalSkills = 3;
        CurrentIndex = 0;
        CurrentPage = 0;
        DisplayedList[0].OnHighlight(true);

        TotalIndex = TotalSkills - 1;
        TotalPage = 1;
        // AllocatedScritableObject = OwnScritableObject.GetRange(0, TotalSkills);
    }


    public override void CommandExecute()
    {
        switch (CurrentIndex)
        {
            default:
                break;
            case 0:
                // Display to Attack

                playerManager.phaser.AttackCommand();
                break;
            case 1:
                // Display to Support
                playerManager.phaser.SupportCommand();
                break;
            case 2:
                // Turn Over
                Debug.Log("Turn Over Execute");
                playerManager.battler.SetSkillExecution(SkillActions.TurnOver);
                playerManager.phaser.CurrentPhase = PhaseType.Activate;
                BattleSystemManager.Instance.SelectTarget(playerManager.battler.SkillSet[SkillActions.TurnOver].activateTarget);
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
        return;
    }
}