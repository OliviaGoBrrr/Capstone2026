using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused;
    public InputActionReference pauseAction;

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
                Time.timeScale = 0;

                Cursor.visible = true;

                // replace with exit animation
                pauseMenu.SetActive(true);

                isPaused = true;
            }
        }
    }

    private void ResumeLogic()
    {
        Time.timeScale = 1;

        // replace with exit animation
        HideAllUI();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        isPaused = false;
    }

    // ---------- Button Press Logic ----------

    public void OnResumePressed()
    {
        ResumeLogic();
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
        // need to add animations
        SceneManager.LoadScene("MainMenu");
    }
}
