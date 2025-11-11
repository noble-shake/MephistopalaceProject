using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum PanelType
{ 
    STATUS,
    INVENTORY,
    OPTION,
    EXIT,
}

public class PauseCanvas : MonoBehaviour
{
    public static PauseCanvas Instance;
    [SerializeField] private Button ContinueButton;
    [SerializeField] public PanelType CurrentPanelType;
    [SerializeField] private List<MenuPanel> panels;
    [SerializeField] private TopPanelUI topPanel;
    [SerializeField] public StatPanel statPanel;
    [SerializeField] private StatusPanel descriptionPanel;
    [SerializeField] public MenuPanel CurrentPanel;
    

    
    

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }

        for (int idx = 0; idx < System.Enum.GetNames(typeof(PanelType)).Length; idx++)
        {
            panels[idx].panelType = (PanelType)idx;
            if (panels[idx].gameObject.activeSelf) panels[idx].gameObject.SetActive(false);
        }

        CurrentPanelType = PanelType.STATUS;
        CurrentPanel = panels[(int)CurrentPanelType];
        ContinueButton.onClick.AddListener(OnClickContinue);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        ContinueCheck();
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.GameContinue(0.5f);
        if(PlayerCharacterManager.Instance.CurrentPlayer != null) PlayerCharacterManager.Instance.CurrentPlayer.isPause = false;
        InputManager.Instance.PlayerInputBind();
    }

    public void OnClickContinue()
    {
        gameObject.SetActive(false);
    }

    private void ContinueCheck()
    {
        if (InputManager.Instance.ContinueInput)
        { 
            OnClickContinue();
        }
    }

    public void OpenMenu(PanelType _type)
    {
        if (CurrentPanel.gameObject.activeSelf)
        {
            CurrentPanel.gameObject.SetActive(false);
        }

        PlayerManager curPlayer = PlayerCharacterManager.Instance.CurrentPlayer;
        var characterInfo = ResourceManager.Instance.PlayerResources[(int)curPlayer.characterType];

        Sprite icon;
        switch (curPlayer.characterType)
        {
            case CharacterType.Knight:
                icon = ResourceManager.Instance.KnightIcon;
                descriptionPanel.SetStatusPanel(icon, characterInfo.CharacterAbillityDescription, characterInfo.Description);
                break;
            case CharacterType.DualBlade:
                icon = ResourceManager.Instance.DualBladeIcon;
                descriptionPanel.SetStatusPanel(icon, characterInfo.CharacterAbillityDescription, characterInfo.Description);
                break;
            case CharacterType.Magician:
                icon = ResourceManager.Instance.MagicianIcon;
                descriptionPanel.SetStatusPanel(icon, characterInfo.CharacterAbillityDescription, characterInfo.Description);
                break;
        }
        statPanel.SetLevel(curPlayer.status.Level);
        statPanel.SetHP(curPlayer.status.HP);
        statPanel.SetMaxHP(curPlayer.status.aMaxHP);
        statPanel.SetMinATK(curPlayer.status.aMinATK);
        statPanel.SetMaxATK(curPlayer.status.aMaxATK);
        statPanel.SetSPD(curPlayer.status.aSpeedWeight);
        statPanel.SetAP(curPlayer.status.AP);
        statPanel.SetCRT(curPlayer.status.aCriticalWeight);
        statPanel.SetDEF(curPlayer.status.aDefenceWeight);
        statPanel.SetEXP(curPlayer.status.EXP);
        statPanel.SetRequiredEXP(curPlayer.status.RequireEXP);

        if (_type == PanelType.STATUS || _type == PanelType.INVENTORY)
        {
            if(!statPanel.gameObject.activeSelf) statPanel.gameObject.SetActive(true);
            if (_type == PanelType.STATUS)
            {
                foreach (EquipSlot e in statPanel.equips)
                {
                    e.isLocked = true;
                }
            }
            else
            {
                foreach (EquipSlot e in statPanel.equips)
                {
                    e.isLocked = false;
                }
            }
        }
        else
        {
            if (statPanel.gameObject.activeSelf) statPanel.gameObject.SetActive(false);
        }
        CurrentPanel = panels[(int)_type];
        CurrentPanel.gameObject.SetActive(true);
        CurrentPanelType = _type;

    }

    public void OnCurrentPanel()
    {
        OpenMenu(CurrentPanelType);
    }

    public void OnInventoryPanel()
    {
        OpenMenu(PanelType.INVENTORY);
    }

    public void OnStatusPanel()
    {
        OpenMenu(PanelType.STATUS);
    }
}
