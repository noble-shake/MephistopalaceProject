using System.Collections.Generic;
using UnityEngine;

public class CharacterBattleManager : MonoBehaviour
{
    [SerializeField] public List<SkillScriptableObject> OwnSkills;
    [SerializeField] public List<SkillScriptableObject> ActivatedSkills;
    [SerializeField] public Dictionary<SkillActions, SkillScriptableObject> SkillSet;

    [SerializeField] public ISkill CurrentTargetSkill;
    [SerializeField] public ActivateTarget CurrentActivateTarget;
    [SerializeField] public List<BattlePhase> CurrentTargets;
}