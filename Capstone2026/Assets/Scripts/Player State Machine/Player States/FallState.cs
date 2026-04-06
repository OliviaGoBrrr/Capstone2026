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
        Debug.Log("Entered Fall State");
    }

    public override void ExitState()
    {
        player.isFalling = false;

        base.ExitState();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        // check if touch ground, playerStateMachine.ChangeState(player.IdleState);
    }
}
