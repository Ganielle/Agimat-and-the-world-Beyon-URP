using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobPatrolState : NormalMobGroundState
{
    public NormalMobPatrolState(NormalMobStatemachineController movementController,
        NormalMobStatemachineChanger statemachine, NormalMobRawData rawData, 
        string animBoolName) : base(movementController, statemachine, rawData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (statemachineController.core.CurrentPatrolState == NormalMobCore.PatrolState.SEARCH)
            {
                statemachineChanger.ChangeState(statemachineController.searchState);
            }
            else if (statemachineController.core.CurrentPatrolState == NormalMobCore.PatrolState.BATTLE)
            {

            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        statemachineController.core.SetVelocityX(rawData.moveSpeed * statemachineController.core.currentDirection,
            statemachineController.core.GetCurrentVelocity.y);
    }
}
