using UnityEngine;
using UnityEngine.Playables;

public class PlayerSkillManager : MonoBehaviour
{
    [SerializeField] public PlayerManager playerManager;
    [SerializeField] public PlayableDirector playerableDirector;
    [SerializeField] public PlayableAsset SpecialMove;
    [SerializeField] public PlayableAsset Move01;
    [SerializeField] public PlayableAsset Move02;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        playerableDirector = GetComponent<PlayableDirector>();
        playerableDirector.playableAsset = SpecialMove;
    }


}