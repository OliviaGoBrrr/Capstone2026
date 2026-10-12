using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource playerBatteryAudioSource, playerMovementAudioSource;
    [SerializeField] private AudioSource SFXObject;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // at some point it might be a good idea to put in something for cancelling audio clips early, 
    public void PlaySFX(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // Spawn sound object
        AudioSource audioSource = Instantiate(SFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        // Destroy clip once finished
        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    public AudioSource PlaySFXWithReference(AudioClip audioClip, Transform spawnTransform, float volume)
    {

        // Spawn sound object
        AudioSource audioSource = Instantiate(SFXObject, spawnTransform.position, Quaternion.identity);


        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        // Destroy clip once finished
        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);

        return audioSource;
    }

    public void PlayRandomSFX(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        // randomly pick sfx
        int rand = Random.Range(0, audioClip.Length);

        // Spawn sound object
        AudioSource audioSource = Instantiate(SFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip[rand];

        audioSource.volume = volume;

        audioSource.Play();

        // Destroy clip once finished
        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }
    public void StartSFXLoop(AudioClip audioClip, Transform spawnTransform, float volume, int movementOrBattery)
    {
        AudioSource target = movementOrBattery == 0 ? playerBatteryAudioSource : playerMovementAudioSource;

        if (target.isPlaying && target.clip == audioClip) return;
        
        target.clip = audioClip;
        target.volume = volume;
        target.DOFade(volume, 1f);
        target.loop = true;
        target.Play();
    }

    public void StopLoop(int movementOrBattery)
    {
        AudioSource target = movementOrBattery == 0 ? playerBatteryAudioSource : playerMovementAudioSource;
        if (target.volume != 0f)
        {
            target.DOFade(0f, 0.5f);
            target.loop = false;
            target.Stop();
        }
        
    }

    public void StopAllSounds()
    {
        foreach (AudioSource source in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
        {
            source.Stop();
        }
    }
}
