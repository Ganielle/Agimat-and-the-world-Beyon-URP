using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobInAirState : NormalMobStatemachine
{
    public NormalMobInAirState(NormalMobStatemachineController movementController, 
        NormalMobStatemachineChanger statemachine, NormalMobRawData rawData, string animBoolName) :
        base(movementController, statemachine, rawData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        statemachineController.core.groundController.PhysicsMaterialChanger(rawData.noFriction);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (statemachineController.isGrounded)
            {
                if (statemachineController.core.LastPatrolState == NormalMobCore.PatrolState.PATROL)
                {
                    statemachineController.core.ChangeEnterTimePatrolState(statemachineController.core.mobRawData.minPatrolTime,
                        statemachineController.core.mobRawData.maxPatrolTime);
                    statemachineChanger.ChangeState(statemachineController.patrolState);
                }
                else if (statemachineController.core.lastPatrolState == NormalMobCore.PatrolState.SEARCH)
                {
                    statemachineController.core.ChangeEnterTimePatrolState(statemachineController.core.mobRawData.minSearchTime,
                        statemachineController.core.mobRawData.maxSearchTime);
                    statemachineChanger.ChangeState(statemachineController.searchState);
                }
                else if (statemachineController.core.LastPatrolState == NormalMobCore.PatrolState.NONE)
                {
                    statemachineChanger.ChangeState(statemachineController.searchState);
                }
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        statemachineController.core.SetVelocityY(statemachineController.core.enemyRB.velocity.y);
    }

    #region IN AIR METHODS

    #endregion
}
