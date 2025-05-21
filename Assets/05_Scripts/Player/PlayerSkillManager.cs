using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;

    [Header("Group A")]
    [SerializeField] private SkillScriptableObject NormalGroupA;

    [Header("Group B")]
    [SerializeField] private SkillScriptableObject NormalGroupB;

    [Header("Group A")]
    [SerializeField] private SkillScriptableObject NormalGroupC;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }


}