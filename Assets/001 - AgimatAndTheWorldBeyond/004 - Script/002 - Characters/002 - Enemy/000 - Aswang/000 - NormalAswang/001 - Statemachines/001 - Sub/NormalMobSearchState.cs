using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobSearchState : NormalMobGroundState
{
    public NormalMobSearchState(NormalMobStatemachineController movementController,
        NormalMobStatemachineChanger statemachine, NormalMobRawData rawData, 
        string animBoolName) : base(movementController, statemachine, rawData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (statemachineController.core.enterTimeStates == 0)
            statemachineController.core.ChangeEnterTimePatrolState(statemachineController.core.mobRawData.minSearchTime,
                statemachineController.core.mobRawData.maxSearchTime);
    }

    public override void Exit()
    {
        base.Exit();

        statemachineController.core.LastPatrolState = NormalMobCore.PatrolState.SEARCH;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (!statemachineController.core.fovDetection.isInFOV)
            {
                if (Time.time >= statemachineController.core.enterTimeStates)
                {
                    statemachineController.core.ChangeEnterTimePatrolState(rawData.minPatrolTime,
                        rawData.maxPatrolTime);
                    statemachineController.core.ChangeDirection();
                    statemachineChanger.ChangeState(statemachineController.patrolState);
                }
            }
            else
            {
                statemachineChanger.ChangeState(statemachineController.alertState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        statemachineController.core.SetVelocityZero();
    }
}
