using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum SubscribeType
{ 
    Interact,
    Trigger,
    Activate,
}

public class EventSubscribe : MonoBehaviour
{
    [SerializeField] private UnityEvent InteractEvent;
    [SerializeField] private UnityEvent TriggerEvent;
    [SerializeField] private UnityEvent ActivateEvent;

    public void SubscribedInvoke(SubscribeType _type)
    {
        switch (_type)
        { 
            case SubscribeType.Interact:
                InteractInvoke();
                break;
            case SubscribeType.Trigger:
                TriggerInvoke();
                break;
            case SubscribeType.Activate:
                ActivateInvoke();
                break;
        }
    }

    private void InteractInvoke()
    { 
        InteractEvent.Invoke();
    }

    private void TriggerInvoke()
    {
        TriggerEvent.Invoke();
    }

    private void ActivateInvoke()
    {
        ActivateEvent.Invoke();
    }

}
