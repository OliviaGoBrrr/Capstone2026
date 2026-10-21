using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerManager : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider, sfxSlider, musicSlider, voiceSlider;

    [SerializeField] private Slider[] sliders;

    private void Start()
    {
        audioManager = GetComponent<AudioManager>();

        SetSliderValues();

        if (sliders.Length > 0)
        {
            for (int i = 0; i < sliders.Length; i++)
            {
                sliders[i].onValueChanged.RemoveAllListeners();
            }
        }
    }

    public void AssignSliders(AudioMixerManager b)
    {
        masterSlider = b.masterSlider;
        sfxSlider = b.sfxSlider;
        voiceSlider = b.voiceSlider;
        musicSlider = b.musicSlider;

        sliders = new Slider[4] {masterSlider, sfxSlider, voiceSlider, musicSlider };    
    }

    public void SetSliderValues()
    {
        masterSlider.onValueChanged.AddListener((value) => { SetVolume(value, "masterVolume", ref audioManager.MasterVolume); });
        sfxSlider.onValueChanged.AddListener((value) => SetVolume(value, "SFXVolume", ref audioManager.SFXVolume));
        voiceSlider.onValueChanged.AddListener((value) => SetVolume(value, "musicVolume", ref audioManager.VoiceVolume));
        musicSlider.onValueChanged.AddListener((value) => SetVolume(value, "voiceVolume", ref audioManager.MusicVolume));

        masterSlider.value = audioManager.MasterVolume;
        sfxSlider.value = audioManager.SFXVolume;
        musicSlider.value = audioManager.MusicVolume;
        voiceSlider.value = audioManager.VoiceVolume;
    }

    public void SetVolume(float volume, string mixer, ref float variableToUpdate)
    {
        audioMixer.SetFloat(mixer, Mathf.Log10(volume) * 20f);
        variableToUpdate = volume;
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
