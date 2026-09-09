using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FarmIntro : CameraSequencer
{
    [Header("UI Variables")]
    public float inSequenceBarHeight = 70f;
    public Image topBar, bottomBar, fadeScreen;

    [Header("Letter Box Effect Variables")]
    public float FadeInTime;
    public float FadeOutTime;
    public float FadeDelay;


    [Header("Audio Variables")]
    public AudioClip fadeInClip;
    public AudioClip fadeOutClip;
    public AudioSource introAduioSource;

    public override void StartSequence()
    {
        StartCoroutine(LetterBoxIn(inSequenceBarHeight, 0.2f));
        base.StartSequence();
    }

    public override void CancelCameraSequence()
    {
        StartCoroutine(FadeInOutCamera());
    }

    // Coroutine to make sure that certain functions go off after other coroutines
    IEnumerator FadeInOutCamera()
    {
        introAduioSource.PlayOneShot(fadeInClip);

        yield return LetterBoxIn(545f, FadeInTime);

        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        playerCamera.gameObject.SetActive(true);

        playSequence = false;

        yield return new WaitForSeconds(FadeDelay);

        introAduioSource.PlayOneShot(fadeOutClip);

        yield return LetterBoxOut(0.1f, FadeOutTime);
    }

    #region UI Coroutines
    IEnumerator LetterBoxInOut(float fadeToHeight, float totalTime = 1f)
    {
        topBar.gameObject.SetActive(true);
        bottomBar.gameObject.SetActive(true);

        Vector2 newHeight = bottomBar.rectTransform.sizeDelta;

        float initialHeight = newHeight.y;

        for (float currentHeight = initialHeight; currentHeight < fadeToHeight; currentHeight += ((fadeToHeight * (Time.deltaTime / (totalTime / 2f)))))
        {
            newHeight.y = currentHeight;
            topBar.rectTransform.sizeDelta = newHeight;
            bottomBar.rectTransform.sizeDelta = newHeight;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        float topHeight = bottomBar.rectTransform.sizeDelta.y;

        for (float currentHeight = topHeight; currentHeight > 0f; currentHeight -= ((fadeToHeight * (Time.deltaTime / (totalTime / 2f)))))
        {
            newHeight.y = currentHeight;
            topBar.rectTransform.sizeDelta = newHeight;
            bottomBar.rectTransform.sizeDelta = newHeight;
            yield return null;
        }
    }

    IEnumerator LetterBoxIn(float fadeToHeight, float fadeInTime = 0.5f)
    {
        topBar.gameObject.SetActive(true);
        bottomBar.gameObject.SetActive(true);

        Vector2 newHeight = bottomBar.rectTransform.sizeDelta;

        float initialHeight = newHeight.y;

        Debug.Log("Fading in");

        for (float currentHeight = initialHeight; currentHeight < fadeToHeight; currentHeight += ((fadeToHeight * (Time.deltaTime / fadeInTime))))
        {
            newHeight.y = currentHeight;
            topBar.rectTransform.sizeDelta = newHeight;
            bottomBar.rectTransform.sizeDelta = newHeight;
            yield return null;
        }
    }

    IEnumerator LetterBoxOut(float fadeToHeight, float fadeInTime = 0.5f)
    {

        Vector2 newHeight = bottomBar.rectTransform.sizeDelta;

        float initialHeight = newHeight.y;

        Debug.Log("Fading out");

        for (float currentHeight = initialHeight; currentHeight > fadeToHeight; currentHeight -= ((initialHeight/fadeInTime) * Time.deltaTime))
        {
            newHeight.y = currentHeight;
            topBar.rectTransform.sizeDelta = newHeight;
            bottomBar.rectTransform.sizeDelta = newHeight;

            yield return null;
        }

        topBar.gameObject.SetActive(false);
        bottomBar.gameObject.SetActive(false);
    }

    #endregion

}
