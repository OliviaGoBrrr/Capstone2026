using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sfxSlider, masterSlider, musicSlider, voiceSlider;

    public void SetMasterVolume()
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(masterSlider.value) * 20f);
    }

    public void SetSFXVolume()
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxSlider.value) * 20f);
    }

    public void SetMusicVolume()
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(musicSlider.value) * 20f);
    }

    public void SetVoiceVolume()
    {
        audioMixer.SetFloat("voiceVolume", Mathf.Log10(voiceSlider.value) * 20f);
    }
}
