using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    InteractObject interactOwner;
    float curTime;
    private void Start()
    {
        interactOwner = GetComponentInParent<InteractObject>();
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
        if (other.gameObject.layer == LayerMask.NameToLayer(LayerEnum.DamagableCollider.ToString()))
        {
            curTime = 2f;
            Debug.Log("SWord Triggered");
            interactOwner.TriggerEvent();
        }
    }
}
