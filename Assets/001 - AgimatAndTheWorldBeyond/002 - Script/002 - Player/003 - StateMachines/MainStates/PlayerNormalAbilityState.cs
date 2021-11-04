using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalAbilityState : PlayerStatemachine
{
    protected bool isAbilityDone;

    public PlayerNormalAbilityState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName, bool isBoolAnim) : base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
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

        statemachineController.core.weaponChangerController.DoneSwitchingWeapon();
        statemachineController.core.weaponChangerController.SwitchWeapon();

        if (isAbilityDone)
        {
            if (statemachineController.isGrounded && statemachineController.core.GetCurrentVelocity.y < 0.01f)
            {
                statemachineChanger.ChangeState(statemachineController.idleState);
            }
            else
            {
                statemachineChanger.ChangeState(statemachineController.inAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //  Slope Calculation
        statemachineController.core.groundPlayerController.CalculateSlopeForward();
        statemachineController.core.groundPlayerController.CalculateGroundAngle();

        statemachineController.core.groundPlayerController.SlopeChecker();
    }
}
