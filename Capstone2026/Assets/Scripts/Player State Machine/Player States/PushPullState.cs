using Unity.VisualScripting;
using UnityEngine;

public class PlayerPushPullState : PlayerCanMoveSuperState
{
    public PlayerPushPullState(PlayerManager player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
    {
    }

    [SerializeField]
    private float inputInteractBuffer = 0.2f;
    private float bufferTimer;

    [SerializeField]
    private float holdingRotationRate = 3f;
    private float prevRotationRate;

    public override void EnterState()
    {
        prevRotationRate = player.movement.rotationSpeed;
        player.movement.rotationSpeed = holdingRotationRate;

        player.isPushPulling = true;
        player.canPickUp = false;

        bufferTimer = inputInteractBuffer;

        base.EnterState();
        //Debug.Log("Entered PushPull State");
    }

    public override void ExitState()
    {
        player.movement.rotationSpeed = prevRotationRate;
        player.isPushPulling = false;
        player.canPickUp = true;

        base.ExitState();
        //Debug.Log("Exited PushPull State");
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if(bufferTimer > 0) { bufferTimer -= Time.deltaTime; }
        else if (player.interact.action.IsPressed()) { ExitState(); }

        // audio if player is moving play sfx (maybe do one depending on each object and chuck it in that script? cuz there are diff objects like wooden and rocky and stuff)
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        // if pushpull done, IDLE
        if (player.isPushPulling == false)
        {
            //playerStateMachine.ChangeState(player.IdleSubState);
        }

        // if jumped out of pushpull. JUMP
        if (player.movement.jumpAction.action.WasPressedThisFrame())
        {
            playerStateMachine.ChangeState(player.JumpState);
        }
    }
}
