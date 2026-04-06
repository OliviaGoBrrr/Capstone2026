using UnityEngine;

public class PlayerGrappleState : PlayerState
{
    public PlayerGrappleState(PlayerCCMovement player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
    {
    }

    public override void EnterState()
    {
        player.isGrappling = true;

        base.EnterState();
        Debug.Log("Entered Grapple State");
    }

    public override void ExitState()
    {
        player.isGrappling = false;

        base.ExitState();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        // check if ended then check below

        // check if ended on ground, playerStateMachine.ChangeState(player.IdleState);

        // check if ended in the air, playerStateMachine.ChangeState(player.FallState);
    }
}
