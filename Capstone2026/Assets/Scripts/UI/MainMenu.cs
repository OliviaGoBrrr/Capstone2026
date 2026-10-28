using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{

    public bool SkipCutscene = false;

    public SceneLoader sceneLoader;

    public InputActionReference pauseAction;

    [SerializeField] private GameObject mainCamera;

    [SerializeField] private Transform cameraSettingsPos;
    [SerializeField] private Transform cameraNormalPos;


    [SerializeField] private InputActionReference navigateAction;


    [Header("Options Screen")]
    // buttons!
    [SerializeField] private GameObject optionsContents;
    [SerializeField] private GameObject optionsButton;
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject attributionsButton;
    [SerializeField] private GameObject creditsButton;

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
    [SerializeField] private GameObject controlsSettingsOptions;
    [SerializeField] private GameObject accessibilitySettingsOptions;

    private bool optionsShown = false;

    [SerializeField] private float settingsFadeInSpeed = 0.5f;
    [SerializeField] private float settingsZoomInSpeed = 0.5f;

    [SerializeField] private float settingsFadeOutSpeed = 0.5f;
    [SerializeField] private float settingsZoomOutSpeed = 0.5f;

    [Header("ControllerStuff")]
    [SerializeField] private CanvasGroup mainMenuGroup;
    [SerializeField] private CanvasGroup settingsGroup;
    [SerializeField] private CanvasGroup monitorButtonsGroup;
    [SerializeField] private GameObject defaultSelection;
    [SerializeField] private GameObject settingsMenuDefaultSelection;
    [SerializeField] private GameObject audioMenuDefaultSelection;
    [SerializeField] private GameObject videoMenuDefaultSelection;
    [SerializeField] private GameObject accessibilityMenuDefaultSelection;
    [SerializeField] private GameObject controlsMenuDefaultSelection;
    [SerializeField] private GameObject mainMenuMenuDefaultSelection;
    [SerializeField] private GameObject accessibilityButton;

    void Awake()
    {
        if (Gamepad.current == null)
        {
            EventSystem.current.firstSelectedGameObject = null;
        }

        PostGameDataLog.listInits();
        
        // kill all tweens relating to the camera
        DOTween.KillAll();
        optionsShown = false;

        optionsContents.SetActive(false);
        optionsButton.SetActive(false);
        settingsButton.SetActive(false);
        attributionsButton.SetActive(false);
        creditsButton.SetActive(false);

        mainCamera.transform.position = cameraNormalPos.position;
        mainCamera.transform.rotation = cameraNormalPos.rotation;

        optionsBg.SetActive(false);
        turnOnEffect.SetActive(false);

        blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0); // sets alpha to 0

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
        Debug.Log(optionsShown);
    }

    public void SettingsScreenEntered()
    {
        mainMenuGroup.interactable = false;
        mainMenuGroup.blocksRaycasts = false;
        if (optionsShown) return; // dont animate again if its already shown

        // kill all tweens relating to the camera
        DOTween.Kill("Camera");
        DOTween.Kill("ScreenOnOff");

        blackScreen.SetActive(true);

        titleText.GetComponent<Image>().DOFade(0f, 0.2f); // fade out title text

        // zoom in to monitor & fade to black
        Sequence zoomFadeCameraIn = DOTween.Sequence();
        zoomFadeCameraIn.Insert(0, mainCamera.transform.DOMove(cameraSettingsPos.position, settingsZoomInSpeed).SetEase(Ease.InOutSine)).
            Insert(0, mainCamera.transform.DORotate(new Vector3(12f, -18f, 12f), settingsZoomInSpeed).SetEase(Ease.InOutSine)).
            Insert(0, blackScreen.GetComponent<Image>().DOFade(1, settingsFadeInSpeed).SetEase(Ease.InQuint)).SetId("Camera").OnComplete(() =>
        {
            // set up turn on effect
            turnOnEffect.SetActive(true);

            turnOnEffect.GetComponent<Image>().color = Color.white;

            turnOnEffect.transform.localScale = new Vector3(0f, 0.01f, 1f);

            // turn on animation
            turnOnEffect.transform.DOScale(new Vector3(1f, 0.01f, 1f), 0.05f).SetId("ScreenOnOff").OnComplete(() =>
            {
                turnOnEffect.transform.DOScale(new Vector3(1f, 1f, 1f), 0.05f).SetId("ScreenOnOff").OnComplete(() =>
                {
                    // enable everything to be displayed
                    optionsBg.SetActive(true);
                    optionsButton.SetActive(true);
                    settingsButton.SetActive(true);
                    attributionsButton.SetActive(true);
                    creditsButton.SetActive(true);

                    blackScreen.SetActive(false); // hide the black screen

                    turnOnEffect.GetComponent<Image>().DOFade(0f, 0.2f).SetId("ScreenOnOff").OnComplete(() => // fade away turn on anim
                    {
                        turnOnEffect.SetActive(false);
                    });
                });
            });
        });

        optionsShown = true;
        optionsContents.SetActive(true);
    }

    public void SettingsScreenExited()
    {
        // screen off anim

        // set alpha 1
        // disable all settings stuff
        // fade to nothing
        // zoom out

        // kill all tweens relating to the camera
        DOTween.Kill("Camera");
        DOTween.Kill("ScreenOnOff");

        turnOnEffect.SetActive(true);
        blackScreen.SetActive(true);

        turnOnEffect.GetComponent<Image>().color = Color.white; // resets color
        blackScreen.GetComponent<Image>().color = Color.black; // resets alpha to max

        turnOnEffect.transform.localScale = new Vector3(1f, 1f, 1f);
        

        // turn off animation
        turnOnEffect.transform.DOScale(new Vector3(1f, 0.01f, 1f), 0.075f).SetId("ScreenOnOff").OnComplete(() =>
        {
            turnOnEffect.transform.DOScale(new Vector3(0f, 0f, 1f), 0.075f).SetId("ScreenOnOff").OnComplete(() =>
            {
                turnOnEffect.SetActive(false);

            });
        });

        Sequence zoomFadeCameraOut = DOTween.Sequence();

        zoomFadeCameraOut.Insert(0, mainCamera.transform.DOMove(cameraNormalPos.position, settingsZoomOutSpeed).SetEase(Ease.InOutSine)).
                Insert(0, mainCamera.transform.DORotate(new Vector3(0f, 0f, 0f), settingsZoomInSpeed).SetEase(Ease.InOutSine)).
                Insert(0.1f, blackScreen.GetComponent<Image>().DOFade(0, settingsFadeOutSpeed).SetEase(Ease.OutQuint)).SetId("Camera").OnComplete(() =>
                {
                    titleText.GetComponent<Image>().DOFade(1f, 0.2f);
                    blackScreen.SetActive(false);

                    mainMenuGroup.interactable = true;
                    mainMenuGroup.blocksRaycasts = true;

                    defaultSelection.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(mainMenuMenuDefaultSelection);
                });

        optionsShown = false;
        mainMenuGroup.interactable = true;
        mainMenuGroup.blocksRaycasts = true;

        optionsContents.SetActive(false);
        optionsButton.SetActive(false);
        attributionsButton.SetActive(false);
        creditsButton.SetActive(false);
        settingsButton.SetActive(false);
        optionsBg.SetActive(false);
    }

    private void HideAllWindows()
    {
        settingsOptions.SetActive(false);
        audioSettingsOptions.SetActive(false);
        videoSettingsOptions.SetActive(false);
        accessibilitySettingsOptions.SetActive(false);
        controlsSettingsOptions.SetActive(false);
    }

    // ---------- Button Press Logic ----------

    public void StartButtonPressed()
    {
        // need to add animations
        DOTween.KillAll();
        if (SkipCutscene)
        {
            sceneLoader.LoadNewScene("_MVPTutorial");
        }
        else
        {
            sceneLoader.LoadNewScene("OpenCutscene");
        }
    }

    public void OptionsButtonPressed()
    {
        if (optionsShown) return; // dont animate again if its already shown

        // kill all tweens relating to the camera
        DOTween.Kill("Camera");
        DOTween.Kill("ScreenOnOff");

        // animations
        titleText.GetComponent<TMP_Text>().DOFade(0f, 0.2f); // fade out title text

        mainCamera.transform.DOMove(cameraSettingsPos.position, 0.5f).SetId("Camera");
        mainCamera.transform.DORotate(new Vector3(0f, 0, 0), 0.5f).SetId("Camera").OnComplete(() =>
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
                    attributionsButton.SetActive(true);
                    creditsButton.SetActive(true);
                    //blackScreen.SetActive(false); // hide the black screen

                    turnOnEffect.GetComponent<Image>().DOFade(0f, 0.2f).SetId("ScreenOnOff").OnComplete(() =>
                    {
                        turnOnEffect.SetActive(false);
                    });
                });
            });
        });

        optionsShown = true;
        optionsContents.SetActive(true);
    }

    public void OptionsBackButtonPressed()
    {
        // kill all tweens relating to the camera
        DOTween.Kill("Camera");
        DOTween.Kill("ScreenOnOff");

        turnOnEffect.SetActive(true);
        //blackScreen.SetActive(true);

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
            titleText.GetComponent<Image>().DOFade(1f, 0.2f);
        });

        optionsShown = false;
        optionsContents.SetActive(false);
        optionsButton.SetActive(false);
        settingsButton.SetActive(false);
        optionsBg.SetActive(false);
    }


    public void OnWindowButtonPressed(GameObject window)
    {
        defaultSelection.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        //accessibilityButton.SetActive(true);

        DOTween.Kill("WindowScale" + window.name);

        // Pick the controller selection for the window being opened.
        GameObject selectionForWindow = null;

        switch (window)
        {
            case var w when w == settingsOptions:
                selectionForWindow = settingsMenuDefaultSelection;
                break;

            case var w when w == audioSettingsOptions:
                selectionForWindow = audioMenuDefaultSelection;
                break;

            case var w when w == videoSettingsOptions:
                selectionForWindow = videoMenuDefaultSelection;
                break;

            case var w when w == accessibilitySettingsOptions:
                selectionForWindow = accessibilityMenuDefaultSelection;
                break;

            case var w when w == controlsSettingsOptions:
                selectionForWindow = controlsMenuDefaultSelection;
                break;
        }

        // Animate the window in. It must be active before selecting one of its buttons.
        Sequence windowScaleSequence = DOTween.Sequence().SetId("WindowScale" + window.name);

        windowScaleSequence.Append(window.transform.DOScale(Vector3.zero, 0f).OnComplete(() => window.SetActive(true)));

        windowScaleSequence.Append(window.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutSine).OnComplete(() =>
                {
                    if (selectionForWindow != null && selectionForWindow.activeInHierarchy)
                    {
                        EventSystem.current.SetSelectedGameObject(selectionForWindow);
                    }
                })
        );

        if (optionsShown)
        {
            mainMenuGroup.interactable = false;
            mainMenuGroup.blocksRaycasts = false;

            if (window == settingsOptions)
            {
                // The Settings page is open, so the TV's monitor buttons should not respond.
                monitorButtonsGroup.interactable = false;
                monitorButtonsGroup.blocksRaycasts = false;
            }
            else
            {
                // A category page is open.
                settingsGroup.interactable = false;
                settingsGroup.blocksRaycasts = false;

                if (currentWindow == settingsOptions)
                {
                    monitorButtonsGroup.interactable = false;
                    monitorButtonsGroup.blocksRaycasts = false;
                }
            }
        }

        currentWindow = window;
        previousWindows.Add(window);
    }

    public void OnBackPressed()
    {
        if (previousWindows.Count > 0) // more than the options screen displayed
        {
            GameObject referenceToCurrentWindow = currentWindow; // so when current window changes reference to old one stays the same
            DOTween.Kill("WindowScale" + referenceToCurrentWindow.name);


            Sequence windowScaleSequence = DOTween.Sequence().SetId("WindowScale" + referenceToCurrentWindow.name);

            windowScaleSequence.Append(referenceToCurrentWindow.transform.DOScale(new Vector3(1, 1, 1), 0f));
            windowScaleSequence.Append(referenceToCurrentWindow.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                referenceToCurrentWindow.SetActive(false);
            }));

            if (previousWindows.Count == 1) // if only 1 window up, itll go back to the base screen which isnt a window so set current to null
            {
                currentWindow = null;
            }
            else
            {
                currentWindow = previousWindows[previousWindows.Count - 2]; // go back to last window
            }   
            previousWindows.Remove(previousWindows[previousWindows.Count - 1]); // delete most recently visited window

            if (currentWindow == null)
            {
                // Back at the monitor. Select its Settings button, not defaultSelection.
                defaultSelection.SetActive(false);

                if (settingsButton.activeInHierarchy)
                {
                    EventSystem.current.SetSelectedGameObject(settingsButton);
                }
            }
            else if (currentWindow == settingsOptions)
            {
                // Back at the Settings menu.
                defaultSelection.SetActive(false);
                EventSystem.current.SetSelectedGameObject(settingsMenuDefaultSelection);
            }



            if (optionsShown && currentWindow == null)
            {
                monitorButtonsGroup.interactable = true;
                monitorButtonsGroup.blocksRaycasts = true;

                // Keep the off-screen main menu buttons disabled while the TV/options screen is open.
                mainMenuGroup.interactable = false;
                mainMenuGroup.blocksRaycasts = false;
            }

            if (currentWindow == settingsOptions || currentWindow == null)
            {
                settingsGroup.interactable = true;
                settingsGroup.blocksRaycasts = true;
            }
        }
        else
        {
            SettingsScreenExited();
        }
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
    }
}
