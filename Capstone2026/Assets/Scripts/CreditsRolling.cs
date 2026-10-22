using UnityEngine;

public class CreditsRolling : MonoBehaviour
{
    //Adjust speed of credits scrolling
    public float scrollSpeed = 40f;     

    private RectTransform rectTransform; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get Recttransform component of UI element
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move text upwards over time
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
    }
}
