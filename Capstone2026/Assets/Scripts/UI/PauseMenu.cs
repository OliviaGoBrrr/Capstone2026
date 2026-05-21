using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public SceneLoader sceneLoader;

    private bool isPaused;
    public InputActionReference pauseAction;
    public InputActionAsset playerInputMap;

    [SerializeField] private GameObject pauseMenu;

    // settings panels
    [SerializeField] private GameObject settingsOptions;
    [SerializeField] private GameObject audioSettingsOptions;
    [SerializeField] private GameObject videoSettingsOptions;
    [SerializeField] private GameObject accessibilitySettingsOptions;
    [SerializeField] private GameObject controlsSettingsOptions;

    private GameObject currentWindow;
    private List<GameObject> previousWindows = new List<GameObject>();

    void Awake()
    {
        Time.timeScale = 1;

        HideAllUI();

        isPaused = false;
    }

    private void HideAllUI()
    {
        pauseMenu.SetActive(false);
        settingsOptions.SetActive(false);
        audioSettingsOptions.SetActive(false);
        videoSettingsOptions.SetActive(false);
        accessibilitySettingsOptions.SetActive(false);
        controlsSettingsOptions.SetActive(false);

        HideCursor();
    }

    public void WasPausePressed()
    {
        if (pauseAction.action.WasPressedThisFrame())
        {
            if (isPaused)
            {
                OnBackPressed();
            }
            else
            {
                PauseLogic();
            }
        }
    }

    private void PauseLogic()
    {
        Time.timeScale = 0;
        playerInputMap.FindActionMap("Player").Disable();
        ShowCursor();

        // replace with exit animation
        pauseMenu.SetActive(true);

        isPaused = true;

        currentWindow = pauseMenu;
        previousWindows.Add(pauseMenu);
    }

    private void ResumeLogic()
    {
        Time.timeScale = 1;
        playerInputMap.FindActionMap("Player").Enable();
        // replace with exit animation
        HideAllUI();

        isPaused = false;

        previousWindows.Remove(pauseMenu);
    }

    // ---------- Button Press Logic ----------

    public void OnResumePressed()
    {
        ResumeLogic();
        HideCursor();
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
        if (previousWindows.Count > 1) // more than the initial window when esc or back button
        {
            currentWindow.SetActive(false);

            currentWindow = previousWindows[previousWindows.Count - 2]; // go back to last window
            previousWindows.Remove(previousWindows[previousWindows.Count - 1]); // delete most recently visited window

            print(previousWindows.Count);
        }
        else
        {
            ResumeLogic();
        }
    }

    public void OnQuitPressed()
    {
        // need to add better animations
        sceneLoader.LoadNewScene("MainMenu");
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
