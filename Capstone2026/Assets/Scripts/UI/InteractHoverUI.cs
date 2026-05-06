using System;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class InteractHoverUI : MonoBehaviour
{
    [SerializeField] private Canvas grappleUICanvas;
    [SerializeField] private TMP_Text interactText;    

    private void Awake()
    {
        grappleUICanvas.gameObject.SetActive(false);
        interactText.alpha = 0f;
    }

    private void LateUpdate()
    {
        if (grappleUICanvas.gameObject.activeInHierarchy)
        {
            RotateTowardsPlayer();
        }
    }

    private void OnTriggerEnter(Collider other) // fade in
    {
        DOTween.Kill("InteractTextFade");

        grappleUICanvas.gameObject.SetActive(true);

        interactText.DOFade(1f, 1f).SetEase(Ease.OutCubic).SetId("InteractTextFade");
    }

    private void OnTriggerExit(Collider other) // fade out
    {
        DOTween.Kill("InteractTextFade");

        interactText.DOFade(0f, 1f).SetEase(Ease.OutCubic).SetId("InteractTextFade").OnComplete(() =>
        {
            grappleUICanvas.gameObject.SetActive(false);
        });
    }

    private void RotateTowardsPlayer()
    {
        grappleUICanvas.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
}
