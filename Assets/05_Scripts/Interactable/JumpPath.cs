using UnityEngine;


public class JumpPath : InteractObject
{
    public JumpPath Pass;

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
