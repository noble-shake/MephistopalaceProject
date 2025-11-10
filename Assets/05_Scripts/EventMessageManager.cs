using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public struct EventContainer
{
    public ContextType eventType;
    public string Name;
    public string Context;
    public Sprite Portrait;
    public ChracaterSpeaker speaker;

}

public class EventMessageManager : MonoBehaviour
{
    public static EventMessageManager Instance;
    [Header("Encounter")]
    [SerializeField] private GlobalMessageUI globalUI;
    [SerializeField] private CharacterMessageUI characterMessageUI;
    [SerializeField] private InformUI informUI;
    [SerializeField] private InformMessage informPrefab;

    [Header("Battle")]
    [SerializeField] private BattleMessageUI battleMessageUI;

    public bool isInformDone;
    public bool isGlobalDone;
    public bool isCharacterMsgDone;
    public bool isBattleMsgDone;

    private Queue<EventContainer> CharacterQueue;
    private Queue<EventContainer> GlobalQueue;
    private Queue<EventContainer> InformQueue;
    private Queue<EventContainer> BattleQueue;

    

    private void Awake()
    {
        if(Instance == null) { Instance = this; } else { Destroy(gameObject); }
    }

    private void Start()
    {
        CharacterQueue = new Queue<EventContainer>();
        GlobalQueue = new Queue<EventContainer>();
        InformQueue = new Queue<EventContainer>();
        BattleQueue = new Queue<EventContainer>();

        isInformDone = true;
        isGlobalDone = true;
        isCharacterMsgDone = true;
        isBattleMsgDone = true;
    }

    public void MessageQueueRegistry(EventContainer _container)
    {
        switch (_container.eventType)
        { 
            case ContextType.Global:
                GlobalQueue.Enqueue(_container);
                break;
            case ContextType.Character:
                CharacterQueue.Enqueue(_container);
                break;
            case ContextType.Inform:
                InformQueue.Enqueue(_container);
                break;
            case ContextType.Battle:
                BattleQueue.Enqueue(_container);
                break;
        }
    }

    public void BattleMessageEvent()
    {
        if (BattleQueue.Count == 0) return;

        if (isBattleMsgDone)
        {
            StartCoroutine(delayBattleQueue(BattleQueue.Dequeue()));
            isBattleMsgDone = false;
        }
    }

    IEnumerator delayBattleQueue(EventContainer data)
    {
        yield return new WaitForSeconds(0.45f);
        battleMessageUI.message.text = data.Context;
        battleMessageUI.Appear();
    }

    public void CharacterMessageEvent()
    {
        if (CharacterQueue.Count == 0)
        {
            return;
        }

        if (isCharacterMsgDone)
        {
            isCharacterMsgDone = false;
            EventContainer data = CharacterQueue.Dequeue();
            characterMessageUI.Appear(data.Context);
            if(data.Portrait !=null) characterMessageUI.Portrait.sprite = data.Portrait;
        }
    }


    public void GlobalMessageEvent()
    {
        if (GlobalQueue.Count == 0) return;

        if (isGlobalDone)
        {
            StartCoroutine(delayGlobalQue(GlobalQueue.Dequeue()));
            isGlobalDone = false;
        }

    }

    IEnumerator delayGlobalQue(EventContainer data)
    {
        yield return new WaitForSeconds(0.45f);
        globalUI.message.text = data.Context;
        globalUI.Appear();
    }

    public void InformMessageEvent()
    {
        if (InformQueue.Count == 0) return;
        if (isInformDone)
        {
            StartCoroutine(delayInformQue(InformQueue.Dequeue()));
            isInformDone = false;
        }
    }

    IEnumerator delayInformQue(EventContainer data)
    {
        yield return new WaitForSeconds(0.45f);
        InformMessage informObject = Instantiate<InformMessage>(informPrefab, informUI.transform);
        informObject.Message.text = data.Context;
        isInformDone = true;
    }

    private void Update()
    {
        CharacterMessageEvent();
        GlobalMessageEvent();
        InformMessageEvent();
        BattleMessageEvent();
    }
}
