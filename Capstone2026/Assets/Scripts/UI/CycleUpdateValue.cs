using TMPro;
using UnityEngine;

public class CycleUpdateValue : MonoBehaviour
{
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;

    [SerializeField] private string[] displayableTextArr;
    private int currentDisplayableTextPosition;

    [SerializeField] private int posOfInitialText;

    [SerializeField] private TMP_Text displayedText;

    [SerializeField] private bool allowWrapping;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        displayedText.text = displayableTextArr[posOfInitialText];

        currentDisplayableTextPosition = posOfInitialText;


        if (allowWrapping) return;

        if (currentDisplayableTextPosition >= displayableTextArr.Length - 1)
        {
            rightButton.SetActive(false);
        }

        if (currentDisplayableTextPosition <= 0)
        {
            leftButton.SetActive(false);
        }
    }

    public void CycleThroughListForward()
    {
        leftButton.SetActive(true);

        int newDisplayTextPosition = currentDisplayableTextPosition + 1;

        if (newDisplayTextPosition >= displayableTextArr.Length)
        {
            if (allowWrapping)
            {
                newDisplayTextPosition = 0;
            }
            else
            {
                return;
            }
        }
        
        displayedText.text = displayableTextArr[newDisplayTextPosition];

        currentDisplayableTextPosition = newDisplayTextPosition;

        if (newDisplayTextPosition >= displayableTextArr.Length - 1 && !allowWrapping)
        {
            rightButton.SetActive(false);
        }
    }

    public void CycleThroughListBackward()
    {
        rightButton.SetActive(true);

        int newDisplayTextPosition = currentDisplayableTextPosition - 1;

        if (newDisplayTextPosition < 0)
        {
            if (allowWrapping)
            {
                newDisplayTextPosition = displayableTextArr.Length - 1;
            }
            else
            {
                return;
            }
        }
        
        displayedText.text = displayableTextArr[newDisplayTextPosition];

        currentDisplayableTextPosition = newDisplayTextPosition;

        if (currentDisplayableTextPosition <= 0 && !allowWrapping)
        {
            leftButton.SetActive(false);
        }
    }
}
