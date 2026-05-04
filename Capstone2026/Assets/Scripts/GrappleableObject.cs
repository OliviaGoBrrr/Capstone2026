using UnityEngine;

public class GrappleableObject : MonoBehaviour
{
    public Transform anchorPoint;
    public Canvas grappleUICanvas;
    private Transform player;

    private void Awake()
    {
        if (grappleUICanvas == null)
        {
            grappleUICanvas = GetComponentInChildren<Canvas>();
            grappleUICanvas.gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if(grappleUICanvas != null)
        {
            RotateTowardsPlayer();
        }
    }

    private void RotateTowardsPlayer()
    {
        grappleUICanvas.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, 
            Camera.main.transform.rotation * Vector3.up);
    }
}
