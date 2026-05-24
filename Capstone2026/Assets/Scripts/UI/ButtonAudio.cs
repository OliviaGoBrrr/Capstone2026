using UnityEngine;

public class ButtonAudio : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip buttonHoverClip;
    [SerializeField] private AudioClip buttonPressClip;

    public void MouseEntered()
    {
        AudioManager.Instance.PlaySFX(buttonHoverClip, transform, 1);
    }

    public void OnPressed()
    {
        AudioManager.Instance.PlaySFX(buttonPressClip, transform, 1);
    }

}
