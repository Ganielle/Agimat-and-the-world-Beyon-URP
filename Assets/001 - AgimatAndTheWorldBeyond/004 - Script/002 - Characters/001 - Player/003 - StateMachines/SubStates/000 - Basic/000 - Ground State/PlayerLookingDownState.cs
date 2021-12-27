using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookingDownState : PlayerGroundState
{
    public PlayerLookingDownState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.LOOKINGDOWN;

        statemachineController.core.SetVelocityZero();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        statemachineController.core.SetVelocityZero();

        if (!isExitingState)
        {
            if (GameManager.instance.gameplayController.movementNormalizeY != -1)
                statemachineChanger.ChangeState(statemachineController.lookingDownDoneState);
        }
    }
}
