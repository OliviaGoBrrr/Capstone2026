using UnityEngine;

public class blueSwitchTrigger_LightMaze : MonoBehaviour
{
    public GameObject BluePlatform_A, BluePlatform_B, BluePlatform_B_Blocker, RedPlatform, Switch_B;
    bool state;

    void Start()
    {
        BluePlatform_B.SetActive(false);
        BluePlatform_A.SetActive(true);
	Switch_B.SetActive(state);
    }

    void OnTriggerExit(Collider other)
    {
        state = !state;
        BluePlatform_A.SetActive(!state);
        BluePlatform_B.SetActive(state);
        if (!RedPlatform.activeSelf) BluePlatform_B_Blocker.SetActive(true);
	else BluePlatform_B_Blocker.SetActive(false);
        Switch_B.SetActive(state);
    }
}
