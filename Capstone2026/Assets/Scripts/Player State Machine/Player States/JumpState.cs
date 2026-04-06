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
        Debug.Log("Entered Jump State");
    }

    public override void ExitState()
    {
        player.isJumping = false;

        base.ExitState();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        // check if start to fall, playerStateMachine.ChangeState(player.FallState);
    }
}
