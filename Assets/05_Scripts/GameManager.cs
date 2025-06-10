using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*
 * 게임 관련 매니저로
 * 옵션도 다루게 해주려고 한다.
 */

[System.Serializable]
public enum GameModeState
{
    Encounter,
    Battle,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameModeState CurrentState;
    [SerializeField] private Slider MouseSensivitySlider;
    [SerializeField] private Slider VolumeSlider;
    [SerializeField] private Toggle MuteToggle;
    private bool isMute;
    public float Sensivity;
    private float GameVolume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Sensivity = 10f;
        GameVolume = 0f;
        MouseSensivitySlider.onValueChanged.AddListener(OnSensivityChanged);
        VolumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    #region Time Management
    public bool isGamePause { get; private set; }

    public void GameStop(float _time = 0f)
    {
        StartCoroutine(TimeStop(_time));
    }

    public void GameContinue(float _time = 0f)
    {
        StartCoroutine(TimeFlow(_time));
    }

    public void GamePause(float _time = 1f)
    {
        StartCoroutine(TimePause(_time));
    }

    IEnumerator TimeStop(float _time = 0f)
    {
        isGamePause = true;
        float curTime = 0;
        float scale;
        while (curTime <= _time)
        {
            curTime += Time.unscaledDeltaTime;
            scale = 1 - curTime;
            if (scale <= 0f) scale = 0f;
            Time.timeScale = scale;
            yield return null;
        }
        Time.timeScale = 0f;
    }

    IEnumerator TimeFlow(float _time = 0f)
    {
        float curTime = 0;
        float scale;
        while (curTime <= 1f)
        {
            curTime += Time.unscaledDeltaTime / _time;
            scale = curTime;
            if (scale >= 1f) scale = 1f;
            Time.timeScale = scale;
            yield return null;
        }

        Time.timeScale = 1f;
        isGamePause = false;
    }

    IEnumerator TimePause(float _time = 0.5f)
    {
        Time.timeScale = 0f;
        float curTime = 0f;
        float scale;
        while (curTime <= 1f)
        {
            curTime += Time.unscaledDeltaTime / _time;
            scale = curTime;
            if (scale >= 1f) scale = 1f;
            Time.timeScale = scale;
            yield return null;
        }

        Time.timeScale = 1f;
    }
    #endregion

    #region Option

    public void OnSensivityChanged(float value)
    {
        Sensivity = value;
    }

    public void OnVolumeChanged(float value)
    {
        GameVolume = value;
    }

    public void OnMuteChanged(bool isOn)
    {
        if (isOn)
        {
            isMute = true;
        }
        else
        {
            isMute = false;
        }
    }

    #endregion

}