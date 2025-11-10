using UnityEngine;

public class DragonEngage : MonoBehaviour
{
    public void OnEngagePlay()
    {
        IngameDragonTimelineSet.Instance.dragon = this;
        IngameDragonTimelineSet.Instance.timeline.Play();
    }

    public void TemporaryHide()
    { 
        gameObject.SetActive(false);
    }
    
}