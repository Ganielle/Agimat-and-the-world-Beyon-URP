using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundAttackState : PlayerStatemachine
{
    protected bool cancelPressed;

    public PlayerGroundAttackState(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData,
        string animBoolName, bool isBoolAnim) : base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        AnimationChanger();
    }

    private void AnimationChanger()
    {
        //  CHANGE DIRECTION WHEN ATTACKING
        if (statemachineController.core.attackController.CanChangeDirection && 
            GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0 &&
            GameManager.instance.gameplayController.GetSetMovementNormalizeX != statemachineController.core.CurrentDirection)
        {
            statemachineController.core.attackController.CanChangeDirection = false;
            statemachineController.core.CheckIfShouldFlip(GameManager.instance.gameplayController.GetSetMovementNormalizeX);
        }


        //  IF ATTACK BUT NOT PRESS THE ATTACK AGAIN OR ON LAST ATTACK COMBO
        if (statemachineController.core.attackController.ExitAttack)
        {
            statemachineController.core.attackController.ExitAttack = false;

            if (GameManager.instance.gameplayController.GetSetMovementNormalizeX == 0)
                statemachineChanger.ChangeState(statemachineController.idleState);

            else if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0)
                statemachineChanger.ChangeState(statemachineController.moveState);

            else if (!isAnimationFinished &&
                GameManager.instance.gameplayController.movementNormalizeY == 1f)
                statemachineChanger.ChangeState(statemachineController.lookingUpState);

            else if (!isAnimationFinished &&
                GameManager.instance.gameplayController.movementNormalizeY == -1)
                statemachineChanger.ChangeState(statemachineController.lookingDownState);

            GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("canAttackTransition", false);
        }

        //  ANIMATION CANCEL
        else if (statemachineController.core.attackController.AnimationCancel)
        {
            if (GameManager.instance.gameplayController.jumpInput &&
                statemachineController.core.groundPlayerController.canWalkOnSlope)
            {
                cancelPressed = true;

                statemachineController.core.attackController.AttackIndex = 0;

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.Parameter,
                0);

                //  RESET THE PARAMETER
                statemachineController.core.attackController.SetComboIndexParameter("");

                statemachineController.core.attackController.LastAttackTime = 0f;

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("canAttackTransition", false);

                statemachineController.core.attackController.CurrentAttacking = false;
                statemachineController.core.attackController.CanChangeDirection = false;
                statemachineController.core.attackController.AnimationCancel = false;
                statemachineController.core.attackController.CanNextAttack = false;
                statemachineController.core.attackController.ExitAttack = false;

                GameManager.instance.gameplayController.UseJumpInput();
                statemachineChanger.ChangeState(statemachineController.jumpState);
            }

            else if (GameManager.instance.gameplayController.dodgeInput
                && statemachineController.playerDodgeState.CheckIfCanDodge())
            {
                cancelPressed = true;

                statemachineController.core.attackController.AttackIndex = 0;

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.Parameter,
                0);

                //  RESET THE PARAMETER
                statemachineController.core.attackController.SetComboIndexParameter("");

                statemachineController.core.attackController.LastAttackTime = 0f;

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("canAttackTransition", false);

                statemachineController.core.attackController.CurrentAttacking = false;
                statemachineController.core.attackController.CanChangeDirection = false;
                statemachineController.core.attackController.AnimationCancel = false;
                statemachineController.core.attackController.CanNextAttack = false;
                statemachineController.core.attackController.ExitAttack = false;

                statemachineChanger.ChangeState(statemachineController.playerDodgeState);
            }

            else if (statemachineController.playerDashState.CheckIfCanDash() &&
            statemachineController.core.groundPlayerController.canWalkOnSlope &&
            GameManager.instance.gameplayController.dashInput &&
            !GameManager.instance.gameplayController.switchPlayerLeftInput)
            {
                cancelPressed = true;

                statemachineController.core.attackController.AttackIndex = 0;

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.Parameter,
                0);

                //  RESET THE PARAMETER
                statemachineController.core.attackController.SetComboIndexParameter("");

                statemachineController.core.attackController.LastAttackTime = 0f;

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("canAttackTransition", false);

                statemachineController.core.attackController.CurrentAttacking = false;
                statemachineController.core.attackController.CanChangeDirection = false;
                statemachineController.core.attackController.AnimationCancel = false;
                statemachineController.core.attackController.CanNextAttack = false;
                statemachineController.core.attackController.ExitAttack = false;

                statemachineChanger.ChangeState(statemachineController.playerDashState);
            }
        }
    }
}
