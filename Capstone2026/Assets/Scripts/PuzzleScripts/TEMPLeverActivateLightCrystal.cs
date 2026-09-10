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

    [SerializeField] GameObject leverHandle;


    // this script takes a light crystal and makes it a source for a set time



    void Start()
    {
        originalLightsNeededToIlluminate = lightCrystalToTurnOn.lightsNeededToIlluminate;
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
            }
        }
    }

    public void OnInteract()
    {
        if (alreadyInteracted) return;

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

    public void ActivateOutline() { }

    public void DeactivateOutline() { }
}
