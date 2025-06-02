using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleCommand : MonoBehaviour
{
    public PlayerManager playerManager;
    public CommandGroup commandGroup;
    public List<SkillScriptableObject> OwnScritableObject;
    protected List<SkillScriptableObject> AllocatedScritableObject;
    [SerializeField] protected  List<CommandFrame> DisplayedList;
    [SerializeField] protected int TotalSkills;
    [SerializeField] public int CurrentIndex;
    [SerializeField] protected int CurrentPage;
    [SerializeField] protected int TotalPage;
    [SerializeField] protected int TotalIndex;
    [SerializeField] protected CommandFrame CurrentCommand;

    [Header("Canvas")]
    public bool isActivated;
    [SerializeField] private CanvasGroup canvasGroup;
    IEnumerator DisplayAction;

    public virtual void SetCommand(PlayerManager player, List<SkillScriptableObject> Skills)
    {
        foreach (CommandFrame frame in DisplayedList)
        {
            frame.OnHighlight(false);
        }

        CurrentCommand = DisplayedList[0];
        playerManager = player;
        TotalSkills = OwnScritableObject.Count;
        CurrentIndex = 0;
        CurrentPage = 0;
        DisplayedList[0].OnHighlight(true);
        if (TotalSkills > 3)
        {
            TotalIndex = 2;
            TotalPage = TotalSkills / 3 + 1;
            AllocatedScritableObject = OwnScritableObject.GetRange(0, 3);
        }
        else
        {
            if (TotalSkills != 0)
            {
                TotalIndex = TotalSkills - 1;
                TotalPage = 1;
                AllocatedScritableObject = OwnScritableObject.GetRange(0, TotalSkills);
            }
            else
            {
                TotalIndex = 0;
                TotalPage = 1;
            }

        }
    }

    public void Next()
    {
        if (TotalIndex == 0) return;

        CurrentCommand.OnHighlight(false);
        CurrentIndex++;
        if (CurrentIndex > TotalIndex) CurrentIndex = 0;
        CurrentCommand = DisplayedList[CurrentIndex];
        CurrentCommand.OnHighlight(true);
    }

    public void Previous()
    {
        if (TotalIndex == 0) return;

        CurrentCommand.OnHighlight(false);
        CurrentIndex--;
        if (CurrentIndex < 0) CurrentIndex = TotalIndex;
        CurrentCommand = DisplayedList[CurrentIndex];
        CurrentCommand.OnHighlight(true);
    }

    public void NextPage()
    {
        if (TotalPage == 1) return;
        CurrentPage++;
        if (CurrentPage > TotalPage) CurrentPage = 0;
        if (TotalSkills % 3 == 0)
        {
            AllocatedScritableObject = OwnScritableObject.GetRange(CurrentPage * 3, 3);
        }
        else
        {
            AllocatedScritableObject = OwnScritableObject.GetRange(CurrentPage * 3, TotalSkills % 3);

            int count = 0;
            for (int idx = 0; idx < AllocatedScritableObject.Count; idx++)
            {
                DisplayedList[idx].Name = AllocatedScritableObject[idx].Name;
                DisplayedList[idx].Description = AllocatedScritableObject[idx].Description;
                count++;
            }

            for (int idx = count; idx < 3; idx++)
            {
                DisplayedList[idx].Name = "";
                DisplayedList[idx].Description = "";
            }

        }

    }

    public void PreviousPage()
    {
        if (TotalPage == 1) return;
        CurrentPage--;
        if (CurrentPage < 0) CurrentPage = TotalPage;
        if (TotalSkills % 3 == 0)
        {
            AllocatedScritableObject = OwnScritableObject.GetRange(CurrentPage * 3, 3);
        }
        else
        {
            AllocatedScritableObject = OwnScritableObject.GetRange(CurrentPage * 3, TotalSkills % 3);

            int count = 0;
            for (int idx = 0; idx < AllocatedScritableObject.Count; idx++)
            {
                DisplayedList[idx].Name = AllocatedScritableObject[idx].Name;
                DisplayedList[idx].Description = AllocatedScritableObject[idx].Description;
                count++;
            }

            for (int idx = count; idx < 3; idx++)
            {
                DisplayedList[idx].Name = "";
                DisplayedList[idx].Description = "";
            }

        }
    }

    public virtual void CommandExecute()
    {
        
    }

    public virtual void CommandBack()
    { 
        
    }

    public void Display(bool isOn=true)
    {
        if(DisplayAction != null) StopCoroutine(DisplayAction);
        DisplayAction = FastDisplay(isOn);
        StartCoroutine(DisplayAction);
    }

    IEnumerator FastDisplay(bool isOn)
    {
        float curTime = 0f;
        while (curTime < 1f)
        {

            if (isOn)
            {
                curTime += Time.deltaTime;
                if (curTime > 1f) curTime = 1f;
                canvasGroup.alpha = curTime;
            }
            else
            {
                curTime += Time.deltaTime * 3f;
                if (curTime > 1f) curTime = 1f;
                canvasGroup.alpha = 1 - curTime;
            }

            yield return null;
        }

    }

}