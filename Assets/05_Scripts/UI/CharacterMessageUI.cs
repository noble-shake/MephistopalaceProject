using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMessageUI : MonoBehaviour
{
    [HideInInspector] public CanvasGroup canvas;
    [SerializeField] public Image Portrait;
    [SerializeField] public TMP_Text Message;

    private void Start()
    {
        canvas = GetComponent<CanvasGroup>();


    }

    public void Appear(string Context)
    {
        StartCoroutine(AppearMessage(Context));
    }

    public void Disappear()
    {
        StartCoroutine(DisapeearMessage());
    }

    IEnumerator AppearMessage(string Context)
    {
        Message.text = "";
        canvas.alpha = 0f;
        while (canvas.alpha < 1f)
        {
            canvas.alpha += Time.deltaTime * 5f;

            yield return null;
        }

        foreach (char s in Context.ToCharArray())
        {
            Message.text += s;
            yield return 0.33f;
        }

        yield return new WaitForSeconds(0.5f);
        Disappear();

    }

    IEnumerator DisapeearMessage()
    {
        yield return new WaitForSeconds(0.5f);
        canvas.alpha = 1f;
        while (canvas.alpha > 0f)
        {
            canvas.alpha -= Time.deltaTime * 5f;

            yield return null;
        }
        EventMessageManager.Instance.isCharacterMsgDone = true;
    }

}
