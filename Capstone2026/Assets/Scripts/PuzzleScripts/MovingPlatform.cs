using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool isPlayerOn = false;

    private void OnTriggerEnter(Collider other)
    {
        //other.transform.parent = transform;

        //other.transform.SetParent(transform);

        isPlayerOn = true;
    }

    private void OnTriggerExit(Collider other)
    {
        //other.transform.parent = null;

        //other.transform.SetParent(null);

        isPlayerOn = false;
    }
}
