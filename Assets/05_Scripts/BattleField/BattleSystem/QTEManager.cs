using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class QTEManager : MonoBehaviour
{
    [SerializeField] public bool PlayerAttacking;
    [SerializeField] private List<Transform> SpawnPosition;
    [SerializeField] private List<TMP_Text> JudgeText;

    [SerializeField] private BattleQTE QteObjectPrefab;
    [SerializeField] private Queue<BattleQTE> QueQTE;

    [HideInInspector] private int SuccessCount;
    [HideInInspector] private int QTECount;
    [HideInInspector] private BattleQTE CurrentQTE;

    public delegate bool JudgeAction();
    public JudgeAction JudgeDelegate;

    float curDelay;

    private void Start()
    {
        QueQTE = new Queue<BattleQTE>();
    }

    public int GetSuccessCount()
    { 
        return SuccessCount;
    }

    public int GetQTECount()
    {
        return QTECount;
    }

    public void SpawnQTE(int numb = 1)
    {
        for (int idx = 0; idx < numb; idx++)
        {
            BattleQTE qte = Instantiate<BattleQTE>(QteObjectPrefab, SpawnPosition[idx]);
            QueQTE.Enqueue(qte);
        }
        QTECount = numb;
        QteActionStart();
        PlayerAttacking = true;
        curDelay = 0.5f;
    }

    public void QteActionStart()
    {
        if (QueQTE.Count == 0) return;
        CurrentQTE = QueQTE.Dequeue();
        CurrentQTE.currentAction = true;
        JudgeDelegate = new JudgeAction(CurrentQTE.Judge);
    }

    private void Update()
    {
        QteAction();
    }

    private void QteAction()
    {
        curDelay -= Time.deltaTime;
        if(curDelay < 0f) curDelay = 0f;
        if (curDelay > 0f) return;

        // QTE Action을 시작하겠다.
        if (!PlayerAttacking) return;


        if (CurrentQTE != null)
        {
            // QTE 액션 시간을 초과 했을 경우.
            if (CurrentQTE.TimeLimit)
            {
                Debug.Log("Fail");
                Destroy(CurrentQTE.gameObject);
                curDelay = 0.3f;
                return;
            }

            // 입력을 받았을 때,
            if (InputManager.Instance.AttackInput)
            {
                InputManager.Instance.AttackInput = false;
                // 타이밍을 맞췄을 때,
                if (JudgeDelegate())
                {
                    Debug.Log("Prefect");

                    Destroy(CurrentQTE.gameObject);
                    SuccessCount++;
                    curDelay = 0.3f;
                    return;
                }
                else
                {
                    // 타이밍을 틀리면,
                    Debug.Log("Fail");

                    Destroy(CurrentQTE.gameObject);
                    curDelay = 0.3f;
                    return;
                }
            }
        }


        // 판정 이후 파괴 된 후에는..
        if (CurrentQTE == null)
        { 
            // 더이상의 QTE 액션이 남지 않았을 때.

            if (QueQTE.Count == 0)
            {
                BattleSystemManager.Instance.WaitQteAction = true;
            }
            // 먼저 카운트를 체크하고 디큐를 해준다.
            QteActionStart();
        }
    }

    public KeyValuePair<int, int> Clear()
    {
        JudgeDelegate = null;
        PlayerAttacking = false;
        KeyValuePair<int, int> result = new(SuccessCount, QTECount);

        SuccessCount = 0;
        QTECount = 0;
        return result;
    }
}
