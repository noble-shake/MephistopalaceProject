using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;


public class InteractObject: MonoBehaviour
{
    [Header("Event Valid")]
    [SerializeField] protected bool isTriggerEventValid;
    [SerializeField] protected bool isInteractEventValid;
    [SerializeField] protected bool isActivateEventValid;
    [SerializeField] protected bool isSubscribeEventValid;

    [Header("Event Subscribed")]
    [SerializeField] protected List<EventContainer> TriggerEventContainers;
    [SerializeField] protected List<EventContainer> InteractEventContainers;
    [SerializeField] protected List<EventContainer> ActivateEventContainers;
    [SerializeField] protected EventSubscribe SubscribeInstance;

    [Header("State")]
    [HideInInspector] protected Vector3 originPosition;
    [HideInInspector] protected Quaternion originRotation;
    [HideInInspector] protected float shake_decay;
    [HideInInspector] protected float shake_intensity;

    [Header("Timeline")]
    [SerializeField] private PlayableDirector timelinePlayer;
    [SerializeField] private TimelineAsset timelineAsset;
    [SerializeField] private bool timelinePlaying;

    protected virtual void Start()
    {
        ShakeInit();
        TryGetComponent<EventSubscribe>(out SubscribeInstance);
        TryGetComponent<PlayableDirector>(out timelinePlayer);
        if (SubscribeInstance != null) isSubscribeEventValid = true;
        if (TriggerEventContainers.Count > 0) isTriggerEventValid = true;
        if (InteractEventContainers.Count > 0) isInteractEventValid = true;
        if (ActivateEventContainers.Count > 0) isActivateEventValid = true;
    }

    protected void ShakeInit()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
        shake_intensity = .08f;
        shake_decay = 0.002f;
    }

    protected void EventSubscribedRun(SubscribeType _type)
    {
        if (!isSubscribeEventValid) return;

        if (SubscribeInstance != null) { SubscribeInstance.SubscribedInvoke(_type); }

    }

    public virtual void TriggerEvent() 
    {
        TriggerMessageQueue();
        EventSubscribedRun(SubscribeType.Trigger);

    }

    public virtual void InteractEvent()
    {
        InteractMessageQueue();
        EventSubscribedRun(SubscribeType.Interact);
        if (timelinePlayer != null)
        {
            if (timelinePlaying) return;
            timelinePlayer.Play();
            timelinePlaying = true;
        }
    }

    public virtual void InteractSpecificEvent()
    {
        InteractMessageQueue();
        EventSubscribedRun(SubscribeType.Interact);
        if (timelinePlayer != null)
        {
            if (timelinePlaying) return;
            timelinePlayer.Play();
            timelinePlaying = true;
        }
    }

    public virtual void ActivateEvent()
    {
        ActivateMessageQueue();
        EventSubscribedRun(SubscribeType.Activate);
        if (timelinePlayer != null)
        {
            if (timelinePlaying) return;
            timelinePlayer.Play();
            timelinePlaying = true;
        }
    }

    protected void TriggerMessageQueue()
    {
        if (!isTriggerEventValid) return;

        foreach (EventContainer e in TriggerEventContainers)
        {
            EventMessageManager.Instance.MessageQueueRegistry(e);
        }
    }


    protected void InteractMessageQueue()
    {
        if (!isInteractEventValid) return;

        foreach (EventContainer e in InteractEventContainers)
        {
            EventMessageManager.Instance.MessageQueueRegistry(e);
        }
    }

    protected void ActivateMessageQueue()
    {
        if (!isActivateEventValid) return;

        foreach (EventContainer e in ActivateEventContainers)
        {
            EventMessageManager.Instance.MessageQueueRegistry(e);
        }
    }



    public void ShakeEvent()
    {
        StartCoroutine(LockedEffect());
    }

    protected IEnumerator LockedEffect()
    {
        float curTime = 0f;
        while (curTime < 0.4f)
        {
            if (shake_intensity > 0)
            {
                transform.position = originPosition + UnityEngine.Random.insideUnitSphere * shake_intensity;
                transform.rotation = new Quaternion(originRotation.x + UnityEngine.Random.Range(-shake_intensity, shake_intensity) * .005f,
                    originRotation.y + UnityEngine.Random.Range(-shake_intensity, shake_intensity) * .005f,
                    originRotation.z + UnityEngine.Random.Range(-shake_intensity, shake_intensity) * .005f,
                    originRotation.w + UnityEngine.Random.Range(-shake_intensity, shake_intensity) * .005f);
            }

            curTime += Time.deltaTime;
            yield return null;
        }

    }

    public void OnTimelinePlay()
    {
        if (timelinePlayer.state == PlayState.Playing) return;

        timelinePlayer.playableAsset = timelineAsset;
        timelinePlayer.Play();
        timelinePlaying = true;
    }

    private void Update()
    {
        if (timelinePlaying)
        {
            if (timelinePlayer.state != PlayState.Playing)
            {
                timelinePlaying = false;
            }
        }
    }
}
