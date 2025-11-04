using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderData;

public class PlayerLocomotionManager : MonoBehaviour
{
    [HideInInspector] private PlayerManager playerManager;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public Rigidbody rigid;
    

    [SerializeField] float CharacterSpeed;
    [SerializeField] float RotationSpeed;
    [SerializeField] float RotationLimit;
    [SerializeField] Vector3 GravityVector = Physics.gravity;

    [SerializeField] float SwitchingDelay;

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
            CharacterShowcaseManager.Instance.CharacterPreview.animator.Play("Appear");
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
        if (SwitchingDelay > 0f) return;

        if (InputManager.Instance.PreviousInput)
        {
            SwitchingDelay = 2f;
            InputManager.Instance.PreviousInput = false;
            PlayerCharacterManager.Instance.CharacterSwitching(SwitchingDirection.Next);
        }

        //if (InputManager.Instance.NextInput)
        //{
        //    InputManager.Instance.NextInput = false;
        //    PlayerCharacterManager.Instance.CharacterSwitching(SwitchingDirection.Next);
        //}
    }

    public void OnJump()
    {
        playerManager.isJump = true;

    }

    public void OnLanding()
    {
        playerManager.isJump = false;
    }

    public void CharacterJump(JumpPath From, JumpPath To)
    {
        if (playerManager.characterType != CharacterType.DualBlade) return;
        if(From.collider != null) From.collider.enabled = false;
        if(To.collider != null) To.collider.enabled = false;
        OnJump();
        StartCoroutine(JumpAction(From, To));
    }

    public IEnumerator JumpAction(JumpPath FromTrs, JumpPath ToTrs)
    {
        playerManager.animator.animator.Play("EncounterJump");
        Vector3 SpawnPos = new Vector3(FromTrs.transform.position.x, 0f, FromTrs.transform.position.z);
        Vector3 AllocatedPos = new Vector3(ToTrs.transform.position.x, 0f, ToTrs.transform.position.z);
        Vector3 MidPos = Vector3.Lerp(SpawnPos, AllocatedPos, 0.5f);
        MidPos.y += 2.0f;

        playerManager.locomotor.rigid.useGravity = false;
        playerManager.locomotor.controller.enabled = false;
        Vector3 direction = (AllocatedPos - transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;

        float lerpInterpol = 0.0f;
        while (lerpInterpol < 1f)
        {
            lerpInterpol += Time.deltaTime * 2f;
            if (lerpInterpol > 1.0f)
            {
                lerpInterpol = 1.0f;
            }
            transform.position = Vector3.Lerp(SpawnPos, MidPos, lerpInterpol);

            // playerManager.locomotor.controller.Move(transform.forward * Time.deltaTime / 1.5f);
            yield return null;
        }
        lerpInterpol = 0.0f;

        while (lerpInterpol < 1f)
        {
            lerpInterpol += Time.deltaTime * 2f;
            if (lerpInterpol > 1.0f)
            {
                lerpInterpol = 1.0f;
            }
            transform.position = Vector3.Lerp(MidPos, AllocatedPos, lerpInterpol);

            // playerManager.locomotor.controller.Move(transform.forward * Time.deltaTime / 1.5f);
            yield return null;
        }

        playerManager.animator.animator.SetTrigger("JumpTrigger");
        yield return null;
        playerManager.locomotor.rigid.useGravity = true;
        playerManager.locomotor.controller.enabled = true;
        if (FromTrs.collider != null) FromTrs.collider.enabled = true;
        if (ToTrs.collider != null) ToTrs.collider.enabled = true;
        OnLanding();
    }


    


    private void Update()
    {
        SwitchingDelay -= Time.deltaTime;
        if (SwitchingDelay < 0f) SwitchingDelay = 0f;

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
