using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The problem is I can't get the value for fullscreenMode AND resolutionSelection to be displayed in the CycleOption when you go from the main menu to the tutorial, it resets to the defaults, as written in the scripts. I can't think of an easy way to fix this.
    /// </summary>

    //public PlayerManager PlayerManager;
    bool inCutscene = false;
    public static GameManager gameManager { get; private set; }

    [Header("Tutorial Controller")]
    [SerializeField] private GameObject DialogueE;
    [SerializeField] private GameObject DialogueL1;

    [Header("Farm Controller")]
    [SerializeField] private GameObject FarmE1;
    [SerializeField] private GameObject FarmE2;
    [SerializeField] private GameObject FarmE3;
    [SerializeField] private GameObject FarmE4;
    [SerializeField] private GameObject FarmE5;
    [SerializeField] private GameObject FarmE6;
    [SerializeField] private GameObject FarmE7;
    [SerializeField] private GameObject FarmE8;
    [SerializeField] private GameObject FarmR1;     
    [SerializeField] private GameObject FarmR2;
    [SerializeField] private GameObject FarmR3;
    [SerializeField] private GameObject FarmR4;
    [SerializeField] private GameObject FarmR5;
    [SerializeField] private GameObject FarmR6;
    [SerializeField] private GameObject FarmR7;
    [SerializeField] private GameObject FarmR8;


    [Header("Settings")]
    public int fullscreenMode;
    public int resolutionSelection;
    [SerializeField] public CycleUpdateValue fullscreenScript; ///the thought was that I could reference this and the one below to only receive info in the main menu - this works.
    [SerializeField] public CycleUpdateValue resolutionScript;

    [Header("Questing")]
    public bool pickedUpCabbages = false;
    public bool pickedUpMotor = false;

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
        if (currentScene.name == "_MVPDockHub" && Gamepad.current != null) //if controller is connected
        {
            DialogueE.SetActive(false);
            DialogueL1.SetActive(true);
        }
        else if (currentScene.name == "_MVPDockHub" && Gamepad.current == null) //if keyboard
        {
            DialogueE.SetActive(true);
            DialogueL1.SetActive(false);
        }

        if (currentScene.name == "_MVPFarmLevel" && Gamepad.current != null) //if controller is connected
        {
            FarmR1.SetActive(true);
            FarmR2.SetActive(true);
            FarmR3.SetActive(true);
            FarmR4.SetActive(true);

            FarmE1.SetActive(false);
            FarmE2.SetActive(false);
            FarmE3.SetActive(false);
            FarmE4.SetActive(false);
            FarmE5.SetActive(false);

        }
        else if (currentScene.name == "_MVPFarmLevel" && Gamepad.current == null)//if keyboard
        {
            FarmE1.SetActive(true);
            FarmE2.SetActive(true);
            FarmE3.SetActive(true);
            FarmE4.SetActive(true);
            FarmE5.SetActive(true);

            FarmR1.SetActive(false);
            FarmR2.SetActive(false);
            FarmR3.SetActive(false);
            FarmR4.SetActive(false);
            FarmR5.SetActive(false);

        }


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
