using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobAttackState : NormalMobStatemachine
{
    public NormalMobAttackState(NormalMobStatemachineController movementController, 
        NormalMobStatemachineChanger statemachine, NormalMobRawData rawData, string animBoolName) : 
        base(movementController, statemachine, rawData, animBoolName)
    {
    }

    //TODO: FIX THE AFTER ATTACK AND TRANSITIONS

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (!statemachineController.core.attackController.CanTransition &&
                statemachineController.core.attackController.AttackIndex == 0)
            {
                if (statemachineController.core.fovDetection.isInFOV)
                {
                    if (Vector2.Distance(statemachineController.transform.position, statemachineController.core.fovDetection.targetTF.transform.position) >
                    rawData.checkDistanceToPlayer)
                        statemachineChanger.ChangeState(statemachineController.chaseState);
                    else
                        statemachineChanger.ChangeState(statemachineController.searchState);
                }

                else if (!statemachineController.core.fovDetection.isInFOV)
                {
                    statemachineController.core.ChangeEnterTimePatrolState(rawData.minPatrolTime,
                        rawData.maxPatrolTime);
                    statemachineController.core.ChangeDirection();
                    statemachineChanger.ChangeState(statemachineController.patrolState);
                }
            }
        }
    }
}
