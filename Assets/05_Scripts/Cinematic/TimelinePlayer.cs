using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelinePlayer : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public TimelineAsset timeline;
    bool isTriggered;

    private void Update()
    {
        if (isTriggered)
        {
            if (playableDirector.state == PlayState.Paused)
            {
                playableDirector.Stop();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered) return;

        if (other.CompareTag("Player"))
        {
            if (playableDirector.state != PlayState.Playing)
            {
                playableDirector.Play();
                isTriggered = true;
            }
        }
    }
}