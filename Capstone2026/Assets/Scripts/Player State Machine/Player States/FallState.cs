using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(PlayerCCMovement player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
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

        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        // GROUNDED STATE
        if (player.playerController.isGrounded)
        {
            playerStateMachine.ChangeState(player.IdleSubState);
        }
    }
}
