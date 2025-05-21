using TeleportFX;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyPhase : BattlePhase
{
    [Header("Engage")]

    [SerializeField] public bool isTurn;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private KriptoFX_Teleportation teleportation;

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
        while (Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), AllocatedPos) > 0.1f)
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

}
