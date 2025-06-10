using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class EnemyBattleManager : CharacterBattleManager
{
    [SerializeField] private EnemyManager enemyManager;

    [SerializeField] public Dictionary<SkillGroup, List<SkillScriptableObject>> GroupSkills;
    public int CurrentRequireQTE;



    private void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
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
        CurrentTargetSkill = SkillSet[skillAction].GetSkillInstance(enemyManager);
        CurrentActivateTarget = SkillSet[skillAction].activateTarget;
        CurrentRequireQTE = SkillSet[skillAction].RequiredQTE;
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
        foreach (BattlePhase b in enemyManager.battler.CurrentTargets)
        {
            b.CurrentPhase = PhaseType.Wait;
        }

        CameraManager.Instance.OnLiveCamera(CameraType.BattleCenter);
        yield return new WaitForSeconds(1.25f);

        Vector3 TargetAroundPos = enemyManager.phaser.AllocatedPoint.position;
        TargetAroundPos.y = 0f;

        Vector3 PlayerPos = transform.position;
        PlayerPos.y = 0f;

        enemyManager.animator.animator.Play("BackToPoint");
        yield return null;
        while (Vector3.Distance(TargetAroundPos, transform.position - new Vector3(0f, transform.position.y, 0f)) > 0.5f)
        {
            enemyManager.locomotor.controller.Move((TargetAroundPos - PlayerPos).normalized * 10f * Time.deltaTime);
            yield return null;
        }

        Vector3 LookVec = BattleSystemManager.Instance.CenterPoints[1].position;
        LookVec.y = 0f;
        PlayerPos = transform.position;
        PlayerPos.y = 0f;
        transform.rotation = Quaternion.LookRotation(LookVec - PlayerPos);
        enemyManager.animator.animator.Play("Idle");
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

    IEnumerator MoveToTargetAction(BattlePhase target, IEnumerator ActionEffect)
    {

        Vector3 TargetAroundPos = target.transform.position + target.transform.forward * 1.5f;
        TargetAroundPos.y = 0f;

        Vector3 PlayerPos = transform.position;
        PlayerPos.y = 0f;

        transform.rotation = Quaternion.LookRotation(TargetAroundPos - PlayerPos);

        enemyManager.animator.animator.Play("RushToPoint");

        yield return null;

        while (Vector3.Distance(TargetAroundPos, transform.position - new Vector3(0f, transform.position.y, 0f)) > 0.5f)
        {
            Debug.Log(Vector3.Distance(TargetAroundPos, PlayerPos));
            enemyManager.locomotor.controller.Move(Quaternion.Euler(TargetAroundPos - PlayerPos) * (TargetAroundPos - PlayerPos).normalized * 5f * Time.deltaTime);
            yield return null;
        }

        target.CurrentPhase = PhaseType.Targetting;

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

    public void OnQTEAction()
    {
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