using System.Collections.Generic;
using UnityEngine;

public class CharacterShowcaseManager : MonoBehaviour
{ 
    public static CharacterShowcaseManager Instance;
    [SerializeField] private List<Showcase> Showcases;
    [SerializeField] public Showcase CharacterPreview;

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }
    }

    public void CharacterOn(CharacterType _type)
    {
        if (CharacterPreview != null) CharacterPreview.Effecter.TeleportationState = TeleportFX.Unsacled_KriptoFX_Teleportation.TeleportationStateEnum.Disappear;

        switch(_type)
        {
            case CharacterType.Knight:
                break;
            case CharacterType.DualBlade:
                break;
            case CharacterType.Magician:
                break;

        }
    }


}