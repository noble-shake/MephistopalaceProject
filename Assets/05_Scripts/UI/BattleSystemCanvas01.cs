using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystemCanvas01: MonoBehaviour
{
    [SerializeField] Button ContinueButton;
    [SerializeField] CanvasGroup NextCanvas;

    private void Start()
    {
        ContinueButton.onClick.AddListener(OnClickedContinue);
    }

    public void OnClickedContinue()
    {
        gameObject.SetActive(false);
        NextCanvas.gameObject.SetActive(true);
    }

    public void OnClickedPrevious()
    {

    }

}