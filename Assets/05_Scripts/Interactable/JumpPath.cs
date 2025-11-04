using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.Playables;
using UnityEngine.Timeline;


public class JumpPath : InteractObject
{
    public JumpPath Pass;
    public BoxCollider collider;

    public override void InteractEvent()
    {
        if (PlayerCharacterManager.Instance.CurrentPlayer.characterType != CharacterType.DualBlade)
        {
            foreach (EventContainer e in InteractEventContainers)
            {
                EventMessageManager.Instance.MessageQueueRegistry(e);
            }
            TryGetComponent<EventSubscribe>(out EventSubscribe subscriber);
            if (subscriber != null) { subscriber.SubscribedInvoke(SubscribeType.Interact); }

        }
        else
        {
            PlayerCharacterManager.Instance.CurrentPlayer.locomotor.CharacterJump(this, Pass);
        }


    }
}
