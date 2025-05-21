public class TurnOver : ISkill
{
    PlayerManager playerManager;
    EnemyManager enemyManager;

    bool isPlayer;
    bool isSkillDone;

    public TurnOver(PlayerManager playerManager) 
    {
        this.playerManager = playerManager;
        isSkillDone = false;
        isPlayer = true;
    }

    public TurnOver(EnemyManager enemyManager)
    {
        this.enemyManager = enemyManager;
        isSkillDone = false;
        isPlayer = false;
    }

    public void Execute()
    {
        if (isPlayer)
        {
            playerManager.status.HPChange((int)(playerManager.status.MaxHP * 0.1f));
            playerManager.status.GainAP(1);
            isSkillDone = true;
        }
        else
        {
            enemyManager.status.HPChange((int)(playerManager.status.MaxHP * 0.1f));
            // enemyManager.status.GainAP(1);
            isSkillDone = true;
        }

    }
}