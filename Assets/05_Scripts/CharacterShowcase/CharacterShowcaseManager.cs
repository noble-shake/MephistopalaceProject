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

    private void Start()
    {
        ShowcaseOpen(0);
    }

    public void CharacterOn(CharacterType _type)
    {
        if (CharacterPreview != null) CharacterPreview.Effecter.TeleportationState = TeleportFX.Unsacled_KriptoFX_Teleportation.TeleportationStateEnum.Disappear;

        CharacterPreview = Showcases[(int)_type];
        CharacterPreview.gameObject.SetActive(true);
        CharacterPreview.Effecter.TeleportationState = TeleportFX.Unsacled_KriptoFX_Teleportation.TeleportationStateEnum.Appear;
        CharacterPreview.TeleportationEffect(true);
    }

    public void ShowcaseOpen(int idx)
    {
        foreach (Showcase shw in Showcases)
        {
            if (CharacterPreview != null)
            {
                if (CharacterPreview == shw) continue;
            }

            if (shw.gameObject.activeSelf == false) continue;
            shw.TeleportationEffect(false);
        }

        CharacterOn((CharacterType)idx);

    }

}