using UnityEngine;

public class PlayerCanMoveSuperState : PlayerState
{
    public PlayerCanMoveSuperState(PlayerManager player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
    {
    }

    public override void EnterState()
    {
        player.canMove = true;

        base.EnterState();
    }

    public override void ExitState()
    {
        player.canMove = false;

        base.ExitState();
    }

    public override void FrameUpdate()
    {
        player.movement.MovePlayer();
        player.movement.IsPlayerRunning();
        player.pauseMenu.WasPausePressed();

        base.FrameUpdate();
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
