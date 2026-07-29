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

        player.fallenInWater = false;

        player.movement.playerVelocity = new Vector3(0, 0, 0);

        // restarts scene when player runs out of power
        // replace this later with animations and what not
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        player.deathfade.deathFade();

        base.EnterState();
    }

    public override void ExitState()
    {
        // character controller overrides manual transform changes
        player.movement.playerController.enabled = false;
        player.transform.position = player.lastCheckpoint;
        player.movement.playerController.enabled = true;
        Debug.Log(player.lastCheckpoint);

        //player.isDead = false;
        player.batteryPercent = 100; // reset battery

        //audio sfx power on

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
        //base.TransitionChecks();

        if(player.isDead == false)
        {
            playerStateMachine.ChangeState(player.IdleSubState);
            
        }
    }
}
