using UnityEngine;

public class PlayerFallState : PlayerCanMoveSuperState
{
    public PlayerFallState(PlayerManager player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
    {
    }

    public override void EnterState()
    {
        player.isFalling = true;

        base.EnterState();
        //Debug.Log("Entered Fall State");
    }

    public override void ExitState()
    {
        player.isFalling = false;

        //audio player lands

        base.ExitState();
    }

    public override void FrameUpdate()
    {

        if (player.movement.FindValidGrappleTarget() == true)
        {
            playerStateMachine.ChangeState(player.GrappleState);
        }

        if (player.movement.coyoteTimer > 0)
        {
            player.movement.PlayerJump();
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

        // GROUNDED STATE / IDLE STATE
        if (player.movement.playerController.isGrounded)
        {
            playerStateMachine.ChangeState(player.IdleSubState);
        }
    }
}
