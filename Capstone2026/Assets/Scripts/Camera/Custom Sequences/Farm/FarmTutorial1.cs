using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FarmTutorial1 : CameraSequencer
{
    [Header("UI Variables")]
    public Image TopBar;
    public Image BottomBar;
    public Image fadeScreen;
    public Canvas TransitionCanvas;
    public float InSequenceBarHeight = 70f;

    [Header("Tutorial UI Variables")]
    public TMP_Text TutorialTextBox;
    public string[] TutorialInstructionTexts;

    [Header("Letter Box Effect Variables")]
    public float FadeInTime;
    public float FadeOutTime;
    public float FadeDelay;

    public override void StartSequence()
    {
        try { CheckTutorialErrors(); }
        catch (Exception e)
        {
            Debug.LogError($"({this.name}) Tutorial Failed To Load - Error: {e}");
            return;
        }

        StartCoroutine(LetterBoxIn(InSequenceBarHeight, 0.3f));
        base.StartSequence();
    }

    public override void NextCameraInSequence()
    {
        base.NextCameraInSequence();
    }


    private void CheckTutorialErrors()
    {
        if(GetComponentInChildren<Collider>() == null)
        {
            throw new ArgumentNullException(paramName: this.gameObject.name, message: "Tutorial doesn't have a collider for triggers.\n Please create a collider and make it a child of the camera sequence");
        }
    }

    public override void CancelCameraSequence()
    {
        StartCoroutine(LetterBoxOut(0.1f, FadeOutTime));
        base.CancelCameraSequence();
    }

    IEnumerator LetterBoxIn(float fadeToHeight, float fadeInTime = 0.5f)
    {
        TopBar.gameObject.SetActive(true);
        BottomBar.gameObject.SetActive(true);

        Vector2 newHeight = BottomBar.rectTransform.sizeDelta;

        float initialHeight = newHeight.y;

        Debug.Log("Fading in");

        for (float currentHeight = initialHeight; currentHeight < fadeToHeight; currentHeight += (fadeToHeight / (fadeInTime / Time.fixedDeltaTime)))
        {
            newHeight.y = currentHeight;
            TopBar.rectTransform.sizeDelta = newHeight;
            BottomBar.rectTransform.sizeDelta = newHeight;
            yield return null;
        }

        newHeight.y = fadeToHeight;
        TopBar.rectTransform.sizeDelta = newHeight;
        BottomBar.rectTransform.sizeDelta = newHeight;
    }

    IEnumerator LetterBoxOut(float fadeToHeight, float fadeOutTime = 0.5f)
    {

        Vector2 newHeight = BottomBar.rectTransform.sizeDelta;

        float initialHeight = newHeight.y;

        Debug.Log("Fading out");

        for (float currentHeight = initialHeight; currentHeight > fadeToHeight; currentHeight -= (fadeToHeight * (fadeOutTime / Time.fixedDeltaTime)))
        {
            newHeight.y = currentHeight;
            TopBar.rectTransform.sizeDelta = newHeight;
            BottomBar.rectTransform.sizeDelta = newHeight;

            yield return null;
        }

        TopBar.gameObject.SetActive(false);
        BottomBar.gameObject.SetActive(false);
    }

}
