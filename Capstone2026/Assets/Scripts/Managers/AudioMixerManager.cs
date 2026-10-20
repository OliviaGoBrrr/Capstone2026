using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerManager : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider, sfxSlider, musicSlider, voiceSlider;

    private void Start()
    {
        audioManager = GetComponent<AudioManager>();

        SetSliderValues();
    }

    public void AssignSliders(AudioMixerManager b)
    {
        this.sfxSlider = b.sfxSlider;
        this.masterSlider = b.masterSlider;
        this.voiceSlider = b.voiceSlider;
        this.musicSlider = b.musicSlider;
    }

    public void SetSliderValues()
    {
        masterSlider.value = audioManager.MasterVolume;
        sfxSlider.value = audioManager.SFXVolume;
        musicSlider.value = audioManager.MusicVolume;
        voiceSlider.value = audioManager.VoiceVolume;
    }

    public void SetMasterVolume()
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(masterSlider.value) * 20f);
        audioManager.MasterVolume = masterSlider.value;
    }

    public void SetSFXVolume()
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxSlider.value) * 20f);
        audioManager.SFXVolume = sfxSlider.value;
    }

    public void SetMusicVolume()
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(musicSlider.value) * 20f);
        audioManager.MusicVolume = musicSlider.value;
    }

    public void SetVoiceVolume()
    {
        audioMixer.SetFloat("voiceVolume", Mathf.Log10(voiceSlider.value) * 20f);
        audioManager.VoiceVolume = voiceSlider.value;
    }
}
