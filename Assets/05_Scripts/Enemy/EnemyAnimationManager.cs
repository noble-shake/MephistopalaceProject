using System;
using System.Collections;
using TeleportFX;
using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    [HideInInspector] private EnemyManager enemyManager;
    [SerializeField] public Animator animator;
    [SerializeField] public float dampTime;
    [SerializeField] public bool dampSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        animator = GetComponent<Animator>();
    }

    public void MoveAnimation(float value)
    {

        //transitionNomalizedTime
        animator.SetFloat("Normalized", value, dampTime, Time.deltaTime);
    }

    #region Locomotor Required
    public void OnEncounterAttackAnimation()
    {
        if (PlayerCharacterManager.Instance.CurrentPlayer == null) return;
        StartCoroutine(EncounterAttackAnimation());
    }

    IEnumerator EncounterAttackAnimation()
    {

        Vector3 moveDirect = PlayerCharacterManager.Instance.CurrentPlayer.transform.position - transform.position;
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
        animator.Play("EncounterAttack");

        yield return StartCoroutine(AttackForward());
    }

    IEnumerator AttackForward()
    {
        yield return new WaitForSeconds(0.2f);
        float curTime = 0f;
        while (curTime < 0.4f)
        {
            enemyManager.locomotor.agent.Move(transform.forward * 1.5f * Time.deltaTime);
            curTime += Time.deltaTime;
            yield return null;
        }

    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        //if (playerManager.isAttack) return;
        //if (playerManager.isPause) return;
        //if (playerManager.isItemEarnAction) return;
        //MoveAnimation(GetMoveVector());
        //AttackAnimation();
    }
}
