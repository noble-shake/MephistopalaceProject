using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.TextCore.Text;

public class PlayerLocomotionManager : MonoBehaviour
{
    [HideInInspector] private PlayerManager playerManager;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public Rigidbody rigid;
    

    [SerializeField] float CharacterSpeed;
    [SerializeField] float RotationSpeed;
    [SerializeField] float RotationLimit;
    [SerializeField] Vector3 GravityVector = Physics.gravity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        rigid = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
    }

    public Vector2 GetMoveVector()
    {
        return InputManager.Instance.MoveInput;
    }

    public Vector2 GetLookVector()
    {
        return InputManager.Instance.CameraInput;
    }

    public void GamePause()
    {


        if (InputManager.Instance.EscapeInput)
        {
            playerManager.isPause = true;
            StartCoroutine(pauseGame(PauseCanvas.Instance.CurrentPanelType));
        }
        else if (InputManager.Instance.InventoryInput)
        {
            playerManager.isPause = true;
            StartCoroutine(pauseGame(PanelType.INVENTORY));
        }
        else if (InputManager.Instance.StatusInput)
        {
            playerManager.isPause = true;
            StartCoroutine(pauseGame(PanelType.STATUS));
        }
    }

    IEnumerator pauseGame(PanelType panel)
    {
        GameManager.Instance.GameStop(0.5f);
        PauseCanvas.Instance.gameObject.SetActive(true);
        PauseCanvas.Instance.OpenMenu(panel);
        Cursor.lockState = CursorLockMode.None;
        InputManager.Instance.UIInputBind();
        yield return null;

    }

    public void CharacterMove()
    {
        if (controller.enabled == false) return;

        Vector2 MoveVector = GetMoveVector();
        Vector3 adjustRotate = transform.TransformDirection(new Vector3(MoveVector.x, 0f, MoveVector.y));
        controller.Move(adjustRotate * CharacterSpeed * Time.deltaTime);

        if (!playerManager.isGravityOff) controller.Move(GravityVector * Time.deltaTime);
    }

    public void CharacterLook()
    {


        Vector2 LookVector = GetLookVector();
        if (Mathf.Abs(LookVector.x) < RotationLimit) return;

        Vector3 RotationTerm =  new Vector3(0f, LookVector.x * RotationSpeed * GameManager.Instance.Sensivity * Time.deltaTime, 0f);
        this.transform.Rotate(RotationTerm);

    }

    public void CharacterSwitchingCheck()
    {
        if (InputManager.Instance.PreviousInput)
        {
            InputManager.Instance.PreviousInput = false;
            PlayerCharacterManager.Instance.CharacterSwitching(SwitchingDirection.Previous);
        }

        if (InputManager.Instance.NextInput)
        {
            InputManager.Instance.NextInput = false;
            PlayerCharacterManager.Instance.CharacterSwitching(SwitchingDirection.Next);
        }
    }

    public void OnJump()
    {
        playerManager.isJump = true;

    }

    public void OnLanding()
    {
        playerManager.isJump = false;
    }

    public void CharacterJump(Transform From, Transform To)
    {
        if (playerManager.isJump == true) return;
        if (playerManager.characterType != CharacterType.DualBlade) return;
        OnJump();
        StartCoroutine(JumpAction(From, To));
    }

    public IEnumerator JumpAction(Transform FromTrs, Transform ToTrs)
    {
        playerManager.animator.animator.Play("EncounterJump");
        Vector3 SpawnPos = new Vector3(FromTrs.position.x, 0f, FromTrs.position.z);
        Vector3 AllocatedPos = new Vector3(ToTrs.position.x, 0f, ToTrs.position.z);
        playerManager.locomotor.rigid.useGravity = false;
        Vector3 direction = (AllocatedPos - transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;
        while (Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), AllocatedPos) > 0.1f)
        {
            playerManager.locomotor.controller.Move(transform.forward * Time.deltaTime / 1.5f);
            yield return null;
        }

        playerManager.animator.animator.SetBool("JumpTrigger", true);


        yield return null;
        OnLanding();
    }


    


    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameModeState.Encounter) return;

        if (playerManager.isAttack) return;
        if (playerManager.isPause) return;
        if (playerManager.isJump) return;
        if (playerManager.isItemEarnAction) return;
        GamePause();
        CharacterMove();
        CharacterLook();
        CharacterSwitchingCheck();

    }
}
