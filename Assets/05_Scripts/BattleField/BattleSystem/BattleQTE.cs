using UnityEngine;
using UnityEngine.UI;

public class BattleQTE : MonoBehaviour
{
    public bool currentAction;
    public bool TimeLimit;
    private float curTime;
    [SerializeField] private Transform Indicator;

    private void Start()
    {
        curTime = 0f;
    }

    private void Update()
    {
        if (!currentAction) return;

        curTime += Time.deltaTime * 1.5f;
        if (curTime > 1f)
        {
            curTime = 1f;
            TimeLimit = true;
        } 

        //Indicator
        Indicator.Rotate(new Vector3(0f, 0f, -360f * Time.deltaTime * 1.5f));
    }

    public bool Judge()
    {
        currentAction = false;
        if (curTime >= 0.75f || curTime < 1f)
        {
            return true;
        }

        return false;
    }
}