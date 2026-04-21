using UnityEngine;

public class PlayerGroundedSuperState : PlayerState
{
    public PlayerGroundedSuperState(PlayerManager player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
    {
    }

    protected Vector2 moveInput;
    protected bool isGrounded;

    public override void EnterState()
    {
        player.isCurrentlyGrounded = true;
        player.canMove = true;

        base.EnterState();
        //Debug.Log("Entered Grounded State");
    }

    public override void ExitState()
    {
        player.isCurrentlyGrounded = false;

        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void TransitionChecks()
    {
        base.TransitionChecks();

        // DIALOGUE STATE
        if (player.isDialogue == true)
        {
            playerStateMachine.ChangeState(player.DialogueState);
        }
        
        // JUMP STATE
        if (player.movement.jumpAction.action.WasPressedThisFrame())
        {
            playerStateMachine.ChangeState(player.JumpState);
        }

        // FALLING STATE
        if (player.movement.playerVelocity.y < 0 && !player.movement.playerController.isGrounded)
        {
            playerStateMachine.ChangeState(player.FallState);
        }

        // GRAPPLE STATE
        
        if (player.movement.grappleAction.action.WasPressedThisFrame())
        {
            playerStateMachine.ChangeState(player.GrappleState);
        }

        // PUSHPULL STATE

        if (player.isPushPulling == true)
        {
            playerStateMachine.ChangeState(player.PushPullState);
        }
    }

}
