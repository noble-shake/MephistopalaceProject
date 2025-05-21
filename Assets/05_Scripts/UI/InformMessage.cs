using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformMessage : MonoBehaviour
{
    [SerializeField] public TMP_Text Message;
    [HideInInspector] public CanvasGroup canvas;

    private void Start()
    {
        canvas = GetComponent<CanvasGroup>();

        StartCoroutine(AppearMessage());
    }

    IEnumerator AppearMessage()
    {
        canvas.alpha = 0f;
        while (canvas.alpha < 1f)
        {
            canvas.alpha += Time.deltaTime * 2f;

            yield return null;
        }

        StartCoroutine(DisapeearMessage());
    }

    IEnumerator DisapeearMessage()
    {
        yield return new WaitForSeconds(3f);
        canvas.alpha = 1f;
        while (canvas.alpha > 0f)
        {
            canvas.alpha -= Time.deltaTime * 2f;

            yield return null;
        }

        Destroy(gameObject);
    }

}
