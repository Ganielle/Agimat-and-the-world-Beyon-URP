using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerStatesController
{
    protected bool isAbilityDone;

    public PlayerAbilityState(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName, bool isBoolAnim) : 
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAbilityDone)
        {
            if (statemachineController.isGrounded && statemachineController.core.GetCurrentVelocity.y < 0.01f)
                statemachineChanger.ChangeState(statemachineController.idleState);
            else
                statemachineChanger.ChangeState(statemachineController.inAirState);
        }
    }
}
