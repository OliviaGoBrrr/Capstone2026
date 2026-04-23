using UnityEngine;

public class PlayerRunSubState : PlayerGroundedSuperState
{
    public PlayerRunSubState(PlayerManager player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        //Debug.Log("Entered Run State");
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
        if (!player.movement.runAction.action.IsPressed())
        {
            playerStateMachine.ChangeState(player.WalkSubState);
        }

        // IDLE STATE
        if (player.movement.moveAction.action.ReadValue<Vector2>() == Vector2.zero)
        {
            playerStateMachine.ChangeState(player.IdleSubState);
        }
    }
}
