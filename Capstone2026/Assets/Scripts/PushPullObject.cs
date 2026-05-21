 using System.Collections;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PushPullObject : MonoBehaviour, IInteractable
{
    public Transform PlayerTransform;
    public PlayerManager PManager;
    private bool Held = false;
    public bool canBeSetDown;
    public Vector3 setDownLocation;
    [HideInInspector] float initialYPos;

    void Start()
    {
        initialYPos = transform.position.y;
    }

    void Update()
    {
        if(Held)
        {
            if(PManager.isPushPulling == true)
            {
                transform.position = PlayerTransform.position + (transform.forward * 2);
                transform.rotation = PlayerTransform.rotation; //should be changed
                
                // For objects that need to stay on one y level
                // like currently if we needed we could include both? just a bool for "stays on same level"

                // transform.position = new Vector3 (transform.position.x, initialYPos, transform.position.z); 
                
                // come back to this if future objects will changed elevation
                float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
                transform.position = new Vector3(transform.position.x, terrainHeight /*+ (transform.localScale.y * 0.5f)*/, transform.position.z);
                
            }   

            else
            {
                Held = false;
                return;
            }
        } 
    }

    public void OnInteract()
    {
        if(Held)
        {
            if(canBeSetDown) transform.position = setDownLocation;
            Held = false;
            // exit state
            PManager.PushPullState.ExitState(); //TESTING THIS 
            return;
        }
        
        if(PManager.canPickUp == true)
        {
            Held = true;
            // enter state
            PManager.PushPullState.EnterState(); //TESTING THIS
        }
        
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "SetDownPoint")
        {
            canBeSetDown = true;
            setDownLocation = collision.transform.position;
            Debug.Log("can be set down: " + $"{setDownLocation}");
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        canBeSetDown = false;
    }
}
