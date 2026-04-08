using UnityEngine;

public class PlayerGroundedSuperState : PlayerState
{
    public PlayerGroundedSuperState(PlayerCCMovement player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
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

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        // DIALOGUE STATE
        
        // JUMP STATE
        if (player.jumpAction.action.WasPressedThisFrame())
        {
            playerStateMachine.ChangeState(player.JumpState);
        }

        // FALLING STATE
        if (player.playerVelocity.y < 0 && !player.playerController.isGrounded)
        {
            playerStateMachine.ChangeState(player.FallState);
        }

        // GRAPPLE STATE
        
        if (player.grappleAction.action.WasPressedThisFrame())
        {
            playerStateMachine.ChangeState(player.GrappleState);
        }

        // PUSHPULL STATE
    }

}
