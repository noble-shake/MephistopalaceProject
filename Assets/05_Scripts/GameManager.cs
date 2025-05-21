using System.Collections;
using UnityEngine;

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
    public bool isGamePause { get; private set; }

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
    }

    public void GameStop(float _time = 0f)
    {
        StartCoroutine(TimeStop(_time));
    }

    public void GameContinue(float _time = 0f)
    {
        StartCoroutine(TimeFlow(_time));
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
}