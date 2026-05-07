using UnityEngine;

public class blueSwitchTrigger_LightMaze : MonoBehaviour
{
    public GameObject BluePlatform_A, BluePlatform_B, BluePlatform_B_Blocker, RedPlatform_Blocker, RedPlatform, Switch_B;
    bool state;

    void Start()
    {
        BluePlatform_B.SetActive(false);
        BluePlatform_A.SetActive(true);
	RedPlatform.SetActive(false);
	Switch_B.SetActive(false);
    }

    void OnTriggerExit(Collider other)
    {
        state = !state;
        BluePlatform_A.SetActive(!state);
        BluePlatform_B.SetActive(state);
	if (BluePlatform_B.activeSelf || RedPlatform.activeSelf) BluePlatform_B_Blocker.SetActive(false);
        else BluePlatform_B_Blocker.SetActive(true);
        Switch_B.SetActive(state);
    }
}
