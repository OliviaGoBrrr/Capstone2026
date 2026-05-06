using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public SceneLoader sceneLoader;

    public InputActionReference pauseAction;

    [SerializeField] private GameObject settingsOptions;
    [SerializeField] private GameObject settingsButton;

    [SerializeField] private GameObject mainCamera;

    private bool settingsShown = false;
    [SerializeField] private GameObject settingsBg;
    [SerializeField] private GameObject turnOnEffect;

    [SerializeField] private Transform cameraSettingsPos;
    [SerializeField] private Transform cameraNormalPos;

    private bool currentlyAnimating = false;

    // AUDIO CLIPS
    [SerializeField] private AudioClip buttonPressedClip;

    void Awake()
    {
        // kill all tweens relating to the camera
        DOTween.KillAll();
        settingsShown = false;

        settingsOptions.SetActive(false);
        settingsButton.SetActive(true);
        

        mainCamera.transform.position = cameraNormalPos.position;
        mainCamera.transform.rotation = cameraNormalPos.rotation;

        settingsBg.SetActive(false);
        turnOnEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseAction.action.WasPressedThisFrame())
        {
            if (settingsShown)
            {
                SettingsBackButtonPressed();
            }
        }
    }

    // ---------- Button Press Logic ----------

    public void StartButtonPressed()
    {
        // need to add animations
        DOTween.KillAll();
        sceneLoader.LoadNewScene("DataLoggingScene");
    }

    public void SettingsButtonPressed()
    {
        // kill all tweens relating to the camera
        DOTween.Kill("Camera");
        DOTween.Kill("ScreenOnOff");
        currentlyAnimating = true;
        mainCamera.transform.DOMove(cameraSettingsPos.position, 0.5f).SetId("Camera");
        mainCamera.transform.DORotate(new Vector3(-12, 0, -5), 0.5f).SetId("Camera").OnComplete(() =>
        {
            turnOnEffect.SetActive(true);
            turnOnEffect.GetComponent<Image>().color = Color.white;
            turnOnEffect.transform.localScale = new Vector3(0f, 0.01f, 0f);
            turnOnEffect.transform.DOScale(new Vector3(1f, 0.01f, 1f), 0.05f).SetId("ScreenOnOff").OnComplete(() =>
            {
                turnOnEffect.transform.DOScale(new Vector3(1f, 1f, 1f), 0.05f).SetId("ScreenOnOff").OnComplete(() =>
                {
                    settingsBg.SetActive(true);
                    turnOnEffect.GetComponent<Image>().DOFade(0f, 0.2f);
                    currentlyAnimating = false;
                });
            });

        });

        AudioManager.Instance.PlaySFX(buttonPressedClip, mainCamera.transform, 1);

        settingsShown = true;
        settingsOptions.SetActive(true);
        settingsButton.SetActive(false);

        
    }

    public void SettingsBackButtonPressed()
    {
        // kill all tweens relating to the camera
        DOTween.Kill("Camera");
        DOTween.Kill("ScreenOnOff");

        mainCamera.transform.DOMove(cameraNormalPos.position, 0.5f).SetId("Camera");
        mainCamera.transform.DORotate(new Vector3(0, 0, 0), 0.5f).SetId("Camera");

        settingsShown = false;
        settingsOptions.SetActive(false);
        settingsButton.SetActive(true);
        settingsBg.SetActive(false);
        turnOnEffect.SetActive(false);
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
    }
}
