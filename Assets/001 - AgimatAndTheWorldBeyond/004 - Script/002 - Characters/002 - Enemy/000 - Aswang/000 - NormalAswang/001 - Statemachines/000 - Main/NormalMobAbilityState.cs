using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobAbilityState : NormalMobStatemachine
{
    protected bool isDoneAbility;

    public NormalMobAbilityState(NormalMobStatemachineController movementController, 
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

        isDoneAbility = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isDoneAbility)
        {
            if (statemachineController.isGrounded && statemachineController.core.GetCurrentVelocity.y < 0.01f)
            {
                if (statemachineController.core.fovDetection.isInFOV)
                    statemachineChanger.ChangeState(statemachineController.chaseState);
                else
                {
                    statemachineController.core.ChangeEnterTimePatrolState(rawData.minSearchTime,
                        rawData.maxSearchTime);
                    statemachineChanger.ChangeState(statemachineController.searchState);
                }
            }
            else
                statemachineChanger.ChangeState(statemachineController.airState);
        }
    }
}
