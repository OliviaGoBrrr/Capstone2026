using StarterAssets;
using UnityEngine;
using UnityEngine.Windows;

public class GrappleInteract : MonoBehaviour
{
    private Transform lookPoint;
    private Quaternion cameraRotation;
    private GameObject mainCamera;
    private LayerMask grappleLayer;

    public Transform playerTransform;

    public float grappleMaxDistance = 8.0f;
    public float grappleSpeed = 5.0f;
    public float grappleTimeout = 1.0f;
    public bool canGrapple;

    private float grappleTimer = 0.0f;
    private bool isGrappling = false;
    private RaycastHit grapplePoint;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        canGrapple = false;
        lookPoint = this.transform;
        grappleLayer = LayerMask.GetMask("GrappleSurface");
    }

    private void Start()
    {
        playerTransform = this.transform.parent.transform;
    }

    void Update() 
    {
        FindGrappleTarget();
    }

    private void LateUpdate()
    {
        FollowPlayerCamera();
    }

    private void FollowPlayerCamera()
    {
        cameraRotation = mainCamera.transform.rotation;
        lookPoint.transform.rotation = cameraRotation;
    }

    private void FindGrappleTarget()
    {
        canGrapple = false;
        Ray ray = new Ray(lookPoint.position, lookPoint.forward);

        if(Physics.Raycast(ray, out grapplePoint, grappleMaxDistance, grappleLayer))
        {
            canGrapple = true;
            Debug.Log($"Player can grapple to {grapplePoint.collider}");

            if(grappleTimer <= 0.0f)//  && player.input
            {
                grappleTimer = grappleTimeout;
                
                isGrappling = true;
                Debug.Log("Player grappled!");
            }
        }

        if (isGrappling)
        {
            GrapplePlayer();
        }

        // Stop the player from being able to grapple immediately
        if (grappleTimer > 0.0f)
        {
            grappleTimer -= Time.deltaTime;
        }

    }

    private void GrapplePlayer()
    {

    }
}
