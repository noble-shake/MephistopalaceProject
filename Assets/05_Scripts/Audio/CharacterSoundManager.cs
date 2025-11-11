using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundManager : MonoBehaviour
{
    [SerializeField] public List<AudioClip> audioClips;
    [SerializeField] public AudioSource audioSource;

    public void OnSFXShot(int _index)
    {
        audioSource.PlayOneShot(audioClips[_index]);
    }
}