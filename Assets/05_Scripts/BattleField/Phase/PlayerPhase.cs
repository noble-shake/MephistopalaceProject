using System.Collections;
using TeleportFX;
using UnityEngine;

public class PlayerPhase : BattlePhase
{

    [SerializeField] public bool isTurn;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private KriptoFX_Teleportation teleportation;
    [SerializeField] private BattleCommand AllocatedPanel;

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
                break;
            case PhaseType.Execute:
                break;
            case PhaseType.Done:
                break;
            case PhaseType.Wait:
                break;
        }
    }

    // this phase transition is executed in Engage Stage.
    public override void PhaseCommand()
    {
        // Display Pannel
        if (AllocatedPanel != null && AllocatedPanel != AllocatedPoint.GetComponentInChildren<CommandMainPanel>()) AllocatedPanel.Display(false);
        CurrentPhase = PhaseType.Command;
        AllocatedPanel = AllocatedPoint.GetComponentInChildren<CommandMainPanel>();
        AllocatedPanel.SetCommand(playerManager, playerManager.battler.ActivatedSkills);
        AllocatedPanel.Display(true);
        // CameraWork
        CameraManager.Instance.OnLiveCamera(AllocatedPoint.GetComponentInChildren<CameraTag>().type);
        CameraManager.Instance.CommandPosChange(SkillGroup.None);
    }

    public override void AttackCommand()
    {
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
        }

    }

    private void ActivateUpdate()
    {
        curDelay -= Time.deltaTime;
        if (curDelay < 0f) curDelay = 0f;
        if (curDelay > 0f) return;
        Vector2 MoveInput = InputManager.Instance.MoveInput;
        bool ExecuteInput = InputManager.Instance.AttackInput;
        bool BackInput = InputManager.Instance.CrouchInput;

        if (MoveInput.x != 0)
        {
            curDelay = 0.3f;
            if (MoveInput.x < 0)
            {
                // AllocatedPanel.PreviousPage();
            }
            else
            {
                // AllocatedPanel.NextPage();
            }
        }

        if (ExecuteInput)
        {
            curDelay = 0.3f;
            ExecuteInput = false;
            // AllocatedPanel.CommandExecute();

            // skill execute and Player will move.
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
        }
    }
}