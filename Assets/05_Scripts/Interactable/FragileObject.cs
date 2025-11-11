using UnityEngine;

public class FragileObject : InteractObject
{
    [SerializeField] private bool isInformed;
    [SerializeField] private int HP;
    [SerializeField] Material OriginMat;
    [SerializeField] private Material HighlightMat;
    [SerializeField] private ItemScriptableObject hasItem;
    bool isActivate;
    [SerializeField] private EarnActionType EarnAction;
    [SerializeField] private AudioClip FragileSFX;

    protected override void Start()
    {
        base.Start();
        OriginMat = GetComponent<MeshRenderer>().material;

    }

    public override void ActivateEvent()
    {
        if (isActivate) return;
        isActivate = true;
        GetComponent<MeshRenderer>().material = HighlightMat;
    }

    public override void InteractEvent()
    {
        foreach (EventContainer e in InteractEventContainers)
        {
            EventMessageManager.Instance.MessageQueueRegistry(e);
        }
        TryGetComponent<EventSubscribe>(out EventSubscribe subscriber);
        if (subscriber != null) { subscriber.SubscribedInvoke(SubscribeType.Interact); }
    }

    public override void TriggerEvent()
    {
        GetComponent<AudioSource>().PlayOneShot(FragileSFX);
        HP--;
        ShakeEvent();



        if (HP < 0f)
        {
            if (hasItem != null)
            {
                ResourceManager.Instance.ItemDropObjectSpawn(transform, hasItem, EarnAction);
            }

            Destroy(gameObject);
        }


    }



    public void OnTriggerEnter(Collider other) { }
    public void OnTriggerExit(Collider other) { }
}
