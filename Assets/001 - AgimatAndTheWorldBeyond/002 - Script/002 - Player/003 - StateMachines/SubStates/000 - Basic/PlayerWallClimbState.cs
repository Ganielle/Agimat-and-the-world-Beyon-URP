﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbState : PlayerTouchingWallState
{
    public PlayerWallClimbState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.WALLCLIMB;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (statemachineController.isTouchingWall && !statemachineController.isTouchingLedge)
            statemachineController.ledgeClimbState.SetDetectedPosition(statemachineController.transform.position);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        statemachineController.core.SetVelocityY(movementData.wallClimbVelocity);

        AnimationChanger();
    }

    private void AnimationChanger()
    {
        if (statemachineController.isTouchingWall && !statemachineController.isTouchingLedge &&
            GameManager.instance.gameplayController.grabWallInput && !statemachineController.isGrounded)
            statemachineChanger.ChangeState(statemachineController.ledgeClimbState);

        else if (GameManager.instance.gameplayController.jumpInput)
        {
            statemachineChanger.ChangeState(statemachineController.wallJumpState);
        }

        if (GameManager.instance.gameplayController.movementNormalizeY != 1)
            statemachineChanger.ChangeState(statemachineController.wallGrabState);
    }
}
