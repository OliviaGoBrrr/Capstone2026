using UnityEngine;
using DG.Tweening;

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
        if (player.solarPanel.CheckIfInLight() <= player.numberOfSunRaysForPower || player.isPoweredByLightBeam) // in light
        {
            player.batteryPercent = player.solarPanel.ChangeBatteryPercent(player.batteryPercent, player.batteryLightRateOfChangePerSecond);
            player.batteryText.text = Mathf.Round(player.batteryPercent).ToString();
            player.batteryText.color = Color.green;

            UpdateBatteryUI();
            player.solarPanel.isInLight = true;

            

            if (!player.isBatteryIncreasing)
            {
                //if (player.batteryDownClipsPlayed.Count == 0) return;

                foreach (AudioSource audioSource in player.batteryDownClipsPlayed)
                {
                    if (audioSource == null)
                    {
                        player.batteryDownClipsPlayed.Remove(audioSource);
                        continue;
                    }

                    audioSource.DOFade(0, 0.1f).OnComplete(() =>
                    {
                        Object.Destroy(audioSource.gameObject);
                        player.batteryDownClipsPlayed.Remove(audioSource);
                    });
                }

                player.batteryDownClipsPlayed.Clear();

                player.isBatteryIncreasing = true;

            }
            
            
        }
        else // in shade
        {
            player.batteryPercent = player.solarPanel.ChangeBatteryPercent(player.batteryPercent, player.batteryShadeRateOfChangePerSecond);
            player.batteryText.text = Mathf.Round(player.batteryPercent).ToString();
            player.batteryText.color = Color.red;

            UpdateBatteryUI();
            player.solarPanel.isInLight = false;

            if (player.isBatteryIncreasing) // play drain sfx one time once the battery starts to drain
            {
               // player.batteryDownClipsPlayed.Add(AudioManager.Instance.PlaySFXWithReference(player.batteryDrainSFX, player.transform, 0.75f));
                player.isBatteryIncreasing = false;
            }
            
        }
    }

    public void UpdateBatteryUI()
    {
        player.batteryImage.fillAmount = player.batteryPercent / 100;

        player.backBatteryImage.fillAmount = player.batteryPercent / 100;
    }

    public virtual void TransitionChecks()
    {
        // DEAD STATE
        if (player.batteryPercent <= 0)
        {
            player.OnPlayerDeath.Invoke();
            playerStateMachine.ChangeState(player.DeadState);
        }

        if (player.fallenInWater == true)
        {
            playerStateMachine.ChangeState(player.DeadState);
        }
    }

    public virtual void AnimationTrigger()
    {
        //isAnimationFinished = true;
    }
}
