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
    [SerializeField] private GameObject settingsOptions;

    private GameObject currentlyShownOptions;

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
        HideCursor();
        currentlyShownOptions = null;
    }

    void Update()
    {
        if (pauseAction.action.WasPressedThisFrame())
        {
            if (isPaused)
            {
                ResumeLogic();
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
    }

    private void ResumeLogic()
    {
        Time.timeScale = 1;
        playerInputMap.FindActionMap("Player").Enable();
        // replace with exit animation
        HideAllUI();

        isPaused = false;
    }

    // ---------- Button Press Logic ----------

    public void OnResumePressed()
    {
        ResumeLogic();
        HideCursor();
    }

    public void OnSettingsPressed()
    {
        // if settings is already displayed, hide it else display!
        if (currentlyShownOptions == settingsOptions)
        {
            settingsOptions.SetActive(false);
            currentlyShownOptions = null;
        }
        else
        {
            settingsOptions.SetActive(true);
            currentlyShownOptions = settingsOptions;
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
