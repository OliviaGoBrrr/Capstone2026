using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public InputActionReference pauseAction;

    [SerializeField] private GameObject settingsOptions;
    [SerializeField] private GameObject settingsButton;

    private bool settingsShown = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settingsOptions.SetActive(false);
        settingsButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseAction.action.WasPressedThisFrame())
        {
            if (settingsShown)
            {
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
        SceneManager.LoadScene("Environment");
    }

    public void SettingsButtonPressed()
    {
        settingsShown = true;
        settingsOptions.SetActive(true);
        settingsButton.SetActive(false);
    }

    public void SettingsBackButtonPressed()
    {
        settingsShown = false;
        settingsOptions.SetActive(false);
        settingsButton.SetActive(true);
    }

    public void ExitButtonPressed()
    {

    }
}
