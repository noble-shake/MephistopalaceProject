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
        }

    }
}