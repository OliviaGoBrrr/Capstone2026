using UnityEngine;

public class PlayerIdleSubState : PlayerGroundedSuperState
{
    public PlayerIdleSubState(PlayerManager player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        //Debug.Log("Entered Idle State");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        // WALK STATE
        if (player.movement.playerInput != Vector3.zero)
        {
            if (player.movement.running) { playerStateMachine.ChangeState(player.RunSubState); }
            else { playerStateMachine.ChangeState(player.WalkSubState); }
        }
    }

}
