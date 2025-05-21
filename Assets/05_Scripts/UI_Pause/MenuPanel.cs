using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    [HideInInspector] protected PauseCanvas pauseCanvas;
    public PanelType panelType;

    protected virtual void Start()
    {
        pauseCanvas = PauseCanvas.Instance;
    }
}