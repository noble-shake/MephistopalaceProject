using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [HideInInspector] public TutorialZone zone;
    [SerializeField] Button ContinueButton;

    private void Start()
    {
        ContinueButton.onClick.AddListener(OnContinueButton);

    }

    public void OnContinueButton()
    {
        InputManager.Instance.PlayerInputBind();
        GameManager.Instance.GameContinue();
        zone.ActivateEvent();
        Destroy(gameObject);
        Destroy(zone);
    }
}