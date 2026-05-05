using UnityEngine;

public class redSwitchTrigger_LightMaze : MonoBehaviour
{
    public GameObject RedPlatform, RedPlatform_Blocker, BluePlatform_B_Blocker;
    bool state;

    void Start()
    {
        RedPlatform.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        state = !state;
        RedPlatform.SetActive(state);
        RedPlatform_Blocker.SetActive(false);
        BluePlatform_B_Blocker.SetActive(false);
    }
}
