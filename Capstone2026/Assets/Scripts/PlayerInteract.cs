using UnityEngine;

/// <summary>
/// This entire script is temporary and just for the demo, cuz i know Liv has made specific grapple interact script. 
/// This could be used for things like NPC's though, or non grappling interacts .
/// </summary>
public class PlayerInteract : MonoBehaviour
{
    private float maxDistance = 5f;
    private GameObject mainCamera;
    public KeyCode interactKey = KeyCode.E;
    public Lever lever;
    CapsuleCollider capsuleCollider;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 endPoint = (mainCamera.transform.forward.normalized * maxDistance) + mainCamera.transform.position;
        Debug.DrawLine(mainCamera.transform.position, endPoint, Color.green);
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, maxDistance) // If an interactable is in view
            && hit.transform.gameObject.tag == "Interactable")
        {
            if (Input.GetKeyDown(interactKey)) // and the player innteracts with it
            {
                hit.transform.GetComponent<Interactable>().onInteract(); // perform the onInteract() function on the gameobject
                Debug.Log("yayy clicked");
            }
        }
    }
}
