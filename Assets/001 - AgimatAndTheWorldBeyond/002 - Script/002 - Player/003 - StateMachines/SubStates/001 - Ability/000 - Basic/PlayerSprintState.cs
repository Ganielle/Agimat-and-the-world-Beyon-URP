using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintState : PlayerNormalAbilityState
{
    private float facingDirection;

    public PlayerSprintState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
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

        SettingsSetter();
    }

    public override void Exit()
    {
        base.Exit();

        isAbilityDone = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        statemachineController.core.CheckIfShouldFlip(GameManager.instance.gameplayController.GetSetMovementNormalizeX);

        AnimationChanger();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        SprintingMove();
    }

    private void SettingsSetter()
    {
        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.SPRINT;

        isAbilityDone = false;
        facingDirection = statemachineController.core.CurrentDirection;
    }

    private void AnimationChanger()
    {
        if (!isExitingState)
        {
            if (statemachineController.core.GetCurrentVelocity.y < 0.01 &&
                !statemachineController.isGrounded)
                statemachineChanger.ChangeState(statemachineController.inAirState);

            else if (statemachineController.isTouchingWall)
                statemachineChanger.ChangeState(statemachineController.idleState);

            else if (statemachineController.isGrounded && !statemachineController.core.groundPlayerController.canWalkOnSlope &&
                        statemachineController.isFrontFootTouchSlope)
            {
                statemachineController.core.SetVelocityZero();
                statemachineChanger.ChangeState(statemachineController.steepSlopeSlide);
            }

            else if (statemachineController.isGrounded && GameManager.instance.gameplayController.jumpInput)
            {
                GameManager.instance.gameplayController.UseJumpInput();
                statemachineChanger.ChangeState(statemachineController.jumpState);
            }

            else if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0 &&
                GameManager.instance.gameplayController.GetSetMovementNormalizeX != facingDirection)
            {
                GameManager.instance.gameplayController.sprintTapCount = 0;
                statemachineChanger.ChangeState(statemachineController.moveState);
            }

            else if (GameManager.instance.gameplayController.sprintTapCount == 0
                && GameManager.instance.gameplayController.GetSetMovementNormalizeX == 0)
                statemachineChanger.ChangeState(statemachineController.idleState);

        }
    }

    private void SprintingMove()
    {
        if (statemachineController.core.CurrentDirection == GameManager.instance.gameplayController.GetSetMovementNormalizeX &&
            statemachineController.isFrontFootTouchDefaultGround)
            return;


        if (GameManager.instance.gameplayController.GetSetMovementNormalizeX == facingDirection)
        {
            statemachineController.core.SetVelocityX(movementData.sprintSpeed *
            GameManager.instance.gameplayController.GetSetMovementNormalizeX,
                statemachineController.core.GetCurrentVelocity.y);

            statemachineController.core.groundPlayerController.SlopeMovement();
        }
    }
}
