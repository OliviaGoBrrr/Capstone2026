using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FarmIntro : CameraSequencer
{
    [Header("UI Variables")]
    public float inSequenceBarHeight = 70f;
    public Image topBar, bottomBar, fadeScreen;
    public Canvas transitionCanvas;

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

        Debug.Log(transitionCanvas.renderingDisplaySize.y);

        yield return LetterBoxIn((Screen.currentResolution.height / 2f) + 20f, FadeInTime);

        base.CancelCameraSequence();

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

        Debug.Log(fadeToHeight);

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

        Debug.Log(fadeToHeight);

        for (float currentHeight = initialHeight; currentHeight < fadeToHeight; currentHeight += (fadeToHeight / (fadeInTime / Time.deltaTime)))
        {
            newHeight.y = currentHeight;
            Debug.Log(currentHeight);
            topBar.rectTransform.sizeDelta = newHeight;
            bottomBar.rectTransform.sizeDelta = newHeight;
            yield return null;
        }
    }

    IEnumerator LetterBoxOut(float fadeToHeight, float fadeOutTime = 0.5f)
    {

        Vector2 newHeight = bottomBar.rectTransform.sizeDelta;

        float initialHeight = newHeight.y;

        for (float currentHeight = initialHeight; currentHeight > fadeToHeight; currentHeight -= (fadeToHeight * (fadeOutTime / Time.deltaTime)))
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
