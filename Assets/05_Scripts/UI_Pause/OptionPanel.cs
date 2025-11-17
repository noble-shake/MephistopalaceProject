using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OptionPanel : MenuPanel
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float AudioVolume;
    bool isMute;

    [SerializeField] Toggle MuteToggle;
    [SerializeField] Slider VolumeSlider;

    protected override void Start()
    {
        base.Start();
        AudioVolume = 1f;
        SetAudioVolume(AudioVolume);
        MuteToggle.onValueChanged.AddListener(OnToggledMute);
        VolumeSlider.onValueChanged.AddListener(OnChangedVolume);
    }

    public void SetAudioVolume(float Volume)
    {
        audioMixer.SetFloat("Volume", Mathf.Log10(Volume) * 20);
    }

    public void OnToggledMute(bool isOn)
    {
        isMute = isOn;
        if (isMute) // false => true
        {
            audioMixer.GetFloat("Volume", out float curVolume);
            AudioVolume = curVolume;
            SetAudioVolume(0.001f);
        }
        else
        {
            SetAudioVolume(AudioVolume);
        }
    }

    public void OnChangedVolume(float Volume)
    {
        AudioVolume = Volume;
        SetAudioVolume(Volume);
    }
}