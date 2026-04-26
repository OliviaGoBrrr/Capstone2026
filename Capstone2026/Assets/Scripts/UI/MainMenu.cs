using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

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
