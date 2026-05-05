using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Scripts")]
    public PlayerCCMovement movement;
    public PlayerSolarDetector solarPanel;
    //public GrappleInteract grapple;
    // public PlayerDialogueScript dialogue;
    // public PlayerGrappleScript grapple;
    // public PlayerPushPullScript pushPull;


    [Header("Player Stats")]
    public float batteryLightROC = 30; // per second
    public float batteryShadeROC = 15; // per second
    public float batteryPercent = 100;
    public TMP_Text batteryText;
    public Vector3 lastCheckpoint;

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
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.CurrentState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.FixedUpdate();
    }
}
