using UnityEngine;
using DG.Tweening;


public class PlatformSwitch : MonoBehaviour, IInteractable
{
    public GameObject bridge;
    public GameObject leverHandle;
    public GameObject leverBase;

    public GameObject bridgePivot;
    public float rotationFloat;

    private bool isDown;

    public void OnInteract()
    {
        leverHandle.transform.DOLocalRotate(new Vector3(rotationFloat, 0, 0), 0.25f).OnComplete(() =>
        {
            //bridge.SetActive(!bridge.activeSelf);
            bridgePivot.transform.DOLocalRotate(new Vector3(2, 58, 3), 1f);
        });

        isDown = true;
        leverHandle.layer = 0;
        leverBase.layer = 0;

    }

    public void ActivateOutline()
    {
        if (isDown) return;
        leverHandle.layer = 30;
        leverBase.layer = 30;
    }

    public void DeactivateOutline()
    {
        leverHandle.layer = 0;
        leverBase.layer = 0;
    }

    /*
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
         PostGameDataLog.lightMazeInteractInt++;
         if(platforms.Length > 0) // For all platforms that the switch flips
         {
             for(int i = 0; i < platforms.Length; i++)
             {
                 platforms[i].SetActive(!platforms[i].activeSelf); // Flips the active state (active -> inactive and vice versa)
             }
         }
     }*/
}
