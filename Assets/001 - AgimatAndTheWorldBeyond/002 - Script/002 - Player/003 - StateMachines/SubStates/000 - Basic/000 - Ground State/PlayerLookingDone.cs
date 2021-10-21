using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookingDone : PlayerGroundState
{
    bool doneLooking;

    public PlayerLookingDone(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName, bool isBoolAnim) : 
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        doneLooking = true;
    }

    public override void Exit()
    {
        base.Exit();

        doneLooking = false;
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        doneLooking = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (GameManager.instance.gameplayController.movementNormalizeY == -1)
                statemachineChanger.ChangeState(statemachineController.lookingDownState);


            else if (GameManager.instance.gameplayController.movementNormalizeY == 1)
                statemachineChanger.ChangeState(statemachineController.lookingUpState);

            if (!doneLooking)
            {
                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX == 0)
                    statemachineChanger.ChangeState(statemachineController.idleState);

                else if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0)
                    statemachineChanger.ChangeState(statemachineController.moveState);

                else if (GameManager.instance.gameplayController.attackInput)
                {
                    GameManager.instance.gameplayController.UseAttackInput();
                    AttackInitiate();
                }

                else if (GameManager.instance.gameplayController.dodgeInput
                    && statemachineController.playerDodgeState.CheckIfCanDodge())
                    statemachineChanger.ChangeState(statemachineController.playerDodgeState);

                else if (statemachineController.playerDashState.CheckIfCanDash() &&
                    statemachineController.core.groundPlayerController.canWalkOnSlope &&
                    GameManager.instance.gameplayController.dashInput &&
                    !GameManager.instance.gameplayController.switchPlayerLeftInput)
                    statemachineChanger.ChangeState(statemachineController.playerDashState);

            }
        }
    }
}
