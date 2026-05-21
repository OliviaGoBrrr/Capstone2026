using UnityEngine;

public class PlayerJumpState : PlayerCanMoveSuperState
{
    public PlayerJumpState(PlayerManager player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
    {
    }

    public override void EnterState()
    {
        player.isJumping = true;

        // jump logic
        player.movement.PlayerJump();

        //audio player jump

        base.EnterState();
        //Debug.Log("Entered Jump State");
    }

    public override void ExitState()
    {
        player.isJumping = false;

        base.ExitState();
    }

    public override void FrameUpdate()
    {

        if (player.movement.FindValidGrappleTarget() == true)
        {
            playerStateMachine.ChangeState(player.GrappleState);
        }

        base.FrameUpdate();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        // FALLING STATE
        if (player.movement.playerVelocity.y < 0 && !player.movement.playerController.isGrounded)
        {
            playerStateMachine.ChangeState(player.FallState);
        }

        // GROUNDED STATE
        if (player.movement.playerController.isGrounded)
        {
            playerStateMachine.ChangeState(player.IdleSubState);
        }
    }
}
