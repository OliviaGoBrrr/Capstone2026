using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This entire script is temporary and just for the demo, cuz i know Liv has made specific grapple interact script. 
/// This could be used for things like NPC's though, or non grappling interacts .
/// </summary>
public class PlayerInteract : MonoBehaviour
{
    [Header("Player Interact")]
    public Camera playerCamera;
    public IInteractable interactable;
    public float maxDistance;
    public float interactAngle = 20f;
    public InputActionReference interactAction;
    public LayerMask InteractLayerMask;
    public LayerMask obstacleLayerMask;
    public Collider[] interactColliders;

    public GameObject interactUI;
    private GameObject interactUIObject;


    [Header("Debug Options")]
    public bool debug;


    private IInteractable lastTarget;

    private void Awake()
    {
        interactColliders = new Collider[10]; // Necessary for NonAlloc OverlapSphere
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FindIntertactable();
    }

    /// <summary>
    /// Function that sends a ray out from the player's camera to a distance. 
    /// If it hits an object, that object can be sent a message to complete an event
    /// </summary>


    private void FindIntertactable()
    {
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, maxDistance, interactColliders, InteractLayerMask);
        
        Vector3 closestInteraction = Vector3.zero;
        float closestDot = 0f;

        if (numColliders > 0)
        {
            // Checks all colliders (within the interact target layer) if they're within the interact angle...
            for (int i = 0; i < numColliders; i++)
            {
                Vector3 direction = (interactColliders[i].transform.position - playerCamera.transform.position).normalized;

                float dirDot = Vector3.Dot(playerCamera.transform.forward, direction);

                // ... and which one is closest to what the player is looking at.
                if (dirDot >= Mathf.Cos(Mathf.Deg2Rad * 20f)) // if its within the search angle
                {
                    if (dirDot > closestDot) // saves the closest interact target
                    {
                        closestDot = dirDot;
                        closestInteraction = direction;
                    }
                }
            }
        }

        if (closestInteraction != Vector3.zero)
        {
            RaycastHit hit;
            
            // will stop the raycast if it hits a wall or terrain
            if (Physics.Raycast(playerCamera.transform.position, closestInteraction, out hit, maxDistance, obstacleLayerMask))
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);

                lastTarget.DeactivateOutline();
                
                return;
            }

            // Sends a ray towards the closest grapple point
            else if (Physics.Raycast(playerCamera.transform.position, closestInteraction, out hit, maxDistance, InteractLayerMask))
            {
                Debug.DrawLine(playerCamera.transform.position, hit.point, Color.green);
                
                /*
                if(interactUI != null)
                {
                    if(interactUIObject != null && interactUIObject.transform.parent != hit.transform)
                    {
                        Destroy(interactUIObject);
                    }

                    Vector3 uiPos = hit.transform.position;
                    uiPos.y = uiPos.y + hit.collider.bounds.extents.y;

                    interactUIObject = Instantiate(interactUI, uiPos, Quaternion.identity);
                }
                */


                // It should find a target, but it allows the disabling of the grapple point
                IInteractable target = hit.transform.GetComponent<IInteractable>();
                if (lastTarget != null)
                {
                    if (lastTarget != target)
                    {
                        lastTarget.DeactivateOutline();
                    }
                }
                
                lastTarget = target;

                
                
                

                


                if (target != null) // If the grapple point isn't disabled
                {
                    // UI Appears
                    /*
                    if (target.grappleUICanvas != null)
                    {
                        if (currentGrappleUI != target.grappleUICanvas.gameObject)
                        {
                            DisableGrappleUI();
                            target.grappleUICanvas.gameObject.SetActive(true);
                            currentGrappleUI = target.grappleUICanvas.gameObject;
                        }
                    }
                    */

                    // if currently hit target is interactable ActivateOutline
                    target.ActivateOutline();


                    // Action is taken
                    if (interactAction.action.WasPressedThisFrame()) // If there was an input buffered
                    {
                        target.OnInteract();
                        // Clears interact colliders (doesn't save on memory, just worried it'll be funky)
                        Array.Clear(interactColliders,0, interactColliders.Length);
                    }
                }
            }
        }
        else
        {
            if (lastTarget != null) lastTarget.DeactivateOutline();
        }
    }

    private void OnEnable()
    {
        interactAction.action.Enable();
    }

    private void OnDisable()
    {
        interactAction.action.Disable();
    }
}
