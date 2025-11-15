using System.Collections;
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
        StartCoroutine(CursorFind());
        _canvas = GetComponent<CanvasGroup>();
        StartButton.onClick.AddListener(OnClickedStart);
        ExitButton.onClick.AddListener(OnClickedExit);
    }

    IEnumerator CursorFind()
    {
        yield return new WaitForSeconds(0.5f);
        Cursor.lockState = CursorLockMode.None;

    }


    public void OnClickedStart()
    {
        StartButton.GetComponent<AudioSource>().Play();
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
        ExitButton.GetComponent<AudioSource>().Play();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}