using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TeleportFX;
using Unity.Cinemachine;
using System;
using System.Linq;

[System.Serializable]
public enum BattleSystemPhase
{ 
    Engage,
    Command,
    Result,
}

[System.Serializable]
public enum ActivateTarget
{
    Self,
    ALL,
    TargettingEnemy,
    TargettingAlly,
    ALLEnemy,
    ALLAlly,
    None, // Only Used In TurnOver.
}
public class BattleSystemManager : MonoBehaviour
{
    public static BattleSystemManager Instance;
    [SerializeField] public BattleSystemPhase CurrentPhase;
    [SerializeField] public BattleUI battleUI;
    [SerializeField] public QTEManager qteManager;
    [SerializeField] public BattlePhase CurrentBattler;
    [SerializeField] public BattleOrderQueue CurrentBattlerOrderQueue;

    [Header("Battle Reference")]
    [SerializeField] private EnemyScriptableObject referenceEnemey;

    [Header("Battler Ordering")]
    [SerializeField] public bool isAmbushed;
    [SerializeField] public List<BattlePhase> BattlerEntries;
    [SerializeField] public Queue<BattlePhase> QueueEntries;
    [SerializeField] public Queue<BattleOrderQueue> BattlerOrderObjects;

    [SerializeField] public List<BattlePhase> TempActivateTargets;

    [Header("Encounter Record")]
    [SerializeField] public Vector3 EncounterPos;

    [Header("BattleField Transform")]
    [SerializeField] public List<Transform> CenterPoints;
    [SerializeField] public List<Transform> EnemySpawnPoints;
    [SerializeField] public List<Transform> SpawnPoints;
    [SerializeField] public List<Transform> EnemyAllocatedPoints;
    [SerializeField] public List<Transform> AllocatedPoints;

    [Header("QTE Action")]
    public bool TestQTE;
    public bool WaitQteAction;
    public int SuccessQTE;
    public int TotalQTE;

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }
    }

    private void Update()
    {
        if (TestQTE) 
        {
            TestQTE = false;
            StartCoroutine(WaitingQTE(3));
        }

        if(GameManager.Instance.CurrentState == GameModeState.Encounter) return;
        UpdateBattleSequence();
    }

    public void SetReferenceEnemy(EnemyScriptableObject enemyRef)
    { 
        referenceEnemey = enemyRef;
    }

    private void EntryRegistry(CharacterManager target)
    {
        BattlerEntries.Add(target.phaser);
    }

    /*
     * Phase.
     * 
     * 1. Engage Battle Phase
     * 카메라로 전체를 한번 살펴본다.
     * Turn이 결정된다.
     * Turn은 선입선출 방식인 큐로..
     * 
     * 2. Player Turn Phase
     * 카메라 노말라이즈 값이 0에서 0.35값 (커맨드 명령 시점)으로 간다.
     * 커맨드 UI 출력한다.
     * 입력을 기다린다. 입력 전까지 다음 페이즈는 오지 않는다.
     * 입력을 행하였다면, Turn을 다음 큐에 보낸다.
     * 
     * 3. Enemy Turn Phase
     * 카메라를 적 카메라로 변경한다.
     * 애니메이션 시작에서 끝까지 카메라 줌과 흔들림으로 연출한다.
     * 특정 스킬 발동시에는 전용 카메라를 이용한다.
     */

    IEnumerator BattleSequenceEngage()
    {
        GameManager.Instance.GameStop();
        float curTime = 0f;
        CinemachineCamera currentCam = CameraManager.Instance.GetCamera();
        while (curTime < 1f)
        {
            curTime += Time.unscaledDeltaTime * 1.5f;
            if (curTime > 1f) curTime = 1f;
            currentCam.Lens.FieldOfView = 60 - 20 * curTime;
            yield return null;
        }

        CameraManager.Instance.fadeCanvas.FadeOut(1f);
        yield return new WaitForSecondsRealtime(2f);
        Camera.main.GetComponent<CinemachineBrain>().DefaultBlend.Time = 0f;
        CameraManager.Instance.OnLiveCamera(CameraType.BattleCenter);

        CameraManager.Instance.fadeCanvas.FadeIn(2f);
        yield return new WaitForSecondsRealtime(1.5f);
        GameManager.Instance.GameContinue();
        Camera.main.GetComponent<CinemachineBrain>().DefaultBlend.Time = 1.5f;
        Instance.OnBattleSequence();
    }

    public void OnEngageSequence()
    {
        
        StartCoroutine(BattleSequenceEngage());
    }


    public void OnBattleSequence()
    {
        CameraManager.Instance.CenterCameraSlideAction();
        PlayerCharacterManager playerCharManager = PlayerCharacterManager.Instance;
        playerCharManager.CurrentPlayer.animator.animator.speed = 1f;
        EncounterPos = playerCharManager.CurrentPlayer.transform.position;

        int PlayerCount = playerCharManager.Playables.Count;
        if (PlayerCount == 0)
        {
            Debug.LogWarning("It Can Be.. Playable count was 0.");
            return;
        }

        // 자리 배정, Entry 포함 시킨다.
        if (PlayerCount == 1)
        {
            playerCharManager.CurrentPlayer.locomotor.controller.enabled = false;
            playerCharManager.CurrentPlayer.transform.position = SpawnPoints[1].position;
            EntryRegistry(playerCharManager.CurrentPlayer);
            playerCharManager.CurrentPlayer.phaser.SpawnPoint = SpawnPoints[1];
            playerCharManager.CurrentPlayer.phaser.AllocatedPoint = AllocatedPoints[1];
            playerCharManager.CurrentPlayer.phaser.PhaseEngage();
        }
        else
        {
            for (int idx = 0; idx < PlayerCount; idx++)
            {
                foreach (PlayerManager player in playerCharManager.Playables.Values)
                {
                    player.locomotor.controller.enabled = false;
                    if (!player.gameObject.activeSelf) player.gameObject.SetActive(true);
                    player.transform.position = SpawnPoints[idx].position;
                    EntryRegistry(player);
                    player.phaser.SpawnPoint = SpawnPoints[idx];
                    player.phaser.AllocatedPoint = AllocatedPoints[idx];
                    // player.phaser.PhaseEngage();
                }
            }
        }
        // 상대할 적을 선택한다.
        // TODO: Random Pool 내에서 인원 수에 따라, 최대 3마리까지 선택한다.
        List<EnemyManager> EnemyEntries = ResourceManager.Instance.GetEnemies(referenceEnemey, PlayerCount > 1 ? UnityEngine.Random.Range(1, 3) : UnityEngine.Random.Range(0, 2));
        if (EnemyEntries.Count == 1)
        {

            EnemyEntries[0].locomotor.controller.enabled = false;
            EnemyEntries[0].transform.position = EnemySpawnPoints[1].position;
            EnemyEntries[0].locomotor.controller.enabled = true;
            EntryRegistry(EnemyEntries[0]);
            EnemyEntries[0].phaser.SpawnPoint = EnemySpawnPoints[1];
            EnemyEntries[0].phaser.AllocatedPoint = EnemyAllocatedPoints[1];
            // EnemyEntries[0].phaser.PhaseEngage();
        }
        else
        {
            for (int idx = 0; idx < EnemyEntries.Count; idx++)
            {
                //EnemyEntries[idx].locomotor.controller.enabled = false;
                //EnemyEntries[idx].transform.position = EnemySpawnPoints[idx].position;
                //EnemyEntries[idx].locomotor.controller.enabled = true;
                EntryRegistry(EnemyEntries[idx]);
                EnemyEntries[idx].phaser.SpawnPoint = EnemySpawnPoints[idx];
                EnemyEntries[idx].phaser.AllocatedPoint = EnemyAllocatedPoints[idx];
                // EnemyEntries[idx].phaser.PhaseEngage();
            }
        }

        // 엔트리가 모두 구성되었으므로, 엔트리 정보로 OrderQueue를 선택해야 한다.
        // 기습을 당했다면, 모든 공격 순서는 랜덤으로 뒤섞인다.
        // 기습을 당하지 않았다면, 플레이어가 먼저 행동한다.
        if (isAmbushed)
        {

        }
        else
        {
            QueueEntries = new Queue<BattlePhase>();
            BattlerOrderObjects = new Queue<BattleOrderQueue>();

            foreach (BattlePhase entry in BattlerEntries)
            { 
                var entryOrder = battleUI.CreateOrderQueue(entry.DisplayName, entry.identityType);
                QueueEntries.Enqueue(entry);
                entryOrder.transform.SetParent(battleUI.OrderQueTransform);
                entryOrder.transform.SetAsLastSibling();
                entryOrder.Entry();
                BattlerOrderObjects.Enqueue(entryOrder);
            }

            CurrentBattler = QueueEntries.Dequeue();
            CurrentBattlerOrderQueue = BattlerOrderObjects.Dequeue();
        }

        EventMessageManager.Instance.MessageQueueRegistry(new EventContainer() { Context = $"{CurrentBattler.DisplayName} 의 차례" });

        foreach (BattlePhase b in BattlerEntries)
        {
            b.PhaseEngage();
        }
    }

    // 전투 행동마다, 현재의 배틀 엔트리를 체크한다.
    public void UpdateBattleSequence()
    {
        switch (CurrentPhase)
        {
            case BattleSystemPhase.Engage:
                EngageTransition();
                break;
            case BattleSystemPhase.Command:
                break;
            case BattleSystemPhase.Result:
                break;
        }        
    }

    public void EngageTransition()
    {
        if (CurrentPhase != BattleSystemPhase.Engage) return;
        if (BattlerEntries.Count != 0 && CurrentBattler != null)
        {
            bool engageRunning = false;
            foreach (BattlePhase b in BattlerEntries)
            {
                if (!b.isEngage)
                {
                    engageRunning = true;
                    break;
                } 
            }

            if (engageRunning)
            {
                return;
            }

            CurrentPhase = BattleSystemPhase.Command;
            StartCoroutine(EngageToCommand());
        }
    }

    // 전투가 완전히 처음 시작 했을 때,
    IEnumerator EngageToCommand()
    {
        yield return new WaitForSeconds(1.25f);
        CurrentBattler.CurrentPhase = PhaseType.Command;
        CurrentBattler.PhaseCommand();
    }

    // 전투 종료 후, 결과를 정산하고 Encounter로 돌아간다.
    public void OffBattleSequence()
    {
        GameManager.Instance.GameStop();
        GameManager.Instance.CurrentState = GameModeState.Battle;

        
    }

    #region After Execute Skill Process

    // 현재 배틀러 행동에 대한 처리가 끝난 후,
    // 스위칭하면서, 상태 이상 및 스탯 변화를 체크한다.
    // CheckTargetState가 먼저 동작한다.
    public void UpdateEntry()
    {
        CheckTargetState(CurrentBattler.GetComponent<CharacterBattleManager>().CurrentTargets);
        CheckStateChange();
        SwitchingQueue();
    }

    // 스킬의 대상이 된 타겟들에 대하여 처리를 한다.
    // 기본적으로 체력을 체크, 버프나 상태이상이 구현된다면.. 여기서 처리해도 될 듯하다.
    public void CheckTargetState(List<BattlePhase> Targets)
    {
        List<BattlePhase> bodies = new List<BattlePhase>();
        for (int idx = 0; idx < Targets.Count; idx++)
        {
            if (Targets[idx].GetComponent<CharacterStatusManger>().isDead)
            {
                bodies.Add(Targets[idx]);
                continue;
            }

            // 상태이상 체크, 버프/디버프 체크
        }

        List<BattlePhase> tempQueue = QueueEntries.ToList();
        foreach (BattlePhase b in bodies)
        {
            tempQueue.Remove(b);
        }
        
    }

    // buff effect Process
    public void CheckStateChange()
    {

        //CurrentBattler = QueueEntries.Dequeue();
        //CurrentBattlerOrderQueue = BattlerOrderObjects.Dequeue();
    }

    public void SwitchingQueue()
    {
        QueueEntries.Enqueue(CurrentBattler);
        CurrentBattler = QueueEntries.Dequeue();
        EventMessageManager.Instance.MessageQueueRegistry(new EventContainer() { Context = $"{CurrentBattler.DisplayName} 의 차례" });
        CurrentBattler.PhaseCommand();
    }

    public void CoroutineRunner(IEnumerator _enumerator)
    {
        StartCoroutine(_enumerator);
    }

    #endregion

    #region Select Target

    /*
     *  ActivateTarget
    Self,
    ALL,
    TargettingEnemy,
    TargettingAlly,
    ALLEnemy,
    ALLAlly,
    None, // Only Used In TurnOver.
     */

    public int SelectIndex;

    public void SelectTarget(ActivateTarget targetType)
    {
        // Action Script Execution
        switch (targetType)
        { 
            case ActivateTarget.None:
            case ActivateTarget.Self:
                TargetSelf();
                break;
            case ActivateTarget.ALL:
                TargetALL();
                break;
            case ActivateTarget.TargettingEnemy:
            case ActivateTarget.ALLEnemy:
                TargetEnemy();
                break;
            case ActivateTarget.ALLAlly:
            case ActivateTarget.TargettingAlly:
                TargetAlly();
                break;
        }

        SelectIndex = 0;
    }

    public void TargetSelf()
    {
        TempActivateTargets = new List<BattlePhase>();
        TempActivateTargets.Add(CurrentBattler);
        CurrentBattler.AllocatedPoint.GetComponent<AllocatedTransform>().circleObject.gameObject.SetActive(true);
    }

    public void TargetALL()
    {
        TempActivateTargets = new List<BattlePhase>();
        foreach (BattlePhase phaser in BattlerEntries)
        {
            if (phaser.GetComponent<CharacterStatusManger>().isDead) continue;
            TempActivateTargets.Add(phaser);
            phaser.AllocatedPoint.GetComponent<AllocatedTransform>().circleObject.gameObject.SetActive(true);
        }
    }

    public void TargetEnemy()
    {
        TempActivateTargets = new List<BattlePhase>();
        foreach (BattlePhase phaser in BattlerEntries)
        {
            if (phaser.GetComponent<CharacterStatusManger>().isDead) continue;
            if (phaser.GetComponent<EnemyPhase>() != null)
            {
                TempActivateTargets.Add(phaser);
            }
        }
        TempActivateTargets[0].AllocatedPoint.GetComponent<AllocatedTransform>().circleObject.gameObject.SetActive(true);
    }

    public void TargetAlly()
    {
        TempActivateTargets = new List<BattlePhase>();
        foreach (BattlePhase phaser in BattlerEntries)
        {
            if (phaser.GetComponent<CharacterStatusManger>().isDead) continue;
            if (phaser.GetComponent<PlayerPhase>() != null)
            {
                TempActivateTargets.Add(phaser);
                phaser.AllocatedPoint.GetComponent<AllocatedTransform>().circleObject.gameObject.SetActive(true);
            }
        }
    }

    public void TargetDied()
    { 
        
    }


    #endregion


    #region QTE Action
    public void QTEEngage(int numb = 1)
    {
        StartCoroutine(WaitingQTE(numb));
    }

    private IEnumerator WaitingQTE(int numb = 1)
    {
        qteManager.SpawnQTE(numb);
        qteManager.PlayerAttacking = true;
        yield return new WaitUntil(() => WaitQteAction);
        WaitQteAction = false;
        Debug.Log("QTE Done");
        var result = qteManager.Clear();
        SuccessQTE = result.Key;
        TotalQTE = result.Value;
        CurrentBattler.OnQTEDone();
    }

    private IEnumerator WaitingQTE(Action<KeyValuePair<int, int>> result, int numb = 1)
    {
        qteManager.SpawnQTE(numb);
        qteManager.PlayerAttacking = true;
        yield return new WaitUntil(() => WaitQteAction);
        WaitQteAction = false;
        Debug.Log("QTE Done");
        result?.Invoke(qteManager.Clear());
    }
    #endregion
}
