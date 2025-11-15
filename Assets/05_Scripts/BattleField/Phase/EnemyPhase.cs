using TeleportFX;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting.Antlr3.Runtime;

public class EnemyPhase : BattlePhase
{
    [Header("Engage")]

    [SerializeField] public bool isTurn;
    [SerializeField] public EnemyManager enemyManager;
    [SerializeField] public KriptoFX_Teleportation teleportation;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        teleportation = GetComponent<KriptoFX_Teleportation>();
    }

    public override void PhaseEngage()
    {
        GetComponent<CharacterController>().enabled = false;
        transform.position = SpawnPoint.position;

        teleportation.enabled = true;
        StartCoroutine(EngageAction());
    }

    IEnumerator EngageAction()
    {
        GetComponent<CharacterController>().enabled = true;
        enemyManager.animator.animator.Play("BattleEngage");
        
        enemyManager.locomotor.rigid.useGravity = false;
        Vector3 SpawnPos = new Vector3(SpawnPoint.position.x, 0f, SpawnPoint.position.z);
        Vector3 AllocatedPos = new Vector3(AllocatedPoint.position.x, 0f, AllocatedPoint.position.z);
        Vector3 direction = (AllocatedPos - transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;
        while (Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), AllocatedPos) > 0.35f)
        {

            enemyManager.locomotor.controller.Move(transform.forward * Time.deltaTime / 1.5f);
            yield return null;
        }

        enemyManager.animator.animator.SetBool("Engage", true);
        yield return null;

        CurrentPhase = PhaseType.Wait;
        
    }

    public void OnEngageActionDone()
    {
        enemyManager.animator.animator.SetBool("Engage", false);
        isEngage = true;
    }

    public override void PhaseCommand()
    {
        CameraManager.Instance.OnLiveCamera(CameraType.BattleEnemy);
        CinemachineCamera cam = CameraManager.Instance.GetCamera();
        cam.Follow = this.transform;

        PhaseJudgement();
    }

    public void ActivatedOff()
    {
        foreach (Transform t in BattleSystemManager.Instance.AllocatedPoints)
        {
            if (t.GetComponent<AllocatedTransform>().circleObject.gameObject.activeSelf)
            {
                t.GetComponent<AllocatedTransform>().circleObject.gameObject.SetActive(false);
            }
        }
    }
    private void PhaseJudgement()
    {
        // 공격 타입, 지원 타입, 턴 넘기기
        EventMessageManager.Instance.MessageQueueRegistry(new EventContainer() { eventType = ContextType.Battle, Context = $"{DisplayName} 의 차례" });

        int isAttackSkillHas = 0;
        int isSupportSkillHas = 0;
        int totalSkill = enemyManager.battler.OwnSkills.Count - 1; // Except TurnOver

        foreach (SkillScriptableObject skill in enemyManager.battler.OwnSkills)
        {
            if (skill.skillGroup == SkillGroup.Attack)
            {
                isAttackSkillHas++;
            }
            else if (skill.skillGroup == SkillGroup.Support)
            {
                isSupportSkillHas++;
            }
        }

        // Attack : Support 비중은 3:2 비율로 설정한다.
        float AttackRatio = (float)isAttackSkillHas  * 3f / ((float)isAttackSkillHas * 3f + (float)isSupportSkillHas * 2f);
        float SupportRatio = (float)isSupportSkillHas * 2f / ((float)isAttackSkillHas * 3f + (float)isSupportSkillHas * 2f);
        float turnOverRatio = 0.05f;

        if (UnityEngine.Random.Range(0f, 1f) < turnOverRatio)
        {
            // TargetSkill = TurnOver
            BattleSystemManager.Instance.TargetSelf();
            enemyManager.battler.CurrentTargets = BattleSystemManager.Instance.TempActivateTargets;
            enemyManager.battler.CurrentTargetSkill = enemyManager.battler.SkillSet[SkillActions.TurnOver].GetSkillInstance(enemyManager);

            // skill execute

            enemyManager.battler.CurrentTargetSkill.Execute();
            CurrentPhase = PhaseType.Execute;
            return;
        }
        float SkillToss = UnityEngine.Random.Range(0f, 1f);

        // TempActivated 가 결정되었고, 현재 스킬이 결정되었다.
        // 타겟은 누구로 할 것인가?
        ActivateTarget targetType;
        if (SkillToss < SupportRatio)
        {
            List<SkillScriptableObject> supportSkills = enemyManager.battler.GroupSkills[SkillGroup.Support];
            int tossSkill = UnityEngine.Random.Range(0, supportSkills.Count);
            enemyManager.battler.CurrentTargetSkill = enemyManager.battler.SkillSet[supportSkills[tossSkill].ActionScript].GetSkillInstance(enemyManager);
            BattleSystemManager.Instance.SelectTarget(supportSkills[tossSkill].activateTarget);
            targetType = supportSkills[tossSkill].activateTarget;
        }
        else
        {
            List<SkillScriptableObject> attackSkills = enemyManager.battler.GroupSkills[SkillGroup.Attack];
            int tossSkill = UnityEngine.Random.Range(0, attackSkills.Count);
            enemyManager.battler.CurrentTargetSkill = enemyManager.battler.SkillSet[attackSkills[tossSkill].ActionScript].GetSkillInstance(enemyManager);
            BattleSystemManager.Instance.SelectTarget(attackSkills[tossSkill].activateTarget);
            targetType = attackSkills[tossSkill].activateTarget;
        }

        // 타겟들 중에서 누구를 선택할 것인가?
        // Enemy (적들 입장에서 아군) : 지원 기술
        // Ally (플레이어) : 공격 기술
        switch (targetType)
        {
            // 타겟을 선택할 필요가 없으면, 바로 실행
            case ActivateTarget.None:
            case ActivateTarget.Self:
            case ActivateTarget.ALLEnemy:
            case ActivateTarget.ALLAlly:
            case ActivateTarget.ALL:
                enemyManager.battler.CurrentTargets = BattleSystemManager.Instance.TempActivateTargets;
                break;


            // 버프 기술
            case ActivateTarget.TargettingEnemy:
                BuffTargetSelection();

                break;
            // 공격 기술

            case ActivateTarget.TargettingAlly:
                AttackTargetSelection();
                break;
        }

        Debug.Log("Execute!!");
        StartCoroutine(ExecutionEffect());
    }

    IEnumerator ExecutionEffect()
    {
        yield return new WaitForSeconds(1f);
        ActivatedOff();
        enemyManager.battler.CurrentTargetSkill.Execute();
        CurrentPhase = PhaseType.Execute;
    }

    private void BuffTargetSelection()
    {
        List<BattlePhase> Targets = BattleSystemManager.Instance.TempActivateTargets;
        int TargetCount = Targets.Count;

        // 15% 확률로 랜덤 토싱
        float TossingLuck = 0.15f;
        List<BattlePhase> Selected = new List<BattlePhase>();
        if (UnityEngine.Random.Range(0f, 1f) < TossingLuck)
        {
            Selected.Add(Targets[UnityEngine.Random.Range(0, TargetCount)]);
            enemyManager.battler.CurrentTargets = Selected;
            return;
        }

        // HP : 5
        // DEF : 5

        int idx = 0;
        int maxIndex = 0;
        float maxValue = -1f;
        foreach (BattlePhase b in Targets)
        {
            EnemyStatusManager enemyStat = ((EnemyPhase)b).enemyManager.status;
            float HPRatio = 1f - (float)enemyStat.HP / (float)enemyStat.aMaxHP;
            float DEFRatio = enemyStat.aDefenceWeight / 100f;

            if (maxValue < HPRatio + DEFRatio)
            {
                maxIndex = idx;
            }
            idx++;
        }

        Selected.Add(Targets[maxIndex]);
        enemyManager.battler.CurrentTargets = Selected;
        return;

    }

    private void AttackTargetSelection()
    {
        List<BattlePhase> Targets = BattleSystemManager.Instance.TempActivateTargets;
        int TargetCount = Targets.Count;

        // 70% 확률로 랜덤 토싱
        float TossingLuck = 0.7f;
        List<BattlePhase> Selected = new List<BattlePhase>();
        if (UnityEngine.Random.Range(0f, 1f) < TossingLuck)
        {
            BattlePhase battlePhase = Targets[UnityEngine.Random.Range(0, TargetCount)];
            battlePhase.AllocatedPoint.GetComponent<AllocatedTransform>().circleObject.gameObject.SetActive(true);
            Selected.Add(battlePhase);
            enemyManager.battler.CurrentTargets = Selected;
            
            return;
        }

        // HP : 5
        // DEF : 5

        int idx = 0;
        int maxIndex = 0;
        float maxValue = -1f;
        foreach (BattlePhase b in Targets)
        {
            PlayerStatusManager enemyStat = ((PlayerPhase)b).playerManager.status;
            float HPRatio = (float)enemyStat.HP / (float)enemyStat.aMaxHP;
            float DEFRatio = 1f - enemyStat.aDefenceWeight / 100f;

            if (maxValue < HPRatio + DEFRatio)
            {
                maxIndex = idx;
            }

            if (enemyManager.status.aMaxATK >= enemyStat.HP)
            {
                maxIndex = idx;
                break;
            }
            idx++;
        }

        Selected.Add(Targets[maxIndex]);
        enemyManager.battler.CurrentTargets = Selected;
        return;
    }



}
