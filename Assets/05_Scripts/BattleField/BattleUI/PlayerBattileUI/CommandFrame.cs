using UnityEngine;
using UnityEngine.UI;

public class CommandFrame : MonoBehaviour
{
    [SerializeField] public string Name { set; get; }
    [SerializeField] public string Description { set; get; }
    [SerializeField] private Image Background;
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