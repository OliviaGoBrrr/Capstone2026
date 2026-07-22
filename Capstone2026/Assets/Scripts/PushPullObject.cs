 using System.Collections;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class PushPullObject : MonoBehaviour, IInteractable
{
    public Transform PlayerTransform;
    public PlayerManager playerM;
    private bool Held = false;
    public bool canBeSetDown;
    public Vector3 setDownLocation;
    [HideInInspector] float initialYPos;

    private void Awake()
    {
        playerM = FindFirstObjectByType<PlayerManager>().GetComponent<PlayerManager>(); 
    }

    void Start()
    {
        initialYPos = transform.position.y;
    }

    void Update()
    {
        if(Held)
        {
            if(playerM.isPushPulling == true)
            {
                float forwardOffset = 0f;
                forwardOffset = GetComponent<Collider>().bounds.extents.z;

                float finalDistance = 1.5f + forwardOffset;
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, finalDistance);

                // find terrain height & set it
                float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
                transform.localPosition = new Vector3(transform.localPosition.x, terrainHeight, transform.localPosition.z);

                //transform.position = PlayerTransform.position + (transform.forward * 2);
                //transform.rotation = PlayerTransform.rotation;
                // transform.rotation = new Quaternion(transform.rotation.x, PlayerTransform.rotation.y, transform.rotation.z, 0); //thing im working on to make the rotation nicer
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
        PostGameDataLog.pushpullInteractInt++;
        
        if(Held)
        {
            if(canBeSetDown) transform.position = setDownLocation;
            Held = false;
            // exit state
            transform.SetParent(null);
            playerM.PushPullState.ExitState(); //TESTING THIS 
            return;
        }
        
        if(playerM.canPickUp == true)
        {
            Held = true;
            // enter state
            // transform.localRotation = Quaternion.identity;
            transform.SetParent(PlayerTransform);

            playerM.PushPullState.EnterState(); //TESTING THIS
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
