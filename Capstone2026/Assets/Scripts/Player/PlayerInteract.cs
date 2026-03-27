using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This entire script is temporary and just for the demo, cuz i know Liv has made specific grapple interact script. 
/// This could be used for things like NPC's though, or non grappling interacts .
/// </summary>
public class PlayerInteract : MonoBehaviour
{
    [Header("Player Interact")]
    public float maxDistance;
    public InputActionReference interactAction;
    public LayerMask InteractLayerMask;

    [Header("Debug Options")]
    public bool debug;

    private GameObject mainCamera;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        FindInteractable();
    }

    /// <summary>
    /// Function that sends a ray out from the player's camera to a distance. 
    /// If it hits an object, that object can be sent a message to complete event
    /// </summary>
    private void FindInteractable()
    {
        // Finds the max distance point in relation to the camera's rotation
        Vector3 endPoint = (mainCamera.transform.forward.normalized * maxDistance) + mainCamera.transform.position;

        // Recalculates the max distance from the camera to the end point
        float interactDistance = Vector3.Distance(mainCamera.transform.position, endPoint);

        // Sends a ray out from the camera to the end point
        if (Physics.Raycast(mainCamera.transform.position,
            mainCamera.transform.forward,
            out RaycastHit hit,
            interactDistance,
            InteractLayerMask)) // Ignores everything but objects in the "Interactable" layer
        {
            
            if (interactAction.action.WasPressedThisFrame()) // and the player innteracts with it
            {
                hit.transform.GetComponent<Interactable>().onInteract(); // perform the onInteract() function on the gameobject
                Debug.Log("yayy clicked");
            }

            // Debugging for when players are able to interact with something
            if (debug)
            {
                Debug.Log("Player can interact with " + hit.collider.gameObject.name);
                Debug.DrawLine(transform.position, hit.transform.position, Color.blue);
            }
        }

        // Debugging Player Interact Raycast
        if (debug)
        {
            Debug.DrawLine(transform.position, endPoint, Color.green);
            Debug.DrawLine(mainCamera.transform.position, endPoint, Color.yellow);
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
