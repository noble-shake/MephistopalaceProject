using UnityEngine;
using UnityEngine.UI;

public class JumpTutorial : MonoBehaviour
{
    [SerializeField] Button CheckBtn;
    [SerializeField] CanvasGroup _canvas;

    private void Start()
    {
        CheckBtn = _canvas.GetComponentInChildren<Button>();
        CheckBtn.onClick.AddListener(OnClickedCheck);
    }

    public void OnClickedCheck()
    {
        GameManager.Instance.GameContinue();
        Cursor.lockState = CursorLockMode.Locked;
        Destroy(_canvas.gameObject);
        Destroy(this);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(LayerEnum.Player.ToString()))
        {
            _canvas.gameObject.SetActive(true);
            GameManager.Instance.GameStop();
            Cursor.lockState = CursorLockMode.None;
        }
    }
}