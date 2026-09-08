using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FarmIntro : CameraSequencer
{
    [Header("UI Variables")]
    public float inSequenceBarHeight = 70f;
    public Image topBar, bottomBar, fadeScreen;

    public override void StartSequence()
    {
        StartCoroutine(LetterBoxIn(inSequenceBarHeight, 0.3f));
        base.StartSequence();
    }

    public override void CancelCameraSequence()
    {
        StartCoroutine(FadeInOutCamera());
    }

    // Coroutine to make sure that certain functions go off after other coroutines
    IEnumerator FadeInOutCamera()
    {
        yield return StartCoroutine(LetterBoxIn(Screen.currentResolution.height / 2f));

        base.CancelCameraSequence();

        yield return StartCoroutine(LetterBoxOut(0.1f));

        Debug.Log("Giving Player Movement");

        StopCoroutine(FadeInOutCamera());
    }

    #region UI Coroutines
    IEnumerator LetterBoxInOut(float fadeToHeight, float totalTime = 1f)
    {
        Debug.Log("Letter Box Fading In");
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
        Debug.Log("Letter Box Fading In");
        topBar.gameObject.SetActive(true);
        bottomBar.gameObject.SetActive(true);

        Vector2 newHeight = bottomBar.rectTransform.sizeDelta;

        float initialHeight = newHeight.y;

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
        Debug.Log("Letter Box Fading Out");

        Vector2 newHeight = bottomBar.rectTransform.sizeDelta;

        float initialHeight = newHeight.y;

        Debug.Log(initialHeight);

        for (float currentHeight = initialHeight; currentHeight > fadeToHeight; currentHeight -= ((initialHeight/fadeInTime) * Time.deltaTime))
        {
            newHeight.y = currentHeight;
            topBar.rectTransform.sizeDelta = newHeight;
            bottomBar.rectTransform.sizeDelta = newHeight;

            Debug.Log("Doing the shrinking");

            yield return null;
        }

        topBar.gameObject.SetActive(false);
        bottomBar.gameObject.SetActive(false);
    }

    #endregion

}
