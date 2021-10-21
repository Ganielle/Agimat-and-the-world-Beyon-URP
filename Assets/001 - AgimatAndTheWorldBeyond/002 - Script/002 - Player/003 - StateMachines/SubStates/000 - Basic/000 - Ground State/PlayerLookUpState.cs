using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookUpState : PlayerGroundState
{
    public PlayerLookUpState(PlayerStateMachinesController movementController,
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.LOOKINGUP;

        statemachineController.core.SetVelocityZero();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        statemachineController.core.SetVelocityZero();

        if (!isExitingState)
        {
            if (GameManager.instance.gameplayController.movementNormalizeY != 1)
                statemachineChanger.ChangeState(statemachineController.lookingUpDoneState);
        }
    }
}
