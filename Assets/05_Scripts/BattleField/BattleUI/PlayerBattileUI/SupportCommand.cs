using UnityEngine;

public class SupportCommand : BattleCommand
{
    public override void CommandBack()
    {
        playerManager.phaser.PhaseCommand();
        playerManager.animator.animator.SetTrigger("MainCommand");
        return;
    }
}
