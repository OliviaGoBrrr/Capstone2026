using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectManager : MonoBehaviour
{
    public SceneLoader sceneLoader;

    public void GoToLevelA()
    {
        sceneLoader.LoadNewScene("MVP_Level-A");
    }

    public void GoToGym()
    {
        sceneLoader.LoadNewScene("Environment");
    }

    public void GoToMainMenu()
    {
        sceneLoader.LoadNewScene("MainMenu");
    }
}
