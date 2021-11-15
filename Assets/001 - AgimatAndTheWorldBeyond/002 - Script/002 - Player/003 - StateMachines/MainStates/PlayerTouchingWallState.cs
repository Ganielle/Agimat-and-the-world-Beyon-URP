using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerStatemachine
{

    public PlayerTouchingWallState(PlayerStateMachinesController movementController,
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName, bool isBoolAnim) : 
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void DoChecks()
    {
        base.DoChecks();
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

        if (statemachineController.isGrounded && !GameManager.instance.gameplayController.grabWallInput)
            statemachineChanger.ChangeState(statemachineController.idleState);

        else if (!statemachineController.isTouchingWall || (GameManager.instance.gameplayController.GetSetMovementNormalizeX !=
            statemachineController.core.CurrentDirection &&
            !GameManager.instance.gameplayController.grabWallInput))
            statemachineChanger.ChangeState(statemachineController.inAirState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
