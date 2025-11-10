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
    public bool APMustSatisfied;
    public bool isConsumeRequire;

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
            case SkillActions.KnightSpecial:
                return new KnightSpecial(playerManager);
            case SkillActions.DualBladeCombo01:
                return new DualBladeCombo01(playerManager);
            case SkillActions.DualBladeCombo02:
                return new DualBladeCombo02(playerManager);
            case SkillActions.DualBladeSpecial:
                return new DualBladeSpecial(playerManager);
            case SkillActions.MagicianCombo01:
                return new MagicianCombo01(playerManager);
            case SkillActions.MagicianCombo02:
                return new MagicianCombo02(playerManager);
            case SkillActions.MagicianSpecial:
                return new MagicianSpecial(playerManager);
            case SkillActions.KnightBuff:
                return new KnightBuff(playerManager);
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
                return new SlimbCombo01(enemyManager, RequiredQTE);
            case SkillActions.SlimbCombo02:
                return new SlimbCombo02(enemyManager);
            case SkillActions.SkeletonCombo01:
                return new SkeletonCombo01(enemyManager, RequiredQTE);
            case SkillActions.SkeletonCombo02:
                return new SkeletonCombo02(enemyManager, RequiredQTE);
            case SkillActions.SkeletonCombo03:
                return new SkeletonCombo03(enemyManager, RequiredQTE);
            case SkillActions.KnightCombo01:
                return new KnightCombo01(enemyManager, RequiredQTE);
            case SkillActions.KnightCombo02:
                return new KnightCombo02(enemyManager, RequiredQTE);
            case SkillActions.KnightCombo03:
                return new KnightCombo03(enemyManager, RequiredQTE);
            case SkillActions.KnightCombo04:
                return new KnightCombo04(enemyManager, RequiredQTE);
            case SkillActions.DragonCombo01:
                return new DragonCombo01(enemyManager, RequiredQTE);
            case SkillActions.DragonCombo02:
                return new DragonCombo02(enemyManager, RequiredQTE);
            case SkillActions.DragonCombo03:
                return new DragonCombo03(enemyManager, RequiredQTE);
            case SkillActions.DragonCombo04:
                return new DragonCombo04(enemyManager);
            case SkillActions.DragonCombo05:
                return new DragonCombo05(enemyManager);
        }

    }
}