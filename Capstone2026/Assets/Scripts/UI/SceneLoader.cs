using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 0.75f;
    public void LoadNewScene(string nextScene)
    {
        StartCoroutine(LoadScene(nextScene));
    }

    IEnumerator LoadScene(string nextScene)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSecondsRealtime(transitionTime);

        PostGameDataLog.updateSceneTime($"{SceneManager.GetActiveScene().name} " + "Time in level: " + $"{Time.timeSinceLevelLoad}");

        SceneManager.LoadScene(nextScene);

        Debug.Log(nextScene);

        Time.timeScale = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "_MVPFarmLevel")
        {
            StartCoroutine(LoadScene("_MVPDock"));
        }
        else
        {
            StartCoroutine(LoadScene("_MVPFarmLevel-A"));
        }
    }
}
