using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectManager : MonoBehaviour
{
    public SceneLoader sceneLoader;

    public void GoToLevelA()
    {
        sceneLoader.LoadNewScene("_MVPFarmLevel");
    }

    public void GoToGym()
    {
        sceneLoader.LoadNewScene("Environment");
    }

    public void GoToMainMenu()
    {
        sceneLoader.LoadNewScene("MainMenu");
    }

    public void GoToDock()
    {
        sceneLoader.LoadNewScene("_MVPDockHub");
    }

    public void GoToTutorial()
    {
        sceneLoader.LoadNewScene("_MVPTutorial");
    }
}
