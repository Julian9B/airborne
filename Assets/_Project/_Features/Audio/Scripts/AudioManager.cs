using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SFX_VOLUME = "SFXVolume";
    private const string UI_VOLUME = "UIVolume";

    public void SetMusicVolume(float value)
    {
        mixer.SetFloat(MUSIC_VOLUME, Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        mixer.SetFloat(SFX_VOLUME, Mathf.Log10(value) * 20);
    }

    public void SetUIVolume(float value)
    {
        mixer.SetFloat(UI_VOLUME, Mathf.Log10(value) * 20);
    }
}
