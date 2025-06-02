using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleMessageUI : MonoBehaviour
{
    [HideInInspector] public CanvasGroup canvas;
    [SerializeField] public TMP_Text message;

    private void Start()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    public void Appear()
    {
        StartCoroutine(AppearMessage());
    }

    IEnumerator AppearMessage()
    {
        canvas.alpha = 0f;
        while (canvas.alpha < 1f)
        {
            canvas.alpha += Time.deltaTime * 4f;

            yield return null;
        }

        StartCoroutine(DisapeearMessage());
    }

    IEnumerator DisapeearMessage()
    {
        yield return new WaitForSeconds(1f);
        canvas.alpha = 1f;
        while (canvas.alpha > 0f)
        {
            canvas.alpha -= Time.deltaTime * 4f;

            yield return null;
        }

        EventMessageManager.Instance.isBattleMsgDone = true;
    }
}