 using System.Collections;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PushPullObject : Interactable
{
    public GameObject Player;
    private bool Held = false;
    public bool canBeSetDown;
    public Vector3 setDownLocation;

    void Update()
    {
        if(Held)
        {
            transform.position = Player.transform.position + (transform.forward * 2);
        } 
    }

    public override void onInteract()
    {
        if(Held)
        {
            if(canBeSetDown) transform.position = setDownLocation;

            Held = false;
            return;
        }

        Held = true;
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
