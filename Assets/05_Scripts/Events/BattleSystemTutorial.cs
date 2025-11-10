using UnityEngine;
using UnityEngine.UI;

public class BattleSystemTutorial : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(LayerEnum.Player.ToString()))
        {
            _canvas.gameObject.SetActive(true);
            GameManager.Instance.GameStop();
            Cursor.lockState = CursorLockMode.None;
            Destroy(this);
        }
    }
}