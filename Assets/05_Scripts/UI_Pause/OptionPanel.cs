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

    public void SetAudioMute()
    {
        if (isMute == false)
        {
            isMute = true;
            audioMixer.GetFloat("Volume", out float curVolume);
            AudioVolume = curVolume;
            SetAudioVolume(0.001f);


        }
        else
        {
            isMute = false;
            SetAudioVolume(AudioVolume);
        }
    }

    public void OnToggledMute(bool isOn)
    {
        isMute = isOn;
        SetAudioMute();
    }

    public void OnChangedVolume(float Volume)
    {
        AudioVolume = Volume;
        SetAudioVolume(Volume);
    }
}