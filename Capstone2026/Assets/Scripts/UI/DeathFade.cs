using System.Collections;
using UnityEngine;

public class DeathFade : MonoBehaviour
{
    public Animator transition;
    public PlayerManager playerM;

    public float transitionTime = 0.75f;
    public void deathFade()
    {
        StartCoroutine(deathFadeCoroutine());
    }

    IEnumerator deathFadeCoroutine()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSecondsRealtime(transitionTime);

        playerM.transform.position = playerM.lastCheckpoint;

        yield return new WaitForSecondsRealtime(transitionTime);

        transition.SetTrigger("End");

        yield return new WaitForSecondsRealtime(transitionTime);

        playerM.DeadState.ExitState();
    }
}
