using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobAlertState : NormalMobGroundState
{
    private float enterTime;

    public NormalMobAlertState(NormalMobStatemachineController movementController,
        NormalMobStatemachineChanger statemachine, NormalMobRawData rawData, string animBoolName) :
        base(movementController, statemachine, rawData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enterTime = Time.time + rawData.alertTimeToChase;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (Time.time >= enterTime)
            {
                if (statemachineController.core.fovDetection.isInFOV)
                    statemachineChanger.ChangeState(statemachineController.chaseState);
            }
            else
            {
                if (statemachineController.core.fovDetection.isInFOV)
                {
                    if (Vector2.Distance(statemachineController.transform.position, statemachineController.core.fovDetection.targetTF.transform.position) <=
                    rawData.checkDistanceToPlayer)
                        InitiateAttack();
                }

                else if (!statemachineController.core.fovDetection.isInFOV)
                {
                    switch (statemachineController.core.LastPatrolState)
                    {
                        case NormalMobCore.PatrolState.SEARCH: statemachineChanger.ChangeState(statemachineController.searchState); break;
                        case NormalMobCore.PatrolState.PATROL: statemachineChanger.ChangeState(statemachineController.patrolState); break;
                    }
                }
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        statemachineController.core.SetVelocityZero();
    }
}
