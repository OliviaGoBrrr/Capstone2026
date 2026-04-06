using UnityEngine;

public class PlayerGroundedSuperState : PlayerState
{
    public PlayerGroundedSuperState(PlayerCCMovement player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
    {
    }

    protected Vector2 moveInput;
    protected bool isGrounded;

    public override void EnterState()
    {
        player.isGrounded = true;
        player.canMove = true;

        base.EnterState();
        Debug.Log("Entered Grounded State");
    }

    public override void ExitState()
    {
        player.isGrounded = false;

        base.ExitState();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        // if interacted with NPC, playerStateMachine.ChangeState(player.DialogueState);

        // if jump key pressed, playerStateMachine.ChangeState(player.JumpState);

        // if falling, playerStateMachine.ChangeState(player.FallState);

        // if grapple key pressed, playerStateMachine.ChangeState(player.GrappleState);

        // if interacted with pushpull object, playerStateMachine.ChangeState(player.PushPullState);
    }

}
