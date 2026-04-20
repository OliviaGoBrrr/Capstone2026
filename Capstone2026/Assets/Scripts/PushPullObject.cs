 using System.Collections;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PushPullObject : Interactable
{
    public Transform PlayerTransform;
    public PlayerManager PManager;
    private bool Held = false;
    public bool canBeSetDown;
    public Vector3 setDownLocation;

    void Update()
    {
        if(Held)
        {
            transform.position = PlayerTransform.position + (transform.forward * 2);
        } 
    }

    public override void onInteract()
    {
        if(Held)
        {
            if(canBeSetDown) transform.position = setDownLocation;
            Held = false;
            PManager.isPushPulling = false;
            return;
        }

        Held = true;
        PManager.isPushPulling = true;
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
