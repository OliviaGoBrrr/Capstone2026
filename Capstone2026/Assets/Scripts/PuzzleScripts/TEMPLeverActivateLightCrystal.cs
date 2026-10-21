using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem.Controls;

public class TEMPLeverActivateLightCrystal : MonoBehaviour, IInteractable
{
    [SerializeField] LightCrystal lightCrystalToTurnOn;
    private int originalLightsNeededToIlluminate;

    [SerializeField] float timerDuration;

    private bool alreadyInteracted = false;
    private bool startTimer = false;
    private float timerCounter;

    [SerializeField] private GameObject leverHandle;
    [SerializeField] private GameObject leverBase;

    public AudioClip TimerSFX;
    public AudioClip LeverPullSFX;

    [SerializeField] private AudioSource soundObject;
    [SerializeField] private float sfxVolume = 0.15f;


    // this script takes a light crystal and makes it a source for a set time



    void Start()
    {
        originalLightsNeededToIlluminate = lightCrystalToTurnOn.lightsNeededToIlluminate;

        // Tries to find an audio source in the object's children, returning the first one found
        if(soundObject == null)
        {
            Transform[] children  = GetComponentsInChildren<Transform>();

            for(int i = 0; i < children.Length; i++)
            {
                if(children[i].TryGetComponent<AudioSource>(out AudioSource audio))
                {
                    soundObject = audio;
                    break;
                }
            }
        }
    }

    void Update()
    {
        if (startTimer)
        {
            timerCounter += Time.deltaTime;

            if (timerCounter >= timerDuration)
            {
                lightCrystalToTurnOn.lightsNeededToIlluminate = originalLightsNeededToIlluminate;

                timerCounter = 0;
                startTimer = false;
                alreadyInteracted = false;

                if (soundObject != null)
                {
                    soundObject.Stop();
                }
            }
        }
    }

    public void OnInteract()
    {
        if (alreadyInteracted) return;

        soundObject.PlayOneShot(LeverPullSFX, sfxVolume);

        if(soundObject != null)
        {
            soundObject.Play();
        }


        leverHandle.transform.DOLocalRotate(new Vector3(-120, 0, 0), 0.25f).OnComplete(() =>
        {
            lightCrystalToTurnOn.lightsNeededToIlluminate = 0;

            startTimer = true;
            alreadyInteracted = true;

            leverHandle.transform.DOLocalRotate(new Vector3(-70, 0, 0), timerDuration - 0.07f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                leverHandle.transform.DOLocalRotate(new Vector3(-60, 0, 0), 0.1f).SetEase(Ease.InCubic);
            });
        });
    }

    public void ActivateOutline()
    {
        leverHandle.layer = 30;
        leverBase.layer = 30;
    }

    public void DeactivateOutline()
    {
        leverHandle.layer = 0;
        leverBase.layer = 0;
    }
}
