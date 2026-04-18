using UnityEngine;

public class PlayerDialogueState : PlayerState
{
    public PlayerDialogueState(PlayerManager player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
    {
    }

    public override void EnterState()
    {
        player.isDialogue = true;
        player.canMove = false;

        base.EnterState();
        //Debug.Log("Entered Dialogue State");
    }

    public override void ExitState()
    {
        player.isDialogue = false;

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

        // if dialogue done, playerStateMachine.ChangeState(player.IdleState);

    }
}
