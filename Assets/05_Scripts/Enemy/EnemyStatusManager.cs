using UnityEngine;

public class EnemyStatusManager : CharacterStatusManger
{
    private EnemyManager enemyManager;
    //[SerializeField, Range(0, 6)] public int AP { get; private set; }
    //[SerializeField] public int MaxAP { get; private set; }

    //public bool UseAP(int _value)
    //{
    //    if (AP < _value) return false;
    //    AP -= _value;
    //    return true;
    //}

    //public void GainAP(int _value)
    //{
    //    AP += _value;
    //    if (AP >= MaxAP) AP = MaxAP;
    //}

    private void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
    }
}
