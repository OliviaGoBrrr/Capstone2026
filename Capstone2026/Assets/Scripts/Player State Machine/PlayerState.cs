using UnityEngine;

public class PlayerState
{
    protected PlayerCCMovement player;
    protected PlayerStateMachine playerStateMachine;
    protected Animator animationController;
    protected string animationName;

    protected bool isExitingState;
    protected bool isAnimationFinished;
    protected float startTime;

    public PlayerState(PlayerCCMovement player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController)
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
    }
    public virtual void TransitionChecks()
    {
    }
    public virtual void AnimationTrigger()
    {
        //isAnimationFinished = true;
    }
}
