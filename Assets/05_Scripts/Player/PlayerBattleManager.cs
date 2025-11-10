using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayerBattleManager : CharacterBattleManager
{
    [SerializeField] private PlayerManager playerManager;

    [SerializeField] public Dictionary<SkillGroup, List<SkillScriptableObject>> GroupSkills;

    [SerializeField] private PlayableDirector cutscenePlayer;
    [SerializeField] private TimelineAsset ultimatePlayerSkill;


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
        CurrentActivateTarget = SkillSet[skillAction].activateTarget;
    }

    public void SkillActivate(SkillActions skillAction)
    {
        // SkillSet[skillAction]
    }

    #region Skill Animation

    public void OnBackToPoint()
    {
        StartCoroutine(BackPointAction());
    }

    IEnumerator BackPointAction()
    {
        CameraManager.Instance.OnLiveCamera(CameraType.BattleCenter);
        yield return new WaitForSeconds(1.25f);

        Vector3 TargetAroundPos = playerManager.phaser.AllocatedPoint.position;
        TargetAroundPos.y = 0f;

        Vector3 PlayerPos = transform.position;
        PlayerPos.y = 0f;

        playerManager.animator.animator.Play("BackToPoint");
        yield return null;
        while (Vector3.Distance(TargetAroundPos, transform.position - new Vector3(0f, transform.position.y, 0f)) > 0.5f)
        {
            playerManager.locomotor.controller.Move((TargetAroundPos - PlayerPos).normalized * 6f * Time.deltaTime);
            yield return null;
        }

        Vector3 LookVec = BattleSystemManager.Instance.CenterPoints[1].position;
        LookVec.y = 0f;
        PlayerPos = transform.position;
        PlayerPos.y = 0f;
        transform.rotation = Quaternion.LookRotation(LookVec - PlayerPos);
        playerManager.animator.animator.Play("BattleIdle");
        CurrentTargetSkill.Done();
    }

    public void MoveToTarget(BattlePhase target, IEnumerator ActionEffect)
    {
        CameraManager.Instance.OnLiveCamera(CameraType.BattlePlayer);
        CinemachineCamera cam = CameraManager.Instance.GetCamera();
        cam.Follow = transform;
        CameraManager.Instance.PlayerAttackCameraAction();
        StartCoroutine(MoveToTargetAction(target, ActionEffect));
    }

    public void MoveToTarget(Transform target, IEnumerator ActionEffect)
    {
        CameraManager.Instance.OnLiveCamera(CameraType.BattlePlayer);
        CinemachineCamera cam = CameraManager.Instance.GetCamera();
        cam.Follow = transform;
        CameraManager.Instance.PlayerAttackCameraAction();
        StartCoroutine(MoveToTargetAction(target, ActionEffect));
    }

    IEnumerator MoveToTargetAction(BattlePhase target, IEnumerator ActionEffect)
    {
        
        Vector3 TargetAroundPos = target.transform.position + target.transform.forward * 1.5f;
        TargetAroundPos.y = 0f;

        Vector3 PlayerPos = transform.position;
        PlayerPos.y = 0f;

        transform.rotation = Quaternion.LookRotation(TargetAroundPos - PlayerPos);

        playerManager.animator.animator.Play("RushToPoint");

        yield return null;

        while (Vector3.Distance(TargetAroundPos, transform.position) > 1f)
        {
            playerManager.locomotor.controller.Move(Quaternion.Euler(TargetAroundPos - PlayerPos) * (TargetAroundPos - PlayerPos).normalized * 5f * Time.deltaTime);
            yield return null;
        }
        transform.rotation = Quaternion.LookRotation(new Vector3(0f, 0f, target.transform.position.z - PlayerPos.z));
        yield return StartCoroutine(ActionEffect);

    }

    IEnumerator MoveToTargetAction(Transform target, IEnumerator ActionEffect)
    {

        Vector3 TargetAroundPos = target.position;
        TargetAroundPos.y = 0f;

        Vector3 PlayerPos = transform.position;
        PlayerPos.y = 0f;

        transform.rotation = Quaternion.LookRotation(TargetAroundPos - PlayerPos);

        playerManager.animator.animator.Play("RushToPoint");

        yield return null;

        while (Vector3.Distance(TargetAroundPos, transform.position) > 1f)
        {
            playerManager.locomotor.controller.Move(Quaternion.Euler(TargetAroundPos - PlayerPos) * (TargetAroundPos - PlayerPos).normalized * 5f * Time.deltaTime);
            yield return null;
        }

        transform.rotation = Quaternion.LookRotation(new Vector3(0f, 0f, target.transform.position.z - PlayerPos.z));
        yield return StartCoroutine(ActionEffect);

    }


    public void OnBuff()
    { 
    
    }

    public void OnDeBuff()
    { 
        
    }

    public void OnSkillAction(ProcessType _React)
    {
        CurrentTargetSkill.Process(_React);
    }

    public void OnSkillAction(int _React)
    {
        CurrentTargetSkill.Process((ProcessType)_React);
    }

    public void OnQTEAction()
    {
        playerManager.animator.animator.SetBool("QTETrigger", true);
        CurrentTargetSkill.QTEAction();
    }

    public void OnSkillDone()
    {
        StartCoroutine(SKillDoneDelay());
    }

    IEnumerator SKillDoneDelay()
    {
        yield return new WaitForSeconds(1f);
        CurrentTargetSkill.Done();
    }

    #endregion


}