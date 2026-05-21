using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class SliderUpdateExternalValue : MonoBehaviour
{
    [SerializeField] private TMP_Text displayedValueText;

    [SerializeField] private UnityEngine.UI.Slider slider;

    private int newValue = 0;

    [SerializeField] private int maxValue = 10;

    public void ChangeExternalSliderValue()
    {
        newValue = (int)(slider.value * maxValue);

        displayedValueText.text = newValue.ToString();
    }
}
