using UnityEngine;
using UnityEngine.Video;

public class OpenCutsceneScript : MonoBehaviour
{
    public VideoPlayer cutscene;
    public SceneLoader sceneLoader;
    void Start()
    {
        if(cutscene.clip != null)
        {
            cutscene.loopPointReached += OnLoopPointReached;

            cutscene.Play();
        }

        else
        {
            sceneLoader.LoadNewScene("_MVPTutorial");
            Debug.LogError("Opening Cutscene Unable To Play - Missing Video File");
        }
    }

    void OnLoopPointReached(VideoPlayer vp)
    {
        sceneLoader.LoadNewScene("_MVPTutorial");
    }
}
