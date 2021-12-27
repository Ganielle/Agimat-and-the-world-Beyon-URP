using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    private float idleEnterTime;
    private bool canTauntIdle;

    public PlayerIdleState(PlayerStateMachinesController movementController,
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData,
        string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SettingsSetter();
    }

    public override void Exit()
    {
        base.Exit();

        canTauntIdle = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        TransitionTauntIdleTimer();

        AnimationChanger();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //  Slope slide
        if (statemachineController.isGrounded && !statemachineController.core.groundPlayerController.canWalkOnSlope &&
            statemachineController.isFrontFootTouchSlope)
            statemachineChanger.ChangeState(statemachineController.steepSlopeSlide);

        if (statemachineController.core.groundPlayerController.canWalkOnSlope)
            statemachineController.core.SetVelocityZero();
    }

    private void SettingsSetter()
    {
        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.IDLE;

        idleEnterTime = Time.time;
    }

    private void AnimationChanger()
    {
        if (!isExitingState)
        {

            if (GameManager.instance.gameplayController.jumpInput &&
                statemachineController.core.groundPlayerController.canWalkOnSlope)
            {
                statemachineChanger.ChangeState(statemachineController.jumpState);
                GameManager.instance.gameplayController.UseJumpInput();
            }

            else if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0f)
            {
                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX !=
                    statemachineController.core.CurrentDirection)
                {
                    statemachineController.changeIdleDirectionState.SpriteDirectionAfterAnimation(
                        GameManager.instance.gameplayController.GetSetMovementNormalizeX);
                    statemachineChanger.ChangeState(statemachineController.changeIdleDirectionState);
                }

                else if (!statemachineController.isTouchingWall)
                    statemachineChanger.ChangeState(statemachineController.moveState);
            }


            else if (GameManager.instance.gameplayController.attackInput)
            {
                GameManager.instance.gameplayController.UseAttackInput();
                AttackInitiate();
            }

            else if (statemachineController.isGrounded && GameManager.instance.gameplayController.sprintTapCount == 2 &&
                GameManager.instance.PlayerStats.GetSetAnimatorStateInfo != PlayerStats.AnimatorStateInfo.ATTACK)
                statemachineChanger.ChangeState(statemachineController.playerSprintState);

            else if (canTauntIdle)
                statemachineChanger.ChangeState(statemachineController.tauntIdleState);

            else if (!isAnimationFinished &&
                GameManager.instance.gameplayController.movementNormalizeY == 1f)
                statemachineChanger.ChangeState(statemachineController.lookingUpState);

            else if (!isAnimationFinished &&
                GameManager.instance.gameplayController.movementNormalizeY == -1)
                statemachineChanger.ChangeState(statemachineController.lookingDownState);

            else if (GameManager.instance.gameplayController.dodgeInput &&
                statemachineController.playerDodgeState.CheckIfCanDodge())
                statemachineChanger.ChangeState(statemachineController.playerDodgeState);

            else if (GameManager.instance.gameplayController.GetSetMovementNormalizeX == 0 &&
                GameManager.instance.gameplayController.switchPlayerLeftInput &&
                GameManager.instance.gameplayController.switchPlayerRightInput &&
                statemachineController.switchPlayerState.CheckIfCanSwitch())
                statemachineChanger.ChangeState(statemachineController.switchPlayerState);

            else if (!statemachineController.isFrontFootTouchGround && !statemachineController.isFrontFootTouchSlope)
                statemachineChanger.ChangeState(statemachineController.nearLedgeState);

            //  Dash
            else if (GameManager.instance.gameplayController.dashInput &&
                statemachineController.playerDashState.CheckIfCanDash())
                statemachineChanger.ChangeState(statemachineController.playerDashState);
        }
    }

    private void TransitionTauntIdleTimer()
    {
        if (Time.time >= idleEnterTime + movementData.idleToTauntIdleTime && !statemachineController.isTouchingWall)
            canTauntIdle = true;
    }
}
