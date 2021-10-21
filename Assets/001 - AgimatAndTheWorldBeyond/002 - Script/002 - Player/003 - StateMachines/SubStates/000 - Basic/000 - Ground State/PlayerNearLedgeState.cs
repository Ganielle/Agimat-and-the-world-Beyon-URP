using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNearLedgeState : PlayerGroundState
{
    private Vector2 lastPos;

    public PlayerNearLedgeState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.NEARLEDGE;

        lastPos = statemachineController.transform.position;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        statemachineController.transform.position = lastPos;

        if (!isExitingState)
        {
            if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0)
                statemachineChanger.ChangeState(statemachineController.moveState);

            else if (statemachineController.isFrontFootTouchGround)
                statemachineChanger.ChangeState(statemachineController.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        statemachineController.core.SetVelocityZero();
    }
}
