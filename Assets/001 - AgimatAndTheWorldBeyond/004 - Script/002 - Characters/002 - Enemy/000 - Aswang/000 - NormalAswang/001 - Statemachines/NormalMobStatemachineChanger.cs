using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobStatemachineChanger
{
    public NormalMobStatemachine currentState { get; private set; }

    public void Initialize(NormalMobStatemachine startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(NormalMobStatemachine newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
