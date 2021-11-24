﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerStatemachine
{
    private Vector2 detectedPos;
    private Vector2 cornerPos;
    private Vector2 stopPosition;
    private Vector2 startPostion;
    private Vector2 oldPosition;

    private bool isHanging;
    private bool isClimbing;
    private bool canLedgeClimb;

    public PlayerLedgeClimbState(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        lastLedgeClimb = Time.time;
        isHanging = false;

        if (isClimbing)
        {
            statemachineController.transform.position = stopPosition;
            isClimbing = false;
        }

        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("ledgeClimb", false);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        isHanging = true;
    }

    public override void Enter()
    {
        base.Enter();
        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.LEDGEHOLD;

        //  For wall jump State
        oldPosition = statemachineController.transform.position;

        statemachineController.core.SetVelocityZero();
        statemachineController.transform.position = detectedPos;
        cornerPos = statemachineController.core.groundPlayerController.DetermineCornerPosition();

        startPostion.Set(cornerPos.x - (statemachineController.core.CurrentDirection * 
            movementData.startOffset.x), cornerPos.y - movementData.startOffset.y);
        stopPosition.Set(cornerPos.x + (statemachineController.core.CurrentDirection *
            movementData.stopOffset.x), cornerPos.y + movementData.stopOffset.y);

        statemachineController.transform.position = startPostion;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        AnimationChanger();
    }

    private void AnimationChanger()
    {
        if (isAnimationFinished)
        {
            statemachineChanger.ChangeState(statemachineController.idleState);
        }
        else
        {
            statemachineController.core.SetVelocityZero();
            statemachineController.transform.position = startPostion;

            //  To ledge Climb
            if ((GameManager.instance.gameplayController.GetSetMovementNormalizeX ==
                statemachineController.core.CurrentDirection ||
                GameManager.instance.gameplayController.movementNormalizeY == 1)
                && isHanging && !isClimbing)
            {
                isClimbing = true;
                GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.LEDGECLIMB;
                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("ledgeClimb", true);
            }

            //  Wall Jump
            else if (GameManager.instance.gameplayController.jumpInput && !isClimbing)
            {
                //  Setting back to old Position to fix the collider between
                // ground and player
                statemachineController.transform.position = 
                    new Vector2(oldPosition.x, statemachineController.transform.position.y);

                statemachineController.core.CheckIfShouldFlip(statemachineController.core.CurrentDirection * -1);
                statemachineChanger.ChangeState(statemachineController.wallJumpState);
            }

            //  To cancel ledge climb with x input
            else if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0 &&
                GameManager.instance.gameplayController.GetSetMovementNormalizeX !=
                statemachineController.core.CurrentDirection && isHanging &&
                !isClimbing)
            {
                statemachineController.core.CheckIfShouldFlip(GameManager.instance.gameplayController.GetSetMovementNormalizeX);
                statemachineController.core.SetVelocityX(10f,
                    statemachineController.core.GetCurrentVelocity.y);
                statemachineChanger.ChangeState(statemachineController.inAirState);
            }

            //  To Cancel ledge climb with y input
            else if (GameManager.instance.gameplayController.movementNormalizeY == -1 &&
                isHanging && !isClimbing)
                statemachineChanger.ChangeState(statemachineController.inAirState);
        }
    }

    public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;

    public bool CheckIfCanLedgeClimb()
    {
        return canLedgeClimb && Time.time >= lastLedgeClimb + 0.5f;
    }

    public bool ResetCanLedgeClimb() => canLedgeClimb = true;
}
