using UnityEngine;

public class PlayerPushPullState : PlayerState
{
    public PlayerPushPullState(PlayerManager player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
    {
    }

    public override void EnterState()
    {
        player.isPushPulling = true;

        base.EnterState();
        //Debug.Log("Entered PushPull State");
    }

    public override void ExitState()
    {
        player.isPushPulling = false;

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

        // if pushpull done, IDLE

        // if jumped out of pushpull. JUMP
    }
}
