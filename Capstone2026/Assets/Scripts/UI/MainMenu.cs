using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public InputActionReference pauseAction;

    [SerializeField] private GameObject settingsOptions;
    [SerializeField] private GameObject settingsButton;

    [SerializeField] private GameObject mainCamera;

    private bool settingsShown = false;

    [SerializeField] private Transform cameraSettingsPos;
    [SerializeField] private Transform cameraNormalPos;

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
                // kill all tweens relating to the camera
                DOTween.Kill("Camera");

                mainCamera.transform.DOMove(cameraNormalPos.position, 0.5f);
                mainCamera.transform.DORotate(new Vector3(cameraNormalPos.rotation.x, cameraNormalPos.rotation.y, cameraNormalPos.rotation.z), 0.5f);

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
        // kill all tweens relating to the camera
        DOTween.Kill("Camera");

        mainCamera.transform.DOMove(cameraSettingsPos.position, 0.5f);
        mainCamera.transform.DORotate(new Vector3(-12, 0, -5), 0.5f);

        settingsShown = true;
        settingsOptions.SetActive(true);
        settingsButton.SetActive(false);
    }

    public void SettingsBackButtonPressed()
    {
        // kill all tweens relating to the camera
        DOTween.Kill("Camera");

        mainCamera.transform.DOMove(cameraNormalPos.position, 0.5f);
        mainCamera.transform.DORotate(new Vector3(cameraNormalPos.rotation.x, cameraNormalPos.rotation.y, cameraNormalPos.rotation.z), 0.5f);

        settingsShown = false;
        settingsOptions.SetActive(false);
        settingsButton.SetActive(true);
    }

    public void ExitButtonPressed()
    {

    }
}
