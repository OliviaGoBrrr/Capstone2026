using UnityEngine;

public class PlayerWalkSubState : PlayerGroundedSuperState
{
    public PlayerWalkSubState(PlayerCCMovement player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered Walk State");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        // if no more input, playerStateMachine.ChangeState(player.IdleState);

        // if run key pressed, playerStateMachine.ChangeState(player.RunState);
    }
}
