using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(PlayerManager player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
    {
    }

    public override void EnterState()
    {
        player.isDead = true;
        player.canMove = false;

        base.EnterState();
        //Debug.Log("Entered Dead State");
    }

    public override void ExitState()
    {
        player.isDead = false;

        base.ExitState();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();
    }
}
