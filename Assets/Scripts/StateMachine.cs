using UnityEngine;
using MEC;

public abstract class StateMachine<StateType> : MonoBehaviour where StateType : State
{
    protected StateType state;

    public void SetState(StateType state)
    {
        if (this.state != null)
        {
            Timing.RunCoroutine(this.state.OnStateExit());
        }
        this.state = state;
        Timing.RunCoroutine(this.state.OnStateEnter());
    }

    protected virtual void Update()
    {
        state.OnStateUpdate();
    }
}