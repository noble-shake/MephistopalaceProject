using UnityEngine;

public class WeaponTriggerJudge : MonoBehaviour
{
    [SerializeField] PlayerEncounterManager player;
    [SerializeField] public GameObject WeaponTrail;
    public Collider WeaponCollider;
    [SerializeField] GameObject VFXPrefab;
    float curTime;

    private void Start()
    {
        player = GetComponentInParent<PlayerEncounterManager>();
        WeaponCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        curTime -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (curTime > 0f) return;
        if (other.gameObject.layer == LayerMask.NameToLayer(LayerEnum.Enemy.ToString()))
        {
            curTime = 1f;
            Vector3 collisionPoint = other.ClosestPoint(transform.position);
            GameObject vfx = Instantiate(VFXPrefab);
            vfx.transform.position = collisionPoint;
        }


    }

    private void OnTriggerExit(Collider other)
    {

    }
}