using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.TextCore.Text;

public class PlayerAnimationManager : MonoBehaviour
{
    [HideInInspector] private PlayerManager playerManager;
    [SerializeField] public Animator animator;
    [SerializeField] public float dampTime;
    [SerializeField] public bool dampSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();
    }
    public Vector2 GetMoveVector()
    {
        return InputManager.Instance.MoveInput;
    }

    public void MoveAnimation(Vector2 move)
    {

        //transitionNomalizedTime
        animator.SetFloat("Horizontal", move.x, dampTime, Time.deltaTime);
        animator.SetFloat("Vertical", move.y, dampTime, Time.deltaTime);
    }

    public void AttackAnimation()
    {

        if (!InputManager.Instance.AttackInput) return;
        playerManager.locomotor.rigid.useGravity = false;
        playerManager.isAttack = true;
        animator.Play("EncounterAttack");
        StartCoroutine(AttackForward());
    }

    IEnumerator AttackForward()
    {
        yield return new WaitForSeconds(0.2f);
        float curTime = 0f;
        while (curTime < 0.4f)
        {
            if (playerManager.locomotor.controller.enabled)
            {
                playerManager.locomotor.controller.Move(transform.forward * 3f * Time.deltaTime);
            }

            curTime += Time.deltaTime;
            yield return null;
        }

    }

    public void ItemEarnAnimation(Transform _item, EarnActionType _action)
    {
        playerManager.isItemEarnAction = true;
        StartCoroutine(EarnAnimator(_item, _action));
    }

    IEnumerator EarnAnimator(Transform _item, EarnActionType _action)
    {
        Vector3 moveDirect = _item.position - transform.position;
        moveDirect = new Vector3(moveDirect.x, 0f, moveDirect.z);
        Quaternion targetQuat = Quaternion.LookRotation(moveDirect.normalized);
        float curFlow = 0f;
        while (curFlow < 1f) 
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetQuat, curFlow);
            curFlow += 5f * Time.deltaTime;
            yield return null;
        }
        yield return null;
        switch (_action)
        {
            case EarnActionType.forward:
                animator.Play("EarnActionForward");
                break;
            case EarnActionType.down:
                animator.Play("EarnActionDown");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentState == GameModeState.Battle) return;
        if (playerManager.isAttack) return;
        if (playerManager.isPause) return;
        if (playerManager.isItemEarnAction) return;
        MoveAnimation(GetMoveVector());
        AttackAnimation();
    }
}
