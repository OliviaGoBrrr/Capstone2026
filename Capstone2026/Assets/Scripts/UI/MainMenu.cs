using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{

    public SceneLoader sceneLoader;

    public InputActionReference pauseAction;

    [SerializeField] private GameObject mainCamera;

    [SerializeField] private Transform cameraSettingsPos;
    [SerializeField] private Transform cameraNormalPos;


    [Header("Options Screen")]
    // buttons!
    [SerializeField] private GameObject optionsContents;
    [SerializeField] private GameObject optionsButton;
    [SerializeField] private GameObject settingsButton;

    [SerializeField] private GameObject optionsBg;
    [SerializeField] private GameObject turnOnEffect; // used for animating the turn on animation
    [SerializeField] private GameObject blackScreen; // shown when the tv is off

    [SerializeField] private GameObject titleText;

    // window navigation in settings
    private GameObject currentWindow;
    private List<GameObject> previousWindows = new List<GameObject>();

    // settings panels
    [SerializeField] private GameObject settingsOptions;
    [SerializeField] private GameObject audioSettingsOptions;
    [SerializeField] private GameObject videoSettingsOptions;
    [SerializeField] private GameObject gameSettingsOptions;
    [SerializeField] private GameObject controlsSettingsOptions;

    private bool optionsShown = false;


    [Header("Audio")]
    // AUDIO CLIPS
    [SerializeField] private AudioClip buttonPressedClip;

    void Awake()
    {
        // kill all tweens relating to the camera
        DOTween.KillAll();
        optionsShown = false;

        optionsContents.SetActive(false);
        optionsButton.SetActive(false);
        settingsButton.SetActive(false);

        mainCamera.transform.position = cameraNormalPos.position;
        mainCamera.transform.rotation = cameraNormalPos.rotation;

        optionsBg.SetActive(false);
        turnOnEffect.SetActive(false);

        HideAllWindows();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseAction.action.WasPressedThisFrame())
        {
            if (optionsShown)
            {

                OnBackPressed();
            }
        }
    }

    private void HideAllWindows()
    {
        settingsOptions.SetActive(false);
        audioSettingsOptions.SetActive(false);
        videoSettingsOptions.SetActive(false);
        gameSettingsOptions.SetActive(false);
        controlsSettingsOptions.SetActive(false);
    }

    // ---------- Button Press Logic ----------

    public void StartButtonPressed()
    {
        // need to add animations
        DOTween.KillAll();
        sceneLoader.LoadNewScene("DataLoggingScene");
    }

    public void OptionsButtonPressed()
    {
        if (optionsShown) return; // dont animate again if its already shown

        // kill all tweens relating to the camera
        DOTween.Kill("Camera");
        DOTween.Kill("ScreenOnOff");

        // animations
        titleText.GetComponent<TMP_Text>().DOFade(0f, 0.2f);

        mainCamera.transform.DOMove(cameraSettingsPos.position, 0.5f).SetId("Camera");
        mainCamera.transform.DORotate(new Vector3(6.5f, 0, -5), 0.5f).SetId("Camera").OnComplete(() =>
        {
            turnOnEffect.SetActive(true);

            turnOnEffect.GetComponent<Image>().color = Color.white;

            turnOnEffect.transform.localScale = new Vector3(0f, 0.01f, 1f);

            turnOnEffect.transform.DOScale(new Vector3(1f, 0.01f, 1f), 0.05f).SetId("ScreenOnOff").OnComplete(() =>
            {
                turnOnEffect.transform.DOScale(new Vector3(1f, 1f, 1f), 0.05f).SetId("ScreenOnOff").OnComplete(() =>
                {
                    // turn on everything to be displayed
                    optionsBg.SetActive(true);
                    optionsButton.SetActive(true);
                    settingsButton.SetActive(true);

                    blackScreen.SetActive(false); // hide the black screen

                    turnOnEffect.GetComponent<Image>().DOFade(0f, 0.2f).SetId("ScreenOnOff").OnComplete(() =>
                    {
                        turnOnEffect.SetActive(false);
                    });
                });
            });
        });
        
        AudioManager.Instance.PlaySFX(buttonPressedClip, mainCamera.transform, 1);

        optionsShown = true;
        optionsContents.SetActive(true);
        
    }

    public void OptionsBackButtonPressed()
    {
        // kill all tweens relating to the camera
        DOTween.Kill("Camera");
        DOTween.Kill("ScreenOnOff");

        turnOnEffect.SetActive(true);
        blackScreen.SetActive(true);

        turnOnEffect.GetComponent<Image>().color = Color.white;

        turnOnEffect.transform.localScale = new Vector3(1f, 1f, 1f);

        turnOnEffect.transform.DOScale(new Vector3(1f, 0.01f, 1f), 0.075f).SetId("ScreenOnOff").OnComplete(() =>
        {
            turnOnEffect.transform.DOScale(new Vector3(0f, 0f, 1f), 0.075f).SetId("ScreenOnOff").OnComplete(() =>
            {
                turnOnEffect.SetActive(false);
            });
        });

        mainCamera.transform.DOMove(cameraNormalPos.position, 0.5f).SetId("Camera");
        mainCamera.transform.DORotate(new Vector3(0, 0, 0), 0.5f).SetId("Camera").OnComplete(() =>
        {
            titleText.GetComponent<TMP_Text>().DOFade(1f, 0.2f);
        });

        optionsShown = false;
        optionsContents.SetActive(false);
        optionsButton.SetActive(false);
        settingsButton.SetActive(false);
        optionsBg.SetActive(false);
    }


    public void OnWindowButtonPressed(GameObject window)
    {
        window.SetActive(true);

        currentWindow = window;
        previousWindows.Add(window);

        print(previousWindows.Count);
    }


    public void OnBackPressed()
    {
        if (previousWindows.Count > 0) // more than the options screen displayed
        {
            currentWindow.SetActive(false);

            if (previousWindows.Count == 1) // if only 1 window up, itll go back to the base screen which isnt a window so set current to null
            {
                currentWindow = null;
            }
            else
            {
                currentWindow = previousWindows[previousWindows.Count - 2]; // go back to last window
            }   
            previousWindows.Remove(previousWindows[previousWindows.Count - 1]); // delete most recently visited window

            print(previousWindows.Count);
        }
        else
        {
            OptionsBackButtonPressed();
        }
    }


    

    public void ExitButtonPressed()
    {
        Application.Quit();
    }
}
