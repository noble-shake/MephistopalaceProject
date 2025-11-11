using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystemCanvas03 : MonoBehaviour
{
    [SerializeField] Button ContinueButton;
    [SerializeField] Button PreviousButton;
    [SerializeField] CanvasGroup PreviousCanvas;

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
        GameManager.Instance.GameContinue();
        Cursor.lockState = CursorLockMode.Locked;

    }

}