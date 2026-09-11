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
    [Header("Assignments")]
    public Transform PlayerTransform;
    public PlayerManager playerM;

    [SerializeField] private GameObject moveableObject;
    [SerializeField] private int outlineLayer = 29;

    [Header("Variables")]
    public bool Held = false;
    private float forwardOffset;
    public const float playerToObjDist = 1.5f;
    public bool canBeSetDown;
    public bool inFinalPosition;
    public Vector3 setDownLocation;

    Terrain levelTerrain;
    [SerializeField] private Vector3 resetPos;
    [SerializeField] private Quaternion resetRot;

    private void Awake()
    {
        playerM = FindFirstObjectByType<PlayerManager>().GetComponent<PlayerManager>();
        PlayerTransform = playerM.GetComponent<PlayerCCMovement>().playerModel.transform;
    }

    void Start()
    {
        if(levelTerrain == null)
        {
            levelTerrain = Terrain.activeTerrain;
        }

        resetPos = transform.position;
        resetRot = transform.rotation;

        forwardOffset = GetComponent<Collider>().bounds.extents.magnitude / 2f;

        playerM.PlayerReset.AddListener(PlayerDeathReset);
    }

    void Update()
    {
        if(!Held) 
        {
            return;
        }
        
        if (levelTerrain != Terrain.activeTerrain)
        {
            levelTerrain = Terrain.activeTerrain;
        }

        if(playerM.StateMachine.CurrentState == playerM.PushPullState)
        {
            playerM.canPickUp = false;
        }

        if (playerM.isPushPulling == true)
        {
            float finalDistance = playerToObjDist + forwardOffset;
            transform.localPosition = new Vector3(0, transform.localPosition.y, finalDistance);

            // find terrain height & set it
            if (levelTerrain != null) // makes it so it can be used in testing scene without terrain
            {
                float terrainHeight = levelTerrain.SampleHeight(transform.position);
                transform.position = new Vector3(transform.position.x, terrainHeight, transform.position.z);
            }
        }

        else if (!playerM.isPushPulling)
        {
            StopHolding();
            return;
        }
        
    }

    public void OnInteract()
    {
        // Stops the player from picking up objects while jumping
        if (!playerM.movement.groundedPlayer) { return; }

        PostGameDataLog.pushpullInteractInt++;

        if(Held)
        {
            if(canBeSetDown) transform.position = setDownLocation;
            StopHolding();
            return;
        }
        
        else if(playerM.canPickUp == true)
        {
            Held = true;
            transform.SetParent(PlayerTransform);

            //playerM.PushPullState.EnterState();
            playerM.StateMachine.ChangeState(playerM.PushPullState);
        }
        
    }
    public void ActivateOutline()
    {
        if (!playerM.isPushPulling)
        {
            moveableObject.layer = outlineLayer;
        }
        else
        {
            moveableObject.layer = 0;
        }
    }

    public void DeactivateOutline()
    {
        moveableObject.layer = 0;
    }

    void StopHolding()
    {
        Held = false;
        transform.SetParent(null);

        if (playerM.StateMachine.CurrentState == playerM.PushPullState)
        {
            playerM.StateMachine.ChangeState(playerM.IdleSubState);
        }
    }

    // Actions
    void PlayerDeathReset()
    {
        // Stop the player from holding and reset barrel
        StopHolding();
        transform.position = resetPos;
        transform.rotation = resetRot;
    }


    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "SetDownPoint")
        {
            canBeSetDown = true;
            setDownLocation = collision.transform.position;
            Debug.Log("can be set down: " + $"{setDownLocation}");

            // if in final position, remove PlayerDeath listener
            // playerM.OnPlayerDeath.RemoveListener(HandleDeath);
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        canBeSetDown = false;
    }
}
