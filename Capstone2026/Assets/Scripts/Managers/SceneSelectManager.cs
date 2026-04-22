using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectManager : MonoBehaviour
{
    public void GoToLevelA()
    {
        SceneManager.LoadScene("MVP_Level-A");
    }

    public void GoToGym()
    {
        SceneManager.LoadScene("Environment");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
