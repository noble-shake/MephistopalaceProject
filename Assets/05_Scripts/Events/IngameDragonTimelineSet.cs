using UnityEngine;
using UnityEngine.Playables;

public class IngameDragonTimelineSet : MonoBehaviour
{
    public static IngameDragonTimelineSet Instance;
    public PlayableDirector timeline;
    public DragonEngage dragon;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        timeline = GetComponent<PlayableDirector>();
        timeline.RebuildGraph(); // the graph must be created before getting the playable graph
        timeline.playableGraph.GetRootPlayable(0).SetSpeed(0.7f);
    }

    public void EngageDone()
    {
        dragon.gameObject.SetActive(true);
        dragon.GetComponent<EnemyPhase>().OnEngageActionDone();
    }
}