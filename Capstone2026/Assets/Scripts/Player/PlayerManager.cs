using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.Cinemachine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Scripts")]
    public PlayerCCMovement movement;
    public PlayerSolarDetector solarPanel;
    public PauseMenu pauseMenu;
    //public GrappleInteract grapple;
    // public PlayerDialogueScript dialogue;
    // public PlayerGrappleScript grapple;
    // public PlayerPushPullScript pushPull;


    [Header("Player Stats")]
    public bool isBatteryIncreasing = true; // change this to an enum
    public float batteryLightRateOfChangePerSecond;
    public float batteryShadeRateOfChangePerSecond;
    public float batteryPercent = 100;
    public int numberOfSunRaysForPower = 2;

    public TMP_Text batteryText;
    public Image batteryImage;

    public Image backBatteryImage;

    public Vector3 lastCheckpoint;
    public InputActionReference recenterCameraAction;
    public InputActionReference interact;
    [HideInInspector] public bool fallenInWater;

    [Header("References")] //remove this if i've done it wrong, this is just the solution im thinking of rn
    public DeathFade deathfade;
    public CinemachineCamera playerCam;
    public ParticleSystem[] sparks;
    private Tweener fovTween;
    [SerializeField] private InputActionReference toggleUI;

    [Header("Settings")]
    public Settings settings;

    [Header("Player SFX")]
    public AudioClip grapplePullSFX;
    public AudioClip batteryDrainSFX;
    [HideInInspector] public List<AudioSource> batteryDownClipsPlayed = new List<AudioSource>();


    [Header("Events")]
    public UnityEvent OnPlayerDeath = new();
    public UnityEvent PlayerReset = new();
    public UnityEvent<float> ChangeFOV = new();

    [Header("Player State Bools")]
    #region State Bools

    public bool canMove = true;
    public bool isCurrentlyGrounded = false;
    public bool isDialogue = false;
    public bool isJumping = false;
    public bool isPushPulling = false;
    public bool isGrappling = false;
    public bool isFalling = false;
    public bool isDead = false;
    public bool canPickUp = true;



    #endregion


    #region State Machine Vars

    public PlayerStateMachine StateMachine;

    public PlayerGroundedSuperState GroundedSuperState;

    public PlayerIdleSubState IdleSubState;
    public PlayerWalkSubState WalkSubState;
    public PlayerRunSubState RunSubState;

    public PlayerJumpState JumpState;
    public PlayerFallState FallState;

    public PlayerGrappleState GrappleState;

    public PlayerPushPullState PushPullState;

    public PlayerDialogueState DialogueState;

    public PlayerDeadState DeadState;

    #endregion

    // initialising state machine
    private void Awake()
    {
        // nulls for last 2 arguments is because we dont have animation players set up yet PlayerState(player, statemachine, animationName, animationController)
        StateMachine = new PlayerStateMachine();

        GroundedSuperState = new PlayerGroundedSuperState(this, StateMachine, null, null);

        IdleSubState = new PlayerIdleSubState(this, StateMachine, null, null);
        WalkSubState = new PlayerWalkSubState(this, StateMachine, null, null);
        RunSubState = new PlayerRunSubState(this, StateMachine, null, null);

        JumpState = new PlayerJumpState(this, StateMachine, null, null);
        FallState = new PlayerFallState(this, StateMachine, null, null);

        GrappleState = new PlayerGrappleState(this, StateMachine, null, null);

        PushPullState = new PlayerPushPullState(this, StateMachine, null, null);

        DialogueState = new PlayerDialogueState(this, StateMachine, null, null);

        DeadState = new PlayerDeadState(this, StateMachine, null, null);

        StateMachine.Initialise(IdleSubState);

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        batteryPercent = 100;
        lastCheckpoint = transform.position;

        OnPlayerDeath.AddListener(HandleDeath);
        PlayerReset.AddListener(HandleReset);
        ChangeFOV.AddListener(SprintFOVChange);


    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.CurrentState.FrameUpdate();

        if (recenterCameraAction.action.WasPressedThisFrame())
        {
            SnapRecenterCamera();
        }


        if(toggleUI != null)
        {
            if (toggleUI.action.WasPressedThisFrame()) // entirely for screenshots and marketing, feel free to remove from final
            {
                pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
            }
        }

    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.FixedUpdate();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11) // 11 is death layer
        {
            fallenInWater = true;
        }
    }

    //Tweening for FOV changes
    public void SprintFOVChange(float targetFOV)
    {
        if (playerCam.Lens.FieldOfView == targetFOV) return;

        fovTween?.Kill();

        //apparently cinemachine doesnt work with DOFieldOfView
        fovTween = DOTween.To( () => playerCam.Lens.FieldOfView, x => playerCam.Lens.FieldOfView = x, targetFOV, 0.2f).SetEase(Ease.OutQuad);
    }

    public void HandleTeleport(Vector3 pos)
    {
        Debug.Log($"Teleporting the player to position to {pos}");
        movement.playerController.enabled = false;
        transform.position = pos;
        SnapRecenterCamera();
        movement.playerController.enabled = true;
    }

    void HandleDeath()
    {
        isDead = true;
        canMove = false;
        movement.playerController.enabled = false;
        movement.CancelGrapple();
    }

    void HandleReset()
    {
        // Teleport player to checkpoint
        transform.position = lastCheckpoint;
        isDead = false;
        movement.playerController.enabled = true;
        canMove = true;
        SnapRecenterCamera();
    }

    // Flickers camera damping so the camera is centered to the player immediately. Used in death, but can be called with a key press/button
    public void SnapRecenterCamera()
    {
        if (playerCam != null)
        {
            playerCam.CancelDamping(true);
            Transform camTarget = playerCam.LookAt;
            playerCam.GetComponent<CinemachineRotationComposer>().ForceCameraPosition(camTarget.position, playerCam.transform.rotation);
            playerCam.CancelDamping(false);
        }
    }
}
