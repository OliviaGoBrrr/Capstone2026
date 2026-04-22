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
        player.movement.PlayerMovementLogic();

        TransitionChecks();
    }

    public virtual void FixedUpdate()
    {
        // Change battery % regardless of current state
        if (player.solarPanel.CheckIfInLight() <= 2)
        {
            player.batteryPercent = player.solarPanel.ChangeBatteryPercent(player.batteryPercent, player.betteryROC, 1);
            player.batteryText.text = Mathf.Round(player.batteryPercent).ToString();
            player.solarPanel.isInLight = true;
        }
        else
        {
            player.batteryPercent = player.solarPanel.ChangeBatteryPercent(player.batteryPercent, player.betteryROC, -1);
            player.batteryText.text = Mathf.Round(player.batteryPercent).ToString();
            player.solarPanel.isInLight = false;
        }
    }

    public virtual void TransitionChecks()
    {
        // DEAD STATE
        if (player.batteryPercent <= 0)
        {
            playerStateMachine.ChangeState(player.DeadState);
        }
    }

    public virtual void AnimationTrigger()
    {
        //isAnimationFinished = true;
    }
}
