using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerRunSubState : PlayerGroundedSuperState
{
    public PlayerRunSubState(PlayerManager player, PlayerStateMachine playerStateMachine, string animationName, Animator animationController) : base(player, playerStateMachine, animationName, animationController)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        player.movement.moveSpeed = player.movement.runSpeed;
        
        if(!player.isEaseFOVRunning) 
        { 
            player.StartCoroutine(player.EaseFOV(player.settings.FOVslider.value , player.settings.FOVslider.value + 10, 0.25f)); 
        }

        if(player.isCurrentlyGrounded)
        {
            foreach (ParticleSystem spark in player.sparks)
            {
                spark.Play();
            }
        }

        Debug.Log("Entered Run State");
    }

    public override void ExitState()
    {
        foreach (ParticleSystem spark in player.sparks)
        {
            spark.Stop();
        }

        if(!player.isEaseFOVRunning) 
        {
            player.StartCoroutine(player.EaseFOV(player.playerCam.Lens.FieldOfView, player.settings.FOVslider.value, 0.25f)); 
        }

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

        // WALK STATE
        if (!player.movement.runAction.action.IsPressed())
        {
            playerStateMachine.ChangeState(player.WalkSubState);
        }

        // IDLE STATE
        if (player.movement.moveAction.action.ReadValue<Vector2>() == Vector2.zero)
        {
            playerStateMachine.ChangeState(player.IdleSubState);
        }
    }
}
