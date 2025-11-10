using UnityEngine;
using UnityEngine.Playables;

public class DragonTimelineSet : MonoBehaviour
{
    PlayableDirector timeline;
    

    private void Start()
    {
        timeline = GetComponent<PlayableDirector>();
        timeline.RebuildGraph(); // the graph must be created before getting the playable graph
        timeline.playableGraph.GetRootPlayable(0).SetSpeed(2f);
    }
}