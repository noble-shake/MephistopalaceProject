using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatUI : MonoBehaviour
{
    [SerializeField] private TMP_Text Name;
    [SerializeField] private TMP_Text Level;
    [SerializeField] private Image Portrait;
    [SerializeField] private List<Image> APBarList; // 0 ~ 5
    [SerializeField] private Image HPBar;
    [SerializeField] private TMP_Text HPValue;

    public void SetAPValue(int _AP)
    {
        foreach (Image ap in APBarList)
        {
            ap.fillAmount = 0f;
        }

        for (int idx = 0; idx < _AP; idx++)
        {
            APBarList[idx].fillAmount = 1f;
        }
    }

    public void SetHPValue(float curHP, float maxHP)
    {
        HPValue.text = $"{curHP} / {maxHP}";
        HPBar.fillAmount = (float)curHP / (float)maxHP;
    }

    public void SetPortrait(string Name, Sprite Portrait)
    {
        this.Name.text = Name;
        this.Portrait.sprite = Portrait;
    }

    public void SetLevel(int level)
    {
        this.Level.text = level.ToString();
    }
}
