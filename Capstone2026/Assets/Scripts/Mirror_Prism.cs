using Unity.VisualScripting;
using UnityEngine;

public class Mirror_Prism : MonoBehaviour
{
    int prismHitCount;
    public Animator AnimController;
    public GameObject grapplePoint;
    bool hasHit;

    private void Update()
    {
        if (prismHitCount == 2 && !hasHit)
        {
            hasHit = true;
            AnimController.Play("mirrorLight");
        }
    }

    public void PrismHit()
    {
        Debug.Log("Hit");
        prismHitCount++;
    }

    public void spawnGrapple()
    {
        grapplePoint.SetActive(true);
    }

}