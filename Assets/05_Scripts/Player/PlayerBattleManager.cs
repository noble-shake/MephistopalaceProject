using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerBattleManager : CharacterBattleManager
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] public List<SkillScriptableObject> OwnSkills;
    [SerializeField] public List<SkillScriptableObject> ActivatedSkills;
    [SerializeField] public Dictionary<SkillGroup, List<SkillScriptableObject>> GroupSkills;
    [SerializeField] public Dictionary<SkillActions, SkillScriptableObject> SkillSet;

    [SerializeField] public ISkill CurrentTargetSkill;
    [SerializeField] public List<BattlePhase> CurrentTargets;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        GroupSkills = new Dictionary<SkillGroup, List<SkillScriptableObject>>();
        SkillSet = new Dictionary<SkillActions, SkillScriptableObject>();

        if (!GroupSkills.ContainsKey(SkillGroup.None)) GroupSkills[SkillGroup.None] = new List<SkillScriptableObject>();
        if (!GroupSkills.ContainsKey(SkillGroup.Attack)) GroupSkills[SkillGroup.Attack] = new List<SkillScriptableObject>();
        if (!GroupSkills.ContainsKey(SkillGroup.Support)) GroupSkills[SkillGroup.Support] = new List<SkillScriptableObject>();

        SkillGroupInit();
    }

    public void SkillGroupInit()
    {
        foreach (SkillScriptableObject skill in OwnSkills)
        {
            SkillSet[skill.ActionScript] = skill;
            if (!ActivatedSkills.Contains(skill)) continue;

            switch (skill.skillGroup)
            { 
                case SkillGroup.None:
                    if (!GroupSkills.ContainsKey(SkillGroup.None)) GroupSkills[SkillGroup.None] = new List<SkillScriptableObject>();
                    GroupSkills[SkillGroup.None].Add(skill);
                    break;
                case SkillGroup.Attack:
                    if (!GroupSkills.ContainsKey(SkillGroup.Attack)) GroupSkills[SkillGroup.Attack] = new List<SkillScriptableObject>();
                    GroupSkills[SkillGroup.Attack].Add(skill);
                    break;
                case SkillGroup.Support:
                    if (!GroupSkills.ContainsKey(SkillGroup.Support)) GroupSkills[SkillGroup.Support] = new List<SkillScriptableObject>();
                    GroupSkills[SkillGroup.Support].Add(skill);
                    break;
            }

        }

    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState.Equals(GameModeState.Encounter)) return;
    }

    public void SetSkillExecution(SkillActions skillAction)
    {
        if (SkillSet.ContainsKey(skillAction) == false) return;
        CurrentTargetSkill = SkillSet[skillAction].GetSkillInstance(playerManager);

    }

    public void SetTargetActivate(List<BattlePhase> Targets)
    {
        CurrentTargets = Targets;
    }

    public void SkillActivate(SkillActions skillAction)
    {
        // SkillSet[skillAction]
    }

    #region Skill Animation

    public void OnBuff()
    { 
    
    }

    public void OnDeBuff()
    { 
        
    }

    #endregion


}