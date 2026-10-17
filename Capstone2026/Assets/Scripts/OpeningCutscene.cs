using UnityEngine;
using UnityEngine.Video;

public class OpenCutsceneScript : MonoBehaviour
{
    public VideoPlayer cutscene;
    public SceneLoader sceneLoader;
    void Start()
    {

        cutscene.loopPointReached += OnLoopPointReached;

        cutscene.Play();
    }

    void OnLoopPointReached(VideoPlayer vp)
    {
        sceneLoader.LoadNewScene("_MVPTutorial");
        Debug.Log("Loop Finished");
    }
}
