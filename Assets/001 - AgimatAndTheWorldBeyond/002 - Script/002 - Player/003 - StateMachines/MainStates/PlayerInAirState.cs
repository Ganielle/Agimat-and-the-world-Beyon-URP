using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerStatemachine
{
    private bool reachMaxJumpHeight;
    private bool isJumping;
    /// <summary>
    /// TODO : Coyote time when you finish the 
    /// skill sets like double jump
    /// </summary>

    public PlayerInAirState(PlayerStateMachinesController movementController,
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        statemachineController.core.groundPlayerController.PhysicsMaterialChanger(movementData.noFriction);

        if (statemachineController.isTouchingClimbWall && !statemachineController.isTouchingLedge)
            statemachineController.ledgeClimbState.SetDetectedPosition(statemachineController.transform.position);
    }

    public override void Enter()
    {
        base.Enter();
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

        AnimationChanger();
        CheckAnimationState();
        CheckIfReachMaxVelocity();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        HighLowJump();
        MovePlayerWhileInAir();
    }

    #region AIR STATE FUNCTIONS

    private void CheckAnimationState()
    {
        if (GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
            PlayerStats.AnimatorStateInfo.WALLJUMP || GameManager.instance.PlayerStats.GetSetAnimatorStateInfo != 
            PlayerStats.AnimatorStateInfo.LEDGECLIMB || GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
            PlayerStats.AnimatorStateInfo.LEDGEHOLD)
        {
            //if (!statemachineController.isGrounded && statemachineController.core.GetCurrentVelocity.y >= 0.01f)
            //    GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.JUMPING;
            //else if (!statemachineController.isGrounded && statemachineController.core.GetCurrentVelocity.y < 0.01f)
            //    GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.FALLING;
        }
    }

    private void AnimationChanger()
    {
        if (statemachineController.isGrounded && statemachineController.core.GetCurrentVelocity.y < 1f)
        {
            if (!reachMaxJumpHeight)
                statemachineChanger.ChangeState(statemachineController.lowLandState);
            else
            {
                statemachineChanger.ChangeState(statemachineController.highLandState);
                reachMaxJumpHeight = false;
            }
        }

        //  TODO : For Double Jump (Jump State)
        //else if (GameManager.instance.gameInputController.JumpInput &&
        //    GameManager.instance.JumpState.CanJump)
        //{

        //}

        //  Wall Climb && Ledge

        else if (statemachineController.ledgeClimbState.CheckIfCanLedgeClimb()
            && statemachineController.isSameHeightToPlatform && statemachineController.isTouchingClimbWall
            && !statemachineController.isTouchingLedge)
            statemachineChanger.ChangeState(statemachineController.ledgeClimbState);

        else if (statemachineController.isTouchingClimbWall && statemachineController.isSameHeightToPlatform &&
            GameManager.instance.gameplayController.GetSetMovementNormalizeX ==
                statemachineController.core.GetFacingDirection &&
                !GameManager.instance.gameplayController.jumpInput &&
                statemachineController.core.GetCurrentVelocity.y < 0.01f)
            statemachineChanger.ChangeState(statemachineController.wallSlideState);
       
        else if (statemachineController.isTouchingClimbWall && statemachineController.isSameHeightToPlatform &&
            GameManager.instance.gameplayController.grabWallInput)
            statemachineChanger.ChangeState(statemachineController.wallGrabState);

        //  Monkey Bar
        else if (statemachineController.isTouchingMonkeyBar &&
            GameManager.instance.gameplayController.grabMonkeyBarInput)
            statemachineChanger.ChangeState(statemachineController.monkeyBarGrab);

        //  Rope
        else if (statemachineController.isTouchingRope &&
            GameManager.instance.gameplayController.ropeInput)
            statemachineChanger.ChangeState(statemachineController.ropeStartGrab);

        //  Dash
        else if (GameManager.instance.gameplayController.dashInput &&
            statemachineController.playerDashState.CheckIfCanDash())
            statemachineChanger.ChangeState(statemachineController.playerDashState);
        else
            statemachineController.core.CheckIfShouldFlip(GameManager.instance.gameplayController.GetSetMovementNormalizeX);
    }

    private void HighLowJump()
    {
        if (isJumping)
        {
            if (GameManager.instance.gameplayController.jumpInputStop)
            {
                statemachineController.core.SetVelocityY(statemachineController
                    .core.GetCurrentVelocity.y *
                    movementData.variableJumpHeightMultiplier);
                isJumping = false;
            }
            else if (statemachineController.core.GetCurrentVelocity.y <= 0f)
                isJumping = false;
        }
    }

    private void MovePlayerWhileInAir()
    {
        if (statemachineController.isGrounded || statemachineController.isTouchingWall || 
            statemachineController.isFrontFootTouchGround || statemachineController.isTouchingGroundWhileInAir)
            return;

        statemachineController.core.SetVelocityX(movementData.movementSpeed *
            GameManager.instance.gameplayController.GetSetMovementNormalizeX,
                    statemachineController.core.GetCurrentVelocity.y);

        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetFloat("yVelocity",
            statemachineController.core.GetCurrentVelocity.y);
        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetFloat("xVelocity",
            Mathf.Abs(statemachineController.core.GetCurrentVelocity.x));
    }

    private void CheckIfReachMaxVelocity()
    {
        if (statemachineController.core.GetCurrentVelocity.y <=
            movementData.maxJumpHeight && !statemachineController.isGrounded)
            reachMaxJumpHeight = true;
    }

    public void SetIsJumping() => isJumping = true;

    #endregion
}
