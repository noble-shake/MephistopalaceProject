using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusPanel : MenuPanel
{
    [SerializeField] TMP_Text CharacterAbillity;
    [SerializeField] Image CharacterIcon;
    [SerializeField] TMP_Text CharacterBackground;
    protected override void Start()
    {
        base.Start();


    }

    public void SetStatusPanel(Sprite _icon, string _abillity, string _background)
    {
        CharacterAbillity.text = _abillity;
        CharacterBackground.text = _background;
        CharacterIcon.sprite = _icon;
    }
}