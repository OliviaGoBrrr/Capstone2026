using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource SFXObject;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void PlaySFX(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        print("AUDIO CLIP PLAYED");
        // Spawn sound object
        AudioSource audioSource = Instantiate(SFXObject, spawnTransform.position, Quaternion.identity);


        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        // Destroy clip once finished
        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
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

}
