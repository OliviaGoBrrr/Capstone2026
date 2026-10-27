using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.VisualScripting;

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
    [SerializeField] private GameObject bugReportSettingsOptions;


    private GameObject currentWindow;
    private List<GameObject> previousWindows = new List<GameObject>();

    [Header("Controller Stuff")]
    [SerializeField] private CanvasGroup pauseMenuGroup;
    [SerializeField] private CanvasGroup settingsMenuGroup;
    [SerializeField] private GameObject defaultSelection;
    [SerializeField] private GameObject pauseMenuDefaultSelection;
    [SerializeField] private GameObject settingsMenuDefaultSelection;
    [SerializeField] private GameObject audioMenuDefaultSelection;
    [SerializeField] private GameObject videoMenuDefaultSelection;
    [SerializeField] private GameObject accessibilityMenuDefaultSelection;
    [SerializeField] private GameObject controlsMenuDefaultSelection;
    [SerializeField] private GameObject bugReportMenuDefaultSelection;


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

    //When a Window is Opened, the *previous* window's canvas group should be disabled.
    //I need to use the previousWindows array/list to do this.




    public void OnResumePressed()
    {
        ResumeLogic();
        HideCursor();
    }

    public void OnWindowButtonPressed(GameObject window)
    {
        defaultSelection.SetActive(true);
        EventSystem.current.SetSelectedGameObject(defaultSelection);

        DOTween.Kill("WindowScale" + window.name);

        // scale set to 0 then tween up
        DG.Tweening.Sequence windowScaleSequence = DOTween.Sequence().SetId("WindowScale" + window.name);

        windowScaleSequence.Append(window.transform.DOScale(Vector3.zero, 0f).OnComplete(() =>
        {
            window.SetActive(true);
        }));
        windowScaleSequence.Append(window.transform.DOScale(new Vector3(1, 1, 1), 0.25f).SetEase(Ease.OutSine));

        if (currentWindow == pauseMenu)
        {
            pauseMenuGroup.interactable = false;
            pauseMenuGroup.blocksRaycasts = false;
            EventSystem.current.SetSelectedGameObject(settingsMenuDefaultSelection);
            defaultSelection.SetActive(false);
        }
        else if (currentWindow == settingsOptions)
        {
            settingsMenuGroup.interactable = false;
            settingsMenuGroup.blocksRaycasts = false;

            if (window == audioSettingsOptions)
            {
                EventSystem.current.SetSelectedGameObject(audioMenuDefaultSelection);
                defaultSelection.SetActive(false);
            }
            else if (window == videoSettingsOptions)
            {
                EventSystem.current.SetSelectedGameObject(videoMenuDefaultSelection);
                defaultSelection.SetActive(false);
            }
            else if (window == accessibilitySettingsOptions)
            {
                EventSystem.current.SetSelectedGameObject(accessibilityMenuDefaultSelection);
                defaultSelection.SetActive(false);
            }
            else if (window == controlsSettingsOptions)
            {
                EventSystem.current.SetSelectedGameObject(controlsMenuDefaultSelection);
                defaultSelection.SetActive(false);
            }
            else if (window == bugReportSettingsOptions)
            {
                EventSystem.current.SetSelectedGameObject(bugReportMenuDefaultSelection);
                defaultSelection.SetActive(false);
            }
        }
        currentWindow = window;
        previousWindows.Add(window);
    }

    public void OnBackPressed()
    {
        if (previousWindows.Count > 1) // more than the initial window when esc or back button
        {
            GameObject referenceToCurrentWindow = currentWindow; // so when current window changes reference to old one stays the same
            DOTween.Kill("WindowScale" + referenceToCurrentWindow.name);


            DG.Tweening.Sequence windowScaleSequence = DOTween.Sequence().SetId("WindowScale" + referenceToCurrentWindow.name);

            windowScaleSequence.Append(referenceToCurrentWindow.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                referenceToCurrentWindow.SetActive(false);

                
            }));

            currentWindow = previousWindows[previousWindows.Count - 2]; // go back to last window
            previousWindows.Remove(previousWindows[previousWindows.Count - 1]); // delete most recently visited window

            defaultSelection.SetActive(true);
            EventSystem.current.SetSelectedGameObject(defaultSelection);

            if (currentWindow == pauseMenu)
            {
                pauseMenuGroup.interactable = true;
                pauseMenuGroup.blocksRaycasts = true;
                EventSystem.current.SetSelectedGameObject(pauseMenuDefaultSelection);
                defaultSelection.SetActive(false);
            }
            else if (currentWindow == settingsOptions)
            {
                settingsMenuGroup.interactable = true;
                settingsMenuGroup.blocksRaycasts = true;
                EventSystem.current.SetSelectedGameObject(settingsMenuDefaultSelection);
                defaultSelection.SetActive(false);
            }
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
        PostGameDataLog.updateSceneTime($"{SceneManager.GetActiveScene().name} " + "Time in level: " + $"{Time.timeSinceLevelLoad}");
        PostGameDataLog.sendInfoToDiscord();
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
