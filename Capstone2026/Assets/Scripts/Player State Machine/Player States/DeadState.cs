using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(PlayerManager player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
    {
    }

    public override void EnterState()
    {
        player.isDead = true;
        player.canMove = false;

        // restarts scene when player runs out of power
        // replace this later with animations and what not
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        base.EnterState();
        //Debug.Log("Entered Dead State");
    }

    public override void ExitState()
    {
        player.isDead = false;

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
    }
}
