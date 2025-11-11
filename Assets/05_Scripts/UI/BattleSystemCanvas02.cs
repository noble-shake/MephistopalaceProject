using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystemCanvas02 : MonoBehaviour
{
    [SerializeField] Button ContinueButton;
    [SerializeField] Button PreviousButton;
    [SerializeField] CanvasGroup PreviousCanvas;
    [SerializeField] CanvasGroup NextCanvas;

    private void Start()
    {
        PreviousButton.onClick.AddListener(OnClickedPrevious);
        ContinueButton.onClick.AddListener(OnClickedContinue);
    }

    public void OnClickedPrevious()
    {
        SoundManager.Instance.EquipSound();
        gameObject.SetActive(false);
        PreviousCanvas.gameObject.SetActive(true);
    }

    public void OnClickedContinue()
    {
        SoundManager.Instance.EquipSound();
        gameObject.SetActive(false);
        NextCanvas.gameObject.SetActive(true);
    }

}