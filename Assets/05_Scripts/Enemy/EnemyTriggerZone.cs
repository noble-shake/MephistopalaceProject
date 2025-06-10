using UnityEngine;

public class EnemyTriggerZone : MonoBehaviour
{
    EnemyManager enemy;
    float curTime;
    private void Start()
    {
        enemy = GetComponentInParent<EnemyManager>();
        curTime = 0f;
    }

    private void Update()
    {
        curTime -= Time.deltaTime;
        if (curTime <= 0f) curTime = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (curTime > 0f) return;
        if (other.gameObject.CompareTag("Weapon"))
        {
            curTime = 2f;


            other.GetComponentInParent<PlayerManager>().animator.animator.speed = 0f;
            other.GetComponentInParent<PlayerManager>().encounter.OffEncounterAttackCollider();
            enemy.animator.animator.speed = 0f;
            enemy.encounter.OnHitEncounter();
            // interactOwner.TriggerEvent();
        }
    }
}
