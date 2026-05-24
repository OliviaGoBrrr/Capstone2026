using Unity.VisualScripting;
using UnityEngine;

public class PlayerPushPullState : PlayerCanMoveSuperState
{
    public PlayerPushPullState(PlayerManager player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
    {
    }

    public override void EnterState()
    {
        player.movement.moveSpeed = 5f;
        
        player.isPushPulling = true;
        player.canPickUp = false;

        base.EnterState();
        //Debug.Log("Entered PushPull State");
    }

    public override void ExitState()
    {
        player.movement.moveSpeed = 10f;

        player.isPushPulling = false;
        player.canPickUp = true;

        base.ExitState();
        //Debug.Log("Exited PushPull State");
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        // audio if player is moving play sfx (maybe do one depending on each object and chuck it in that script? cuz there are diff objects like wooden and rocky and stuff)
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        // if pushpull done, IDLE
        if (player.isPushPulling == false)
        {
            playerStateMachine.ChangeState(player.IdleSubState);
        }

        // if jumped out of pushpull. JUMP
        if (player.movement.jumpAction.action.WasPressedThisFrame())
        {
            playerStateMachine.ChangeState(player.JumpState);
        }
    }
}
