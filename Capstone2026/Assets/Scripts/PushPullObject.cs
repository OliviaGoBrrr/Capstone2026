 using System.Collections;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PushPullObject : Interactable
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
                
                transform.position = new Vector3 (transform.position.x, initialYPos, transform.position.z); //for objects on flat ground

                // for objects on changing ground
                /*if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit ground))
                {
                    transform.up = ground.normal;
                }*/
            }

            else
            {
                Held = false;
                return;
            }
        } 
    }

    public override void onInteract()
    {
        if(Held)
        {
            if(canBeSetDown) transform.position = setDownLocation;
            Held = false;
            // exit state
            PManager.PushPullState.ExitState(); //TESTING THIS 
            return;
        }

        Held = true;
        // enter state
        PManager.PushPullState.EnterState(); //TESTING THIS
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
