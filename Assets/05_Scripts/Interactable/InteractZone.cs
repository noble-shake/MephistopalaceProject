using UnityEngine;
using UnityEngine.UI;

public class InteractZone : MonoBehaviour
{
    protected InteractObject interactOwner;
    protected float curTime;
    protected bool interacted;
    protected float interactTime;
    [SerializeField] protected InteractGauge interactGauge;
    

    protected virtual void Start()
    {
        interactOwner = GetComponentInParent<InteractObject>();
        curTime = 0f;
    }

    protected virtual void Update()
    {
        curTime -= Time.deltaTime;
        if (curTime <= 0f) curTime = 0f;

        InteractCheck();
    }

    public void InteractCheck()
    {
        if (!interacted)
        {
            if (interactGauge.gameObject.activeSelf) { interactGauge.Cancel(); }
            return;
        }
        if (InputManager.Instance.InteractInput)
        {
            interactGauge.Interact();
        }
        else
        {
            interactGauge.Cancel();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(LayerEnum.Player.ToString()))
        {
            interacted = true;
            interactGauge.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(LayerEnum.Player.ToString()))
        {
            interacted = false;
            interactGauge.gameObject.SetActive(false);
        }
    }
}
