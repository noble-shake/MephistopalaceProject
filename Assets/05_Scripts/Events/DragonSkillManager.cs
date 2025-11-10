using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DragonSkillManager : MonoBehaviour
{
    [SerializeField] public PlayableDirector playableDirector;
    public PlayableAsset GroundSkillCombo;
    public PlayableAsset AirSkillCombo;
}