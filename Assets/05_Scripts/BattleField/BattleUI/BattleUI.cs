using UnityEngine;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private Sprite PlayerBackground;
    [SerializeField] private Sprite EnemyBackground;

    [SerializeField] public CanvasGroup OrderCanvas;
    [SerializeField] public Transform OrderQueTransform;
    [SerializeField] private BattleOrderQueue orderQuePrefab;

    public BattleOrderQueue CreateOrderQueue(BattlePhase battler, string Name, Identifying _identity)
    {
        BattleOrderQueue orderObject = Instantiate(orderQuePrefab);
        orderObject.battler = battler;
        switch (_identity)
        { 
            case Identifying.Enemy:
                orderObject.SetOrderInfo(EnemyBackground, Name);
                return orderObject;
            case Identifying.Player:
                orderObject.SetOrderInfo(PlayerBackground, Name);
                return orderObject;
        
        }
        return null;
    }
}
