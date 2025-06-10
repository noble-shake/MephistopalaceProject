using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultCanvas : MonoBehaviour
{
    [SerializeField] CanvasGroup canvas;
    [SerializeField] public TMP_Text ResultText;

    private void OnEnable()
    {
        StartCoroutine(PopupEvent());
    }

    IEnumerator PopupEvent()
    {
        canvas.alpha = 0f;

        float curTime = 0f;
        while (curTime < 1f)
        {
            curTime += Time.unscaledDeltaTime / 2f;
            canvas.alpha = curTime;
            yield return null;
        }

        yield return new WaitForSeconds(1.25f);

        curTime = 0f;
        while (curTime < 1f)
        {
            curTime += Time.unscaledDeltaTime / 2f;
            canvas.alpha = 1f - curTime;
            yield return null;
        }

        canvas.alpha = 0f;
        gameObject.SetActive(false);
    }
}