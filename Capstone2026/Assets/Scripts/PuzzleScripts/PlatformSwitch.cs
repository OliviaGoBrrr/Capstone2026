using UnityEngine;

public class PlatformSwitch : MonoBehaviour, IInteractable
{
    public GameObject[] platforms;
    public GameObject[] flipOnAwake;

    private void Awake()
    {
        if(flipOnAwake.Length > 0) 
        {
            for(int i = 0; i < flipOnAwake.Length; i++)
            {
                flipOnAwake[i].SetActive(!flipOnAwake[i].activeSelf);
            }
        }
    }
    public void OnInteract()
    {
        if(platforms.Length > 0) // For all platforms that the switch flips
        {
            for(int i = 0; i < platforms.Length; i++)
            {
                platforms[i].SetActive(!platforms[i].activeSelf); // Flips the active state (active -> inactive and vice versa)
            }
        }
    }
}
