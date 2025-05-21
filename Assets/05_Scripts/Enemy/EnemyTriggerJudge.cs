using System.Collections;
using UnityEngine;
public class EnemyTriggerJudge : MonoBehaviour
{
    [SerializeField] EnemyEncounterManager owner;
    public Collider EnemyAttackCollider;


    private void Start()
    {
        owner = GetComponentInParent<EnemyEncounterManager>();
        EnemyAttackCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {

    }
}