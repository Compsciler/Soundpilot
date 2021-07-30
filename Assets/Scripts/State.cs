using System.Collections.Generic;
using MEC;

// Source: https://www.youtube.com/watch?v=G1bd75R10m4
public abstract class State  // Derive with a constructor with a system parameter (e.g. abstract class BattleState with BattleSystem field)
{
    public virtual IEnumerator<float> OnStateEnter()
    {
        yield return Timing.WaitForOneFrame;
    }

    public virtual IEnumerator<float> OnStateExit()
    {
        yield return Timing.WaitForOneFrame;
    }

    public virtual void OnStateUpdate()
    {

    }
}