using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleOrderQueue : MonoBehaviour
{
    [SerializeField] public BattlePhase battler;
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private Image CharacterBackground;
    [SerializeField] private TMP_Text CharName;

    public void SetOrderInfo(Sprite background, string Name)
    {
        this.CharName.text = Name;
        CharacterBackground.sprite = background;
        
    }

    public void Entry()
    {
        StartCoroutine(EntryEffect());
    }

    IEnumerator EntryEffect()
    {
        canvas.alpha = 0f;
        float curTime = 0f;
        while (curTime < 0f)
        { 
            curTime += Time.deltaTime * 2f;
            if (curTime > 1f) curTime = 1f;
            canvas.alpha = curTime;
            yield return null;
        }
    }

    public void Pop()
    { 
        StartCoroutine (PopEffect());
    }

    IEnumerator PopEffect()
    {
        canvas.alpha = 1f;
        float curTime = 0f;
        while (curTime < 0f)
        {
            curTime += Time.deltaTime * 2f;
            if (curTime > 1f) curTime = 1f;
            canvas.alpha = 1f - curTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
