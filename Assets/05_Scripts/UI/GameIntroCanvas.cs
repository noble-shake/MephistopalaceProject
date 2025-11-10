using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class GameIntroCanvas : MonoBehaviour
{
    CanvasGroup _canvas;
    [SerializeField] PlayableDirector GameIntroPlayer;
    [SerializeField] Button StartButton;
    [SerializeField] Button ExitButton;
    [SerializeField] CanvasGroup CreditCanvas;
    [SerializeField] TutorialUI tutorialStart;

    private void Start()
    {
        _canvas = GetComponent<CanvasGroup>();
        StartButton.onClick.AddListener(OnClickedStart);
        ExitButton.onClick.AddListener(OnClickedExit);
    }


    public void OnClickedStart()
    {
        GameIntroPlayer.Play();
    }

    public void TutorialOn()
    {
        InputManager.Instance.UIInputBind();
        tutorialStart.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnClickedExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}