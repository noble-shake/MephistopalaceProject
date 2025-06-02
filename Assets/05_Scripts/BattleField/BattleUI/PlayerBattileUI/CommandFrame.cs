using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class CommandFrame : MonoBehaviour
{
    [SerializeField] public string Name { set { CommandName.text = value; } get { return CommandName.text; } }
    [SerializeField] public string Description { set { CommandDescription.text = value; } get { return CommandDescription.text; } }
    [SerializeField] private Image Background;
    [SerializeField] private TMP_Text CommandName;
    [SerializeField] private TMP_Text CommandDescription;
    public void OnHighlight(bool isOn)
    {
        if (isOn)
        {
            Background.color = new Color(0f, 255f, 255f);
        }
        else
        {
            Background.color = Color.white;
        }

    }
}