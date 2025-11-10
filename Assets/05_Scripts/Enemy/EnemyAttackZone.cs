using UnityEngine;

public class EnemyAttackZone : MonoBehaviour
{
    [HideInInspector] public EnemyManager enemyManager;
    public Collider attackCollider;
    float curTime;

    private void Start()
    {
        attackCollider = GetComponent<Collider>();
        enemyManager = GetComponentInParent<EnemyManager>();
        curTime = 0f;
    }

    private void Update()
    {
        curTime -= Time.deltaTime;
        if (curTime < 0f) { curTime = 0f; }
    }

    private void OnTriggerStay(Collider other)
    {
        if (enemyManager.locomotor.isHit) return;
        if (curTime > 0f) return;
        if (other.gameObject.layer == LayerMask.NameToLayer(LayerEnum.Player.ToString()))
        {
            curTime = 0.5f;
            Vector3 enemyPos = transform.position + Vector3.up;
            Vector3 playerPos = other.gameObject.transform.position + Vector3.up;
            // Debug.DrawRay(enemyPos, playerPos, Color.yellow);
            if (Physics.Raycast(enemyPos, playerPos - enemyPos, out RaycastHit hit))
            {

                if (hit.collider.CompareTag("Player"))
                {
                    enemyManager.locomotor.EnemyAttack();
                }

                return;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(LayerEnum.Player.ToString()))
        {
            curTime = 3f;
            Vector3 enemyPos = transform.position + Vector3.up;
            Vector3 playerPos = other.gameObject.transform.position + Vector3.up;
            // Debug.DrawRay(enemyPos, playerPos, Color.red);
            if (Physics.Raycast(enemyPos, playerPos - enemyPos, out RaycastHit hit))
            {

                if (hit.collider.CompareTag("Player"))
                {
                    enemyManager.locomotor.isAttack = false;
                }

                return;
            }

        }
    }
}