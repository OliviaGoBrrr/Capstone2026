using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerCCMovement player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
    {
    }

    public override void EnterState()
    {
        player.isJumping = true;

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
        base.FrameUpdate();
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        // FALLING STATE
        if (player.playerVelocity.y < 0 && !player.playerController.isGrounded)
        {
            playerStateMachine.ChangeState(player.FallState);
        }
    }
}
