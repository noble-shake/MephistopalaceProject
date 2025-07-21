using System.Collections;
using UnityEngine;

public class FadeCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvas;

    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    // black => white
    public void FadeIn(float _time = 1f)
    {
        if (!canvas.gameObject.activeSelf) return;
        StartCoroutine(FadeInEffect(_time));
    }

    // white => black
    public void FadeOut(float _time = 1f)
    {
        if (!canvas.gameObject.activeSelf) return;
        StartCoroutine(FadeOutEffect(_time));
    }

    public void fadeInOut(float _time = 1f)
    {
        if (!canvas.gameObject.activeSelf) return;
        StartCoroutine(FadeInOutEffect(_time));
    }

    IEnumerator FadeInEffect(float _time = 1f)
    {
        canvas.alpha = 1f;
        float a = 1f;
        while (canvas.alpha > 0f)
        {
            a -= Time.unscaledDeltaTime / _time;
            if (a < 0f) a = 0f;
            canvas.alpha = a;
            yield return null;
        }
    }

    IEnumerator FadeOutEffect(float _time = 1f)
    {
        canvas.alpha = 0f;
        float a = 0f;
        while (canvas.alpha < 1f)
        {
            if (!canvas.gameObject.activeSelf) break;
            a += Time.unscaledDeltaTime / _time;
            if (a > 1f) a = 1f;
            canvas.alpha = a;
            yield return null;
        }
    }

    IEnumerator FadeInOutEffect(float _time = 1f)
    {
        canvas.alpha = 0f;
        float a = 0f;
        while (canvas.alpha < 1f)
        {
            if (!canvas.gameObject.activeSelf) break;
            a += Time.unscaledDeltaTime / (_time/2);
            if (a > 1f) a = 1f;
            canvas.alpha = a;
            yield return null;
        }

        canvas.alpha = 1f;
        a = 1f;
        while (canvas.alpha > 0f)
        {
            a -= Time.unscaledDeltaTime / (_time/2);
            if (a < 0f) a = 0f;
            canvas.alpha = a;
            yield return null;
        }
    }

}
