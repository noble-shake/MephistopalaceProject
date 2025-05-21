using UnityEngine;

public class EnemyTraceZone : MonoBehaviour
{
    [HideInInspector] public EnemyManager enemyManager;

    private void Start()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
    }


    private void OnTriggerStay(Collider other)
    {
        if (enemyManager.locomotor.isHit) return;
        if (other.gameObject.layer == LayerMask.NameToLayer(LayerEnum.Player.ToString()))
        {
            Vector3 enemyPos = transform.position + Vector3.up;
            Vector3 playerPos = other.gameObject.transform.position + Vector3.up;
            if (Physics.Raycast(enemyPos, playerPos - enemyPos, out RaycastHit hit))
            {

                if (hit.collider.CompareTag("Player"))
                {
                    Debug.DrawRay(enemyPos, playerPos, Color.red);
                    enemyManager.locomotor.EnemyTrace();
                }

                return;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(LayerEnum.Player.ToString()))
        {
            enemyManager.locomotor.isTrace = false;
        }
    }
}