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

    /// <summary>
    /// Steps in patrol mode
    /// 1.) only move when its on patrol, the game will generate random numbers
    /// for min and max patrol time and move on two direction 
    /// 2.) the enemy will stop when on search mode and at the same time it will
    /// generate random number for min and max search time
    /// 3.) directions would be random on start but after that it will alternate
    /// 4.) create a detector for wall and near ledge for the enemy to stop when
    /// its on patrol mode
    /// </summary>

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (statemachineController.core.CurrentPatrolState == NormalMobCore.PatrolState.PATROL)
            {
                statemachineController.core.ChangeDirection();
                statemachineChanger.ChangeState(statemachineController.patrolState);
            }
            else if (statemachineController.core.CurrentPatrolState == NormalMobCore.PatrolState.BATTLE)
            {

            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        statemachineController.core.SetVelocityZero();
    }
}
