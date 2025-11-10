using UnityEngine;

public class CheckPointZone : MonoBehaviour
{
    Collider coll;

    private void Start()
    {
        coll = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BattleSystemManager.Instance.CheckPointPos = this.transform.position;
            coll.enabled = false;
        }
    }
}