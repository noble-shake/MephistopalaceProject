using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.Playables;
using UnityEngine.Timeline;


public class JumpPath : InteractObject
{
    public Transform Path1Transform;
    public Transform Path2Transform;

    public override void InteractEvent()
    {
        foreach (EventContainer e in InteractEventContainers)
        {
            EventMessageManager.Instance.MessageQueueRegistry(e);
        }
        TryGetComponent<EventSubscribe>(out EventSubscribe subscriber);
        if (subscriber != null) { subscriber.SubscribedInvoke(SubscribeType.Interact); }
    }
}
