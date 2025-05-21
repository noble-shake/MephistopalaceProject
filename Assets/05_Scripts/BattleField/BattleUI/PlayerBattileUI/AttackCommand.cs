using UnityEngine;
using UnityEngine.UI;

public class AttackCommand : BattleCommand
{

    public override void CommandBack()
    {
        playerManager.phaser.PhaseCommand();
        playerManager.animator.animator.SetTrigger("MainCommand");
        return;
    }
}
