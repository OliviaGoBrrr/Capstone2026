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

        //yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(DURATION));

        //yield return new WaitForSeconds(transitionTime);

        //StartCoroutine(WaitForRealSeconds(transitionTime));

        yield return new WaitForSecondsRealtime(transitionTime);

        SceneManager.LoadScene(nextScene);

        Time.timeScale = 1;
    }

    IEnumerator WaitForRealSeconds(float seconds)
    {
        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - startTime < seconds)
        {
            yield return null;
        }
    }
}
