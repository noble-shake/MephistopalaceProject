using UnityEngine;
public class EnemyTriggerJudge : MonoBehaviour
{
    [SerializeField] EnemyEncounterManager owner;
    public Collider EnemyAttackCollider;
    float curFlow;

    private void Update()
    {
        curFlow -= Time.deltaTime;
        if (curFlow < 0f) curFlow = 0f;
    }

    private void Start()
    {
        owner = GetComponentInParent<EnemyEncounterManager>();
        EnemyAttackCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (curFlow > 0f) return;
            curFlow = 2f;

            Debug.Log($"{other.name} Player OnHit !!!");
            owner.OffEncounterAttackJudge();
            owner.OnEncounterAttackExecute();
            other.GetComponent<PlayerManager>().animator.animator.speed = 0f;
            owner.enemyManager.animator.animator.speed = 0f;
            owner.OnHitEncounter();
            BattleSystemManager.Instance.isAmbushed = true;
            // interactOwner.TriggerEvent();
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }
}