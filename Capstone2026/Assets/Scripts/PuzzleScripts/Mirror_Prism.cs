using Unity.VisualScripting;
using UnityEngine;

public class Mirror_Prism : MonoBehaviour
{
    int prismHitCount;
    public bool isReflectMirror;
    public Animator AnimController;
    public GameObject grapplePoint;
    [HideInInspector] public bool reflectMode;

    private void Update()
    {
        if (!isReflectMirror)
        {
            if (prismHitCount == 2 && !reflectMode || Input.GetKeyDown(KeyCode.Return) && !reflectMode)
            {
                reflectMode = true;
                AnimController.Play("mirrorLight");
            }
        }
    }

    public void PrismHit()
    {
        prismHitCount++;
        Debug.Log("Count = " + prismHitCount);
    }

    public void spawnGrapple()
    {
        grapplePoint.SetActive(true);
    }
}