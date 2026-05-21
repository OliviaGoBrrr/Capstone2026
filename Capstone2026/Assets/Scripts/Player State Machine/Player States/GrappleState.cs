using UnityEngine;

public class PlayerGrappleState : PlayerCanMoveSuperState
{
    public PlayerGrappleState(PlayerManager player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
    {
    }

    public override void EnterState()
    {
        player.isGrappling = true;
        PostGameDataLog.grappleAmountInt++;

        base.EnterState();
        //Debug.Log("Entered Grapple State");
    }

    public override void ExitState()
    {
        player.isGrappling = false;

        base.ExitState();
    }

    public override void FrameUpdate()
    {
        player.movement.GrappleToTarget();

        base.FrameUpdate();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        // check if player cancels grapple, JUMP STATE
        if (player.movement.jumpAction.action.WasPressedThisFrame())
        {
            player.movement.CancelGrapple();
            playerStateMachine.ChangeState(player.JumpState);
        }

        // check if grapple ended then check below statements

        if (player.movement.grappling)
        {
            return;
        }
        
        // check if on ground, IDLE STATE
        if (player.movement.playerController.isGrounded)
        {
            playerStateMachine.ChangeState(player.IdleSubState);
        }

        // check if in the air, FALLING STATE
        if (player.movement.playerVelocity.y < 0 && !player.movement.playerController.isGrounded)
        {
            playerStateMachine.ChangeState(player.FallState);
        }

        
    }
}
