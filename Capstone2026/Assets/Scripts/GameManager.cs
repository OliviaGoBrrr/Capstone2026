using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The problem is I can't get the value for fullscreenMode AND resolutionSelection to be displayed in the CycleOption when you go from the main menu to the tutorial, it resets to the defaults, as written in the scripts. I can't think of an easy way to fix this.
    /// </summary>

    //public PlayerManager PlayerManager;
    bool inCutscene = false;
    public static GameManager gameManager { get; private set; }

    [Header("Settings")]
    public int fullscreenMode;
    public int resolutionSelection;
    [SerializeField] public CycleUpdateValue fullscreenScript; ///the thought was that I could reference this and the one below to only receive info in the main menu - this works.
    [SerializeField] public CycleUpdateValue resolutionScript;

    [Header("Questing")]
    public bool pickedUpCabbages = false;

    private void Awake()
    {
        if (gameManager != null && gameManager != this)
        {
            Destroy(gameObject);
            return;
        }
        gameManager = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene(); ///grabs current scene
        if (currentScene.name == "MainMenu") ///if in main menu, set the "fullscreenMode" int to the choice set in the main menu.
        {
            fullscreenMode = fullscreenScript.currentDisplayableTextPosition;
        }
        switch (fullscreenMode)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen; //Exclusive Fullscreen
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow; //Borderless Fullscreen
                break;
            case 2:
                Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.Windowed); //Windowed
                break;
            default:
                break;
        }
        if (currentScene.name == "MainMenu") //if in main menu, set the "resolutionSelection" int to the choice set in the main menu.
        {
            resolutionSelection = resolutionScript.currentDisplayableTextPosition;
        }
        switch (resolutionSelection)
        {
            case 0:
                Screen.SetResolution(1280, 720, Screen.fullScreenMode);
                break;
            case 1:
                Screen.SetResolution(1366, 768, Screen.fullScreenMode);
                break;
            case 2:
                Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
                break;
            case 3:
                Screen.SetResolution(2560, 1440, Screen.fullScreenMode);
                break;
            default:
                break;
        }
    }
}
