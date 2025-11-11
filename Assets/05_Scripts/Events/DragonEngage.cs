using UnityEngine;

public class DragonEngage : MonoBehaviour
{
    public void OnEngagePlay()
    {
        SoundManager.Instance.ChangeSong((int)ThemeList.Boss);
        IngameDragonTimelineSet.Instance.dragon = this;
        IngameDragonTimelineSet.Instance.timeline.Play();
    }

    public void TemporaryHide()
    { 
        gameObject.SetActive(false);
    }
    
}