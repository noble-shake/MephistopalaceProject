

public class ProjectilPath : InteractObject
{

    public override void ActivateEvent()
    {
        base.ActivateEvent();
    }

    public override void InteractEvent()
    {
        if (PlayerCharacterManager.Instance.CurrentPlayer.characterType != CharacterType.Magician)
        {
            foreach (EventContainer e in InteractEventContainers)
            {
                EventMessageManager.Instance.MessageQueueRegistry(e);
            }
            //TryGetComponent<EventSubscribe>(out EventSubscribe subscriber);
            //if (subscriber != null) { subscriber.SubscribedInvoke(SubscribeType.Interact); }

        }
        else
        {
            PlayerCharacterManager.Instance.CurrentPlayer.encounter.EncounterFirePlay(this.transform);
        }


    }
}
