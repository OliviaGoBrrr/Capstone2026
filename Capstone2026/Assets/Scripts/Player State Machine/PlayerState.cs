using UnityEngine;

public class PlayerState
{
    protected PlayerManager player;
    protected PlayerStateMachine playerStateMachine;
    protected Animator animationController;
    protected string animationName;

    protected bool isExitingState;
    protected bool isAnimationFinished;
    protected float startTime;

    public PlayerState(PlayerManager player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
        this.animationController = animationController;
        this.animationName = animationName;
    }

    public virtual void EnterState()
    {
        isAnimationFinished = false;
        isExitingState = false;
        startTime = Time.time;
        //animationController.SetBool(animationName, true);
    }
    public virtual void ExitState()
    {
        isExitingState = true;
        //if (!isAnimationFinished) isAnimationFinished = true;
        //animationController.SetBool(animationName, false);
    }
    public virtual void FrameUpdate()
    {
        TransitionChecks();
    }
    public virtual void FixedUpdate()
    {
        // Change battery % regardless of current state
        if (player.solarPanel.CheckIfInLight() <= 2)
        {
            player.solarPanel.ChangeBatteryPercent(player.batteryPercent, 2, 1);
            player.solarPanel.isInLight = true;
        }
        else
        {
            player.solarPanel.ChangeBatteryPercent(player.batteryPercent, 2, -1);
            player.solarPanel.isInLight = false;
        }
    }
    public virtual void TransitionChecks()
    {
        // DEAD STATE
        if (player.batteryPercent < 0)
        {
            playerStateMachine.ChangeState(player.DeadState);
        }
    }
    public virtual void AnimationTrigger()
    {
        //isAnimationFinished = true;
    }
}
