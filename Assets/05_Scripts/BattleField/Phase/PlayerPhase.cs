using System.Collections;
using TeleportFX;
using UnityEngine;

public class PlayerPhase : BattlePhase
{

    [SerializeField] public bool isTurn;
    [SerializeField] public PlayerManager playerManager;
    [SerializeField] private KriptoFX_Teleportation teleportation;
    [SerializeField] private BattleCommand AllocatedPanel;

    [HideInInspector] public int CurrentTargetIndex;
    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        teleportation = GetComponent<KriptoFX_Teleportation>();
    }

    public bool EngageTest;
    public override void PhaseEngage()
    {
        teleportation.enabled = true;
        StartCoroutine(EngageAction());
    }

    IEnumerator EngageAction()
    {
        yield return new WaitForSeconds(0.2f);
        if (!playerManager.locomotor.controller.enabled) playerManager.locomotor.controller.enabled = true;
        playerManager.animator.animator.Play("BattleEngage");
        Vector3 SpawnPos = new Vector3(SpawnPoint.position.x, 0f, SpawnPoint.position.z);
        Vector3 AllocatedPos = new Vector3(AllocatedPoint.position.x, 0f, AllocatedPoint.position.z);
        playerManager.locomotor.rigid.useGravity = false;
        Vector3 direction = (AllocatedPos - transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation =lookRotation;
        while (Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), AllocatedPos) > 0.1f)
        {
            playerManager.locomotor.controller.Move(transform.forward * Time.deltaTime / 1.5f);
            yield return null;
        }

        playerManager.animator.animator.SetBool("Engage", true);
        yield return null;

        CurrentPhase = PhaseType.Wait;
        

    }

    public void OnEngageActionDone()
    {
        playerManager.animator.animator.SetBool("Engage", false);
        isEngage = true;
    }

    public void OnDisEngageActionDone()
    {
        playerManager.animator.animator.SetTrigger("DisEngage");
        isEngage = false;
    }



    public override void PhaseBuff()
    {

    }

    public override void PhaseDbuff()
    {

    }

    public override void PhaseExecute()
    {

    }
    public override void PhaseDone()
    {

    }

    private void Update()
    {
        if (EngageTest)
        {
            EngageTest = false;
            PhaseEngage();

        }

        if (GameManager.Instance.CurrentState != GameModeState.Battle) return;
        PhaseUpdate();

    }

    private void PhaseUpdate()
    {
        switch (CurrentPhase)
        {
            case PhaseType.Engage:
                break;
            case PhaseType.Command:
            case PhaseType.Attack:
            case PhaseType.Support:
                CommandUpdate();
                break;
            case PhaseType.Activate:
                ActivateUpdate();
                break;
            case PhaseType.Execute:
            case PhaseType.Done:
            case PhaseType.Wait:
                break;
            case PhaseType.Targetting:
                ParryDurationUpdate();
                ParryActionUpdate();
                break;
        }
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

    // this phase transition is executed in Engage Stage.
    public override void PhaseCommand()
    {
        ActivatedOff();

        CurrentTargetIndex = 0;
        // Display Pannel
        if (AllocatedPanel != null && AllocatedPanel != AllocatedPoint.GetComponentInChildren<CommandMainPanel>()) AllocatedPanel.Display(false);
        CurrentPhase = PhaseType.Command;
        AllocatedPanel = AllocatedPoint.GetComponentInChildren<CommandMainPanel>();
        AllocatedPanel.SetCommand(playerManager, playerManager.battler.ActivatedSkills);
        AllocatedPanel.Display(true);
        // CameraWork
        CameraManager.Instance.OnLiveCamera(AllocatedPoint.GetComponentInChildren<CameraTag>().type);
        CameraManager.Instance.CommandPosChange(SkillGroup.None);

        EventMessageManager.Instance.MessageQueueRegistry(new EventContainer() { eventType = ContextType.Battle, Context = $"{DisplayName} ÀÇ Â÷·Ê" });
    }

    public override void AttackCommand()
    {
        CurrentTargetIndex = 0;
        // Display Pannel
        if (AllocatedPanel != null && AllocatedPanel != AllocatedPoint.GetComponentInChildren<AttackCommand>()) AllocatedPanel.Display(false);
        CurrentPhase = PhaseType.Attack;
        AllocatedPanel = AllocatedPoint.GetComponentInChildren<AttackCommand>();
        AllocatedPanel.SetCommand(playerManager, playerManager.battler.GroupSkills[SkillGroup.Attack]);
        AllocatedPanel.Display(true);
        // Display Update

        // CameraWork
        CameraManager.Instance.OnLiveCamera(AllocatedPoint.GetComponentInChildren<CameraTag>().type);
        CameraManager.Instance.CommandPosChange(SkillGroup.Attack);

        // AnimationWork
        playerManager.animator.animator.SetTrigger("AttackCommand");
    }

    public override void SupportCommand()
    {
        CurrentTargetIndex = 0;
        // Display Pannel
        if (AllocatedPanel != null && AllocatedPanel != AllocatedPoint.GetComponentInChildren<SupportCommand>()) AllocatedPanel.Display(false);
        CurrentPhase = PhaseType.Support;
        AllocatedPanel = AllocatedPoint.GetComponentInChildren<SupportCommand>();
        AllocatedPanel.SetCommand(playerManager, playerManager.battler.GroupSkills[SkillGroup.Support]);
        AllocatedPanel.Display(true);
        // CameraWork
        CameraManager.Instance.OnLiveCamera(AllocatedPoint.GetComponentInChildren<CameraTag>().type);
        CameraManager.Instance.CommandPosChange(SkillGroup.Support);

        // AnimationWork
        playerManager.animator.animator.SetTrigger("SupportCommand");
    }

    [HideInInspector] private float curDelay;
    private void CommandUpdate()
    {
        curDelay -= Time.deltaTime;
        if(curDelay < 0f) curDelay = 0f;
        if (curDelay > 0f) return;
        
        Vector2 MoveInput = InputManager.Instance.MoveInput;
        bool ExecuteInput = InputManager.Instance.AttackInput;
        bool BackInput = InputManager.Instance.CrouchInput;

        if (MoveInput.y != 0)
        {
            curDelay = 0.3f;
            if (MoveInput.y < 0)
            {
                MoveInput.y = 0;
                AllocatedPanel.Next();
            }
            else
            {
                MoveInput.y = 0;
                AllocatedPanel.Previous();
            }
        }

        if (MoveInput.x != 0)
        {
            curDelay = 0.3f;
            if (MoveInput.x < 0)
            {
                MoveInput.x = 0;
                AllocatedPanel.NextPage();
            }
            else
            {
                MoveInput.x = 0;
                AllocatedPanel.PreviousPage();
            }

        }

        if (ExecuteInput)
        {
            Debug.Log("Execute Input");
            curDelay = 0.3f;
            ExecuteInput = false;
            AllocatedPanel.CommandExecute();
        }

        if (BackInput)
        {
            Debug.Log("Back Input");
            curDelay = 0.3f;
            BackInput = false;
            AllocatedPanel.CommandBack();
            ActivatedOff();
        }

    }

    private void ActivateUpdate()
    {
        if (BattleSystemManager.Instance.TempActivateTargets == null)
        {
            Debug.Log("Activate is null");
            return;
        }

        curDelay -= Time.deltaTime;
        if (curDelay < 0f) curDelay = 0f;
        if (curDelay > 0f) return;
        Vector2 MoveInput = InputManager.Instance.MoveInput;
        bool ExecuteInput = InputManager.Instance.AttackInput;
        bool BackInput = InputManager.Instance.CrouchInput;

        if (BattleSystemManager.Instance.TempActivateTargets.Count != 1 && playerManager.battler.CurrentActivateTarget == ActivateTarget.TargettingEnemy || playerManager.battler.CurrentActivateTarget == ActivateTarget.TargettingAlly)
        {
            if (MoveInput.x != 0)
            {
                curDelay = 0.3f;
                ActivatedOff();


                if (MoveInput.x < 0)
                {
                    // playerManager.battler.CurrentTargets
                    CurrentTargetIndex--;
                    if (CurrentTargetIndex < 0)
                    {
                        CurrentTargetIndex = BattleSystemManager.Instance.TempActivateTargets.Count - 1;
                    }
                    BattleSystemManager.Instance.TempActivateTargets[CurrentTargetIndex].AllocatedPoint.GetComponent<AllocatedTransform>().circleObject.gameObject.SetActive(true);
                    transform.rotation = Quaternion.Euler(BattleSystemManager.Instance.TempActivateTargets[CurrentTargetIndex].transform.position - transform.position);
                }
                else
                {
                    CurrentTargetIndex++;
                    if (CurrentTargetIndex > BattleSystemManager.Instance.TempActivateTargets.Count - 1)
                    {
                        CurrentTargetIndex = 0;
                    }
                    BattleSystemManager.Instance.TempActivateTargets[CurrentTargetIndex].AllocatedPoint.GetComponent<AllocatedTransform>().circleObject.gameObject.SetActive(true);
                    transform.rotation = Quaternion.Euler(BattleSystemManager.Instance.TempActivateTargets[CurrentTargetIndex].transform.position - transform.position);
                }
            }
        }


        if (ExecuteInput)
        {
            curDelay = 0.3f;
            ExecuteInput = false;
            ActivatedOff();
            // skill execute and Player will move.
            if (playerManager.battler.CurrentActivateTarget == ActivateTarget.TargettingEnemy || playerManager.battler.CurrentActivateTarget == ActivateTarget.TargettingAlly)
            {
                playerManager.battler.CurrentTargets = BattleSystemManager.Instance.TempActivateTargets.GetRange(CurrentTargetIndex, 1);
                playerManager.battler.CurrentTargetSkill.Execute();
                CurrentPhase = PhaseType.Execute;

            }
            else
            {
                playerManager.battler.CurrentTargets = BattleSystemManager.Instance.TempActivateTargets;
                playerManager.battler.CurrentTargetSkill.Execute();
                CurrentPhase = PhaseType.Execute;
            }

        }

        if (BackInput)
        {
            curDelay = 0.3f;
            BackInput = false;
            CurrentPhase = PhaseType.Command;
            PhaseCommand();
            playerManager.animator.animator.SetTrigger("MainCommand");
            // AllocatedPanel.CommandBack();

            // back.. skill execute and Player will move.
            ActivatedOff();
        }
    }

    public override void OnQTEDone()
    {
        playerManager.animator.animator.SetTrigger("QTETrigger");
    }

    [SerializeField] private float ParryCooltime;
    [SerializeField] private float EvadeCooltime;
    [SerializeField] private float CurrentCooltime;
    [SerializeField] public bool isParrying;
    [SerializeField] public bool isEvading;
    [SerializeField] private float QTEduration;
    public void ParryDurationUpdate()
    {
        if (!isParrying && !isEvading) return;

        QTEduration -= Time.deltaTime;
        if (QTEduration < 0f)
        {
            QTEduration = 0f;
            isParrying = false;
            isEvading = false;
        } 

    }


    public void ParryActionUpdate()
    {
        bool ExecuteInput = InputManager.Instance.AttackInput;
        bool BackInput = InputManager.Instance.CrouchInput;

        CurrentCooltime -= Time.deltaTime;
        if (CurrentCooltime < 0f)
        {
            CurrentCooltime = 0f;
        }

        if (CurrentCooltime > 0f) return;

        if (ExecuteInput)
        {
            ExecuteInput = false;
            CurrentCooltime = ParryCooltime;
            isParrying = true;
            playerManager.animator.animator.Play("BattleParry");
            QTEduration = 0.6f;
        }

        if (BackInput)
        {
            BackInput = false;
            CurrentCooltime = EvadeCooltime;
            isEvading = true;
            playerManager.animator.animator.Play("BattleEvade");
            QTEduration = 0.6f;
        }

    }

    public void ParrySuccess(bool isOn)
    {
        Debug.Log("PARRY SUCCESS !!");
        if (isOn)
        {
            playerManager.animator.animator.Play("BattleParrySuccess");
            CurrentCooltime = 0.002f;
            GameManager.Instance.GamePause();
        }
        else
        {
            CurrentCooltime = ParryCooltime;
        }
    }

    public void OnWeaponVFX()
    { 
        playerManager.encounter.Weapon.VFXLight.SetActive(true);
        playerManager.encounter.Weapon.CreateWeaponVFX();
    }

    public void OffWeaponVFX()
    {
        playerManager.encounter.Weapon.VFXLight.SetActive(false);
    }

    public void EvadeSuccess(bool isOn)
    {
        Debug.Log("PARRY SUCCESS !!");
        if (isOn)
        {
            playerManager.status.GainAP(1);
            CurrentCooltime = 0.002f;
            GameManager.Instance.GamePause();
        }
        else
        {
            CurrentCooltime = EvadeCooltime;
        }
    }

}