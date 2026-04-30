using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class MainMenu : MonoBehaviour
{
    public SceneLoader sceneLoader;

    public InputActionReference pauseAction;

    [SerializeField] private GameObject settingsOptions;
    [SerializeField] private GameObject settingsButton;

    [SerializeField] private GameObject mainCamera;

    private bool settingsShown = false;

    [SerializeField] private Transform cameraSettingsPos;
    [SerializeField] private Transform cameraNormalPos;

    #region EvelynStuff
    int playerID;
    bool playerIDExists;
    List<string> playerInfo;
    #endregion

    // AUDIO CLIPS
    [SerializeField] private AudioClip buttonPressedClip;

    void Start()
    {
        // kill all tweens relating to the camera
        DOTween.KillAll();
        settingsShown = false;

        settingsOptions.SetActive(false);
        settingsButton.SetActive(true);

        mainCamera.transform.position = cameraNormalPos.position;
        mainCamera.transform.rotation = cameraNormalPos.rotation;

        #region EvelynStuff2
        //Set Up/Create file for keeping playerID's
        string path = @"../name.txt";

        using (StreamReader sr = new StreamReader(path))
{
            while (true)
            {
                string? line = sr.ReadLine();
                if (line == null) break;
                if (line == playerID.ToString())
                {
                    playerIDExists = true;
                    break;
                }
                playerID++;
            }
        }

        if (!File.Exists(path) && !playerIDExists)
        {
            string playerIDToBeStored = playerID.ToString();
            File.WriteAllText(path, playerIDToBeStored);
        }

        //Retrieve Device Specs
        List<string> deviceInfo = new List<string>
        {
            $"SystemSpecs",
            $"DeviceName: {SystemInfo.deviceUniqueIdentifier}",
            $"OS: {SystemInfo.operatingSystem}",
            $"Processor: {SystemInfo.processorModel}",
            $"Processor Cores: {SystemInfo.processorCount}",
            $"System Memory: {SystemInfo.systemMemorySize}MB",
            $"GPU: {SystemInfo.graphicsDeviceName}",
            $"GPU Memory: {SystemInfo.graphicsMemorySize}MB",
            $"Graphics Driver Version: {SystemInfo.graphicsDeviceVersion}",
            $"Screen Resolution {Screen.width} x {Screen.height}",
            "IsEditor: " + (Application.isEditor ? "Yes" : "No")
        };


        

        string messageToSend = string.Join("\n", deviceInfo);
            
        /*if (!Application.isEditor) */DiscordWebhooks.SendMessage(messageToSend, $"PlayerID: {playerID}");
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseAction.action.WasPressedThisFrame())
        {
            if (settingsShown)
            {
                // kill all tweens relating to the camera
                DOTween.Kill("Camera");

                mainCamera.transform.DOMove(cameraNormalPos.position, 0.5f).SetId("Camera");
                mainCamera.transform.DORotate(new Vector3(0, 0, 0), 0.5f).SetId("Camera");

                settingsShown = false;
                settingsOptions.SetActive(false);
                settingsButton.SetActive(true);
            }
        }
    }

    // ---------- Button Press Logic ----------

    public void StartButtonPressed()
    {
        // need to add animations
        DOTween.KillAll();
        sceneLoader.LoadNewScene("SceneSelect");
    }

    public void SettingsButtonPressed()
    {
        // kill all tweens relating to the camera
        DOTween.Kill("Camera");
        mainCamera.transform.DOMove(cameraSettingsPos.position, 0.5f).SetId("Camera");
        mainCamera.transform.DORotate(new Vector3(-12, 0, -5), 0.5f).SetId("Camera");

        //AudioManager.Instance.PlaySFX(buttonPressedClip, mainCamera.transform, 1);

        settingsShown = true;
        settingsOptions.SetActive(true);
        settingsButton.SetActive(false);
    }

    public void SettingsBackButtonPressed()
    {
        // kill all tweens relating to the camera
        DOTween.Kill("Camera");

        mainCamera.transform.DOMove(cameraNormalPos.position, 0.5f).SetId("Camera");
        mainCamera.transform.DORotate(new Vector3(0, 0, 0), 0.5f).SetId("Camera");

        settingsShown = false;
        settingsOptions.SetActive(false);
        settingsButton.SetActive(true);
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
    }
}
