using UnityEngine;

[System.Serializable]
public struct SkillContainer
{
    public SkillGroup skillGroup;
    public ActivateTarget activateTarget;
    public string Name;
    public string Description;
    public int RequiredQTE;
    public Sprite SkillIcon;
    public float DamageRatio;
    public SkillActions ActionScript;
}

[CreateAssetMenu(fileName = "Skill 1", menuName = "PalaceSkill/SkillScritableObject")]
public class SkillScriptableObject : ScriptableObject
{
    public SkillGroup skillGroup;
    public ActivateTarget activateTarget;
    public string Name;
    public string Description;
    public int RequiredQTE;
    public Sprite SkillIcon;
    public float DamageRatio;
    public SkillActions ActionScript;
    public int RequiredAP;

    public SkillContainer GetSkillInfo()
    { 
        return new SkillContainer 
        {
            skillGroup = skillGroup,
            activateTarget = activateTarget,
            Name = Name,
            Description = Description,
            RequiredQTE = RequiredQTE,
            SkillIcon = SkillIcon,
            DamageRatio = DamageRatio,
            ActionScript = ActionScript,
        };
    }

    public ISkill GetSkillInstance(PlayerManager playerManager)
    {
        switch (ActionScript)
        {
            default:
            case SkillActions.TurnOver:
                return new TurnOver(playerManager);
            case SkillActions.DoubleSlash:
                return new DoubleSlash(playerManager);
            case SkillActions.ChargingAttack:
                return new ChargingAttack(playerManager);
        }

    }

    public ISkill GetSkillInstance(EnemyManager enemyManager)
    {
        switch (ActionScript)
        {
            default:
            case SkillActions.TurnOver:
                return new TurnOver(enemyManager);
            case SkillActions.DoubleSlash:
                return new DoubleSlash(enemyManager);
            case SkillActions.SlimbCombo01:
                return new SlimbCombo01(enemyManager);
            case SkillActions.SlimbCombo02:
                return new SlimbCombo02(enemyManager);
            case SkillActions.SkeletonCombo01:
                return new SkeletonCombo01(enemyManager);
            case SkillActions.SkeletonCombo02:
                return new SkeletonCombo02(enemyManager);
            case SkillActions.SkeletonCombo03:
                return new SkeletonCombo03(enemyManager);
        }

    }
}