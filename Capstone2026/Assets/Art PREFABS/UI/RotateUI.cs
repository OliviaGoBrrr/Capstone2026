using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Import DOTween

public class RotateUI : MonoBehaviour
{
    [SerializeField] private Canvas Canvas;
    [SerializeField] public Image SpeechBubble;   
    [SerializeField] public Image E;     

    private void Awake()
    {
        Canvas.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (Canvas.gameObject.activeInHierarchy)
        {
            //RotateTowardsPlayer();
        }
    }

    private void OnTriggerEnter(Collider other) // fade in
    {
        DOTween.Kill("EFade");
        DOTween.Kill("BubbleFade");

        Canvas.gameObject.SetActive(true);

        E.DOFade(1f, 0.25f).SetEase(Ease.OutCubic).SetId("EFade");
        E.DOFade(1f, 0.25f).SetEase(Ease.OutCubic).SetId("BubbleFade");
    }

    private void OnTriggerExit(Collider other) // fade out
    {
        DOTween.Kill("EFade");
        DOTween.Kill("BubbleFade");

        E.DOFade(0f, 0.25f).SetEase(Ease.OutCubic).SetId("EFade");

        SpeechBubble.DOFade(0f, 0.25f).SetEase(Ease.OutCubic).SetId("BubbleFade").OnComplete(() =>
        {
            Canvas.gameObject.SetActive(false);
        });
    }

    private void RotateTowardsPlayer()
    {
        Canvas.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
}
