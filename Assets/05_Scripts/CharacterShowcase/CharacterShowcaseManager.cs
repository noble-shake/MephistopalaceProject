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
        CharacterPreview = Showcases[(int)_type];
        CharacterPreview.gameObject.SetActive(true);
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
        }
        CharacterPreview.gameObject.SetActive(false);
        CharacterOn((CharacterType)idx);

    }

}