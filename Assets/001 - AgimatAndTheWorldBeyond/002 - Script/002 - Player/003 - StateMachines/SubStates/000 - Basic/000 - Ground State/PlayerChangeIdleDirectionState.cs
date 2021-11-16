﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeIdleDirectionState : PlayerGroundState
{
    private bool canTransitionToOtherAnimation;
    private bool doneAnimation;
    public int direction;

    public PlayerChangeIdleDirectionState(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName, bool isBoolAnim) : 
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        doneAnimation = true;
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        canTransitionToOtherAnimation = true;
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.CHANGEIDLEDIRECTION;
    }

    public override void Exit()
    {
        base.Exit();
        canTransitionToOtherAnimation = false;
        doneAnimation = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {

            if (doneAnimation)
            {
                statemachineController.core.CheckIfShouldFlip(direction);

                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX == 0)
                    statemachineChanger.ChangeState(statemachineController.idleState);
            }

            else if (canTransitionToOtherAnimation)
            {
                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0 &&
                    !statemachineController.isTouchingWall)
                {
                    statemachineController.core.CheckIfShouldFlip(direction);
                    statemachineChanger.ChangeState(statemachineController.moveState);
                }

                else if (statemachineController.isGrounded && GameManager.instance.gameplayController.sprintTapCount == 2
                    && !statemachineController.isTouchingWall)
                    statemachineChanger.ChangeState(statemachineController.playerSprintState);

            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        statemachineController.core.SetVelocityZero();
    }

    public void SpriteDirectionAfterAnimation(int direction) => this.direction = direction;
}
