using Unity.VisualScripting;
using UnityEngine;

public class Mirror_Prism : MonoBehaviour
{
    int prismHitCount, hasHit = 0;
    public Animator AnimController;

    private void Update()
    {

        if(prismHitCount == 2 && hasHit == 0)
        {
            hasHit = 1;
            AnimController.Play("mirrorLight");
        }
    }

    public void PrismHit()
    {
        Debug.Log("Hit");
        prismHitCount++;
    }
}
