using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerStatesController
{
    public PlayerGroundState(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, 
        string animBoolName, bool isBoolAnim) : base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        AnimationChanger();

        statemachineController.core.weaponChangerController.DoneSwitchingWeapon();
        statemachineController.core.weaponChangerController.SwitchWeapon();
    }

    private void AnimationChanger()
    {
        if (!isExitingState)
        {
            if (!statemachineController.isGrounded)
                statemachineChanger.ChangeState(statemachineController.inAirState);

            // TODO: coyote time for JumpState
            else if (statemachineController.isTouchingWall && GameManager.instance.gameplayController.grabWallInput &&
                GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
                PlayerStats.AnimatorStateInfo.HIGHLAND && statemachineController.isTouchingLedge)
                statemachineChanger.ChangeState(statemachineController.wallGrabState);
        }
    }
}
