using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState _CurrentState;

    public void InitializeStateMachine(PlayerState initialState)
    {
        _CurrentState = initialState;
        _CurrentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        _CurrentState.Exit();
        _CurrentState = newState;
        _CurrentState.Enter();
    }
}
