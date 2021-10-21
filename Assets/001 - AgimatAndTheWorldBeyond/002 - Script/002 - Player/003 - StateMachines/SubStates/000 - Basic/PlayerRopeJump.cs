﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRopeJump : PlayerNormalAbilityState
{
    public PlayerRopeJump(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine, 
        PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }



    public override void DoChecks()
    {
        base.DoChecks();

        if (statemachineController.isTouchingWall && !statemachineController.isTouchingLedge)
            statemachineController.ledgeClimbState.SetDetectedPosition(statemachineController.transform.position);
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.ROPEJUMP;

        statemachineController.core.SetVelocityWallJump(movementData.ropeJumpVelocity, movementData.ropeJumpAngle,
            statemachineController.core.GetFacingDirection);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //  Animation in air velocity setter
        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetFloat("yVelocity",
            statemachineController.core.GetCurrentVelocity.y);
        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetFloat("xVelocity",
            Mathf.Abs(statemachineController.core.GetCurrentVelocity.x));

        if (!isExitingState)
        {
            if (Time.time >= startTime + movementData.wallJumpTime)
                isAbilityDone = true;

            //  Monkey bar
            else if (GameManager.instance.gameplayController.grabMonkeyBarInput &&
                statemachineController.core.CheckIfTouchingMonkeyBar)
            {
                isAbilityDone = true;
                statemachineChanger.ChangeState(statemachineController.monkeyBarGrab);
            }

            //  Rope
            else if (GameManager.instance.gameplayController.ropeInput &&
                statemachineController.core.ropePlayerController.CheckIfTouchingRope &&
                Time.time >= startTime + 0.25f)
            {
                isAbilityDone = true;
                statemachineChanger.ChangeState(statemachineController.ropeStartGrab);
            }

            //  For Dash State
            else if (GameManager.instance.gameplayController.dashInput &&
                statemachineController.playerDashState.CheckIfCanDash())
            {
                isAbilityDone = true;
                statemachineChanger.ChangeState(statemachineController.playerDashState);
            }

            else if (statemachineController.isGrounded && Time.time >= startTime + movementData.delayToCheckForGround)
                isAbilityDone = true;

            //  For ledgeClimb state
            else if (statemachineController.isTouchingWall && !statemachineController.isTouchingLedge)
            {
                isAbilityDone = true;
                statemachineChanger.ChangeState(statemachineController.ledgeClimbState);
            }

            else if (statemachineController.isTouchingWall)
                isAbilityDone = true;
        }
    }
}
