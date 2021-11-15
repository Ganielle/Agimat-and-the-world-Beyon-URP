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

        if (statemachineController.core.enterTimeStates == 0)
            statemachineController.core.ChangeEnterTimePatrolState(statemachineController.core.mobRawData.minPatrolTime,
                statemachineController.core.mobRawData.maxPatrolTime);
    }

    public override void Exit()
    {
        base.Exit();

        statemachineController.core.LastPatrolState = NormalMobCore.PatrolState.PATROL;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (statemachineController.core.groundController.frontFootGroundAngle <= statemachineController.core.groundController.maxSlopeAngle ||
            statemachineController.core.groundController.frontFootGroundAngle >= statemachineController.core.groundController.minimumSlopeAngle)
            statemachineController.core.ChangeDirection();

        if (!isExitingState)
        {
            if (!statemachineController.core.fovDetection.isInFOV)
            {
                if (Time.time >= statemachineController.core.enterTimeStates)
                {
                    statemachineController.core.ChangeEnterTimePatrolState(rawData.minSearchTime, 
                        rawData.maxSearchTime);
                    statemachineChanger.ChangeState(statemachineController.searchState);
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

        statemachineController.core.SetVelocityX(rawData.moveSpeed * statemachineController.core.currentDirection,
            statemachineController.core.GetCurrentVelocity.y);

        statemachineController.core.groundController.SlopeMovement();
    }
}
