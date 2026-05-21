using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SliderUpdateExternalValue : MonoBehaviour
{
    [SerializeField] private TMP_Text displayedValueText;

    [SerializeField] private UnityEngine.UI.Slider slider;

    private int newValue = 0;

    [SerializeField] private int maxValue = 10;

    [SerializeField] private AudioClip sliderMoveClip;
    private bool isMouseDown = false;
    private float clipTimer = 0f;

    private void Start()
    {
        newValue = (int)(slider.value * maxValue);
        displayedValueText.text = newValue.ToString();
    }

    public void ChangeExternalSliderValue()
    {
        newValue = (int)(slider.value * maxValue);

        displayedValueText.text = newValue.ToString();
    }

    public void PointerDown()
    {
        isMouseDown = true;
        
    }

    public void PointerUp()
    {
        isMouseDown = false;
        clipTimer = 0;
    }

    private void Update()
    {
        if (isMouseDown)
        {
            if (clipTimer >= 0.2)
            {
                AudioManager.Instance.PlaySFX(sliderMoveClip, transform, 1);
                clipTimer = 0;
            }

            clipTimer += Time.deltaTime;
        }
    }
}
