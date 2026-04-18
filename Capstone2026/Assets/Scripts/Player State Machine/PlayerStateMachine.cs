using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentState;

    public void Initialise(PlayerState initialState)
    {
        CurrentState = initialState;
        CurrentState.EnterState();
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
        //Debug.Log("Changed state to " + CurrentState.ToString());
    }
}
