using System.Collections;
using UnityEngine;

public class LockedDoor : InteractObject
{
    [SerializeField] private ItemScriptableObject KeyItem;
    [SerializeField] private bool isForward;
    [SerializeField] private bool isLocked;
    [SerializeField] private bool isOpened;
    [SerializeField] private bool isKeyNotRequired;




    protected override void Start()
    {
        base.Start();

    }

    public override void ActivateEvent()
    {
        if (isOpened) return;
        TryGetComponent<EventSubscribe>(out EventSubscribe subscriber);
        if (subscriber != null) { subscriber.SubscribedInvoke(SubscribeType.Activate); }
    }


    public override void TriggerEvent()
    {
        if (isOpened) return;
        if (isLocked)
        {
            StartCoroutine(LockedEffect());
            foreach (EventContainer e in TriggerEventContainers)
            {
                EventMessageManager.Instance.MessageQueueRegistry(e);            
            }
            TryGetComponent<EventSubscribe>(out EventSubscribe subscriber);
            if (subscriber != null) { subscriber.SubscribedInvoke(SubscribeType.Trigger); }
        }
    }

    public override void InteractEvent()
    {
        // 열리지 않는 문이거나, 열쇠가 없어도 되는 문이면..
        if (KeyItem != null)
        {
            if (isKeyNotRequired)
            {
                isLocked = false;
            }
            else
            {
                if (InventoryManager.Instance.UseKeyItem(KeyItem))
                {
                    isLocked = false;
                }
            }

        }


        if (isOpened) return;
        if (isLocked)
        {
            StartCoroutine(LockedEffect());
            foreach (EventContainer e in InteractEventContainers)
            {
                EventMessageManager.Instance.MessageQueueRegistry(e);
            }

            TryGetComponent<EventSubscribe>(out EventSubscribe subscriber);
            if (subscriber != null) { subscriber.SubscribedInvoke(SubscribeType.Interact); }
        }
        else
        {
            isOpened = true;
            StartCoroutine(RotateDoor());
        }
    }

    IEnumerator RotateDoor()
    {
        float curTime = 0f;
        while (curTime < 1f)
        {
            curTime += Time.deltaTime;
            transform.Rotate(0, (isForward ? 90f:-90f) * Time.deltaTime, 0f);
            yield return null;
        }

    }



    public void OnTriggerEnter(Collider other) { }
    public void OnTriggerExit(Collider other) { }
}
