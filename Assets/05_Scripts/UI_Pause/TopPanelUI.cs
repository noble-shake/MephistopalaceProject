using UnityEngine;
using UnityEngine.UI;

public class TopPanelUI : MonoBehaviour
{
    [SerializeField] private Button statusBtn;
    [SerializeField] private Button inventoryBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button exitBtn;

    private void Start()
    {
        statusBtn.onClick.AddListener(() => OpenPanel(PanelType.STATUS));
        inventoryBtn.onClick.AddListener(() => OpenPanel(PanelType.INVENTORY));
        optionBtn.onClick.AddListener(() => OpenPanel(PanelType.OPTION));
        exitBtn.onClick.AddListener(() => OpenPanel(PanelType.EXIT));
    }

    public void OpenPanel(PanelType _panel)
    {
        PauseCanvas.Instance.OpenMenu(_panel);
    }

}