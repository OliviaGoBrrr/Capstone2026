using UnityEngine;
using DG.Tweening;


public class PlatformSwitch : MonoBehaviour, IInteractable
{
    public GameObject leverHandle;
    public GameObject leverBase;


    public float bridgeLowerTime = 1f;
    public float bridgeLowerAngle = 87f;
    public GameObject bridgePivot;
    public float rotationFloat;

    private bool isDown;

    [Header("Audio")]
    public AudioClip BridgeLoweringSFX;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && !isDown)
        {
            LowerBridge();
        }
    }

    public void LowerBridge()
    {
        isDown = true;
        Vector3 bridgeRot = bridgePivot.transform.localRotation.eulerAngles;
        bridgePivot.transform.DOLocalRotate(new Vector3(bridgeLowerAngle, bridgeRot.y, bridgeRot.z), bridgeLowerTime);
    }

    public void OnInteract()
    {
        AudioManager.Instance.PlaySFX(BridgeLoweringSFX, transform, 0.4f);
        leverHandle.transform.DOLocalRotate(new Vector3(rotationFloat, 0, 0), 0.25f).OnComplete(() =>
        {
            //bridge.SetActive(!bridge.activeSelf);
            LowerBridge();
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
