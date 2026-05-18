using UnityEngine;

public class SwitchPuzzleLogic : MonoBehaviour
{
    public GameObject redPlatform;
    public GameObject bluePlatform;

    public GameObject[] blockers;

    private void Update()
    {
        if(redPlatform.activeSelf && bluePlatform.activeSelf)
        {
            if(blockers.Length > 0)
            {
                for(int i = 0; i < blockers.Length; i++)
                {
                    blockers[i].SetActive(false);
                }
            }
        }
    }
}
