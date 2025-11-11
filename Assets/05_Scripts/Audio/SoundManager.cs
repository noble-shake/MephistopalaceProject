using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum ThemeList
{ 
    MainTitle,
    Encounter,
    Battle,
    Boss,
    Ending,
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [Header("Theme")]
    [SerializeField] public AudioClip MainTitle;
    [SerializeField] public AudioClip Encounter;
    [SerializeField] public AudioClip Battle;
    [SerializeField] public AudioClip Boss;
    [SerializeField] public AudioClip Ending;

    [Header("SFX")]
    [SerializeField] public AudioClip Equip;

    public AudioSource SurrogateSource;

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
        SurrogateSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.position = Camera.main.transform.position;
    }


    public void ChangeSong(int _index)
    {
        ThemeList targetTheme = (ThemeList)_index;

        AudioClip targetClip;
        switch (targetTheme)
        {
            default:
            case ThemeList.MainTitle:
                targetClip = MainTitle;
                break;
            case ThemeList.Encounter:
                targetClip = Encounter;
                break;
            case ThemeList.Battle:
                targetClip = Battle;
                break;
            case ThemeList.Boss:
                targetClip = Boss;
                break;
            case ThemeList.Ending:
                targetClip = Ending;
                break;
        }

        StartCoroutine(ThemeChange(targetClip));
    }

    IEnumerator ThemeChange(AudioClip _target)
    {
        float curVolume = 0.5f;
        while (curVolume > 0f)
        {
            curVolume -= Time.unscaledDeltaTime / 2f;
            if (curVolume < 0f) curVolume = 0f;
            Camera.main.GetComponent<AudioSource>().volume = curVolume;
            yield return null;
        }
        Camera.main.GetComponent<AudioSource>().Stop();
        yield return null;
        Camera.main.GetComponent<AudioSource>().clip = _target;
        Camera.main.GetComponent<AudioSource>().Play();
        while (curVolume < 0.5f)
        {
            curVolume += Time.unscaledDeltaTime / 2f;
            if (curVolume > 0.5f) curVolume = 0.5f;
            Camera.main.GetComponent<AudioSource>().volume = curVolume;
            yield return null;
        }
    }

    public void EquipSound()
    {
        SurrogateSource.PlayOneShot(Equip);
    }

    public void EncounterAttackSound()
    {
        SurrogateSource.PlayOneShot(Encounter);
    }

}