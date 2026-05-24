using System.Collections;
using UnityEngine;

public class DeathFade : MonoBehaviour
{
    public Animator transition;
    public PlayerManager playerM;

    public float transitionTime = 0.75f;
    public void deathFade()
    {
        PostGameDataLog.diedInt++; // putting this here for now cuz the deadstate triggers a Lot
        StartCoroutine(deathFadeCoroutine());
    }

    IEnumerator deathFadeCoroutine() // this needs to be improved a little bit but whatever
    {
        transition.SetTrigger("Start");

        //audio player shut down
        yield return new WaitForSeconds(0.75f);

        playerM.isDead = false;

        transition.SetTrigger("End");

        //yield return new WaitForSecondsRealtime(transitionTime);

        
    }
}
