using System.Collections.Generic;
using UnityEngine;

public class SupportCommand : BattleCommand
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
    public override void CommandBack()
    {
        playerManager.phaser.PhaseCommand();
        playerManager.animator.animator.SetTrigger("MainCommand");
        return;
    }
}
