using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundAttackState : PlayerStatemachine
{
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
        if (statemachineController.core.attackController.canChangeDirectionWhenAttacking && 
            GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0 &&
            GameManager.instance.gameplayController.GetSetMovementNormalizeX != statemachineController.core.CurrentDirection)
        {
            statemachineController.core.attackController.canChangeDirectionWhenAttacking = false;
            statemachineController.core.CheckIfShouldFlip(GameManager.instance.gameplayController.GetSetMovementNormalizeX);
        }


        //  IF ATTACK BUT NOT PRESS THE ATTACK AGAIN OR ON LAST ATTACK COMBO
        if ((!statemachineController.core.attackController.canTransitionToNextAttack || statemachineController.core.attackController.onLastAttackIndex) 
            && statemachineController.core.attackController.canExit)
        {

            statemachineController.core.attackController.canExit = false;

            //  RESET THE PARAMETER
            statemachineController.core.attackController.SetComboIndexParameter("");

            if (GameManager.instance.gameplayController.GetSetMovementNormalizeX == 0)
            {
                Debug.Log("to idle attack");
                statemachineChanger.ChangeState(statemachineController.idleState);
            }

            else if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0)
            {
                Debug.Log("to move attack");
                statemachineChanger.ChangeState(statemachineController.moveState);
            }

            else if (!isAnimationFinished &&
                GameManager.instance.gameplayController.movementNormalizeY == 1f)
            {
                Debug.Log("to looking up attack");
                statemachineChanger.ChangeState(statemachineController.lookingUpState);
            }

            else if (!isAnimationFinished &&
                GameManager.instance.gameplayController.movementNormalizeY == -1)
            {
                Debug.Log("to looking down attack");
                statemachineChanger.ChangeState(statemachineController.lookingDownState);
            }

            GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("canAttackTransition", false);
        }

        //  ANIMATION CANCEL
        else if (statemachineController.core.attackController.canAnimationCancel)
        {
            if (GameManager.instance.gameplayController.jumpInput &&
                statemachineController.core.groundPlayerController.canWalkOnSlope)
            {
                statemachineController.core.attackController.attackIndex = 0;

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.parameter,
                0);

                //  RESET THE PARAMETER
                statemachineController.core.attackController.SetComboIndexParameter("");

                statemachineController.core.attackController.currentAttacking = false;
                statemachineController.core.attackController.canAnimationCancel = false;
                statemachineController.core.attackController.canNextAttack = false;

                statemachineController.core.attackController.lastAttackEnterTime = 0f;

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("canAttackTransition", false);

                statemachineChanger.ChangeState(statemachineController.jumpState);
                GameManager.instance.gameplayController.UseJumpInput();
            }

            else if (GameManager.instance.gameplayController.dodgeInput
                && statemachineController.playerDodgeState.CheckIfCanDodge())
            {
                Debug.Log("to dodge attack");

                statemachineController.core.attackController.attackIndex = 0;

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.parameter,
                0);

                //  RESET THE PARAMETER
                statemachineController.core.attackController.SetComboIndexParameter("");

                statemachineController.core.attackController.currentAttacking = false;
                statemachineController.core.attackController.canAnimationCancel = false;
                statemachineController.core.attackController.canNextAttack = false;

                statemachineController.core.attackController.lastAttackEnterTime = 0f;

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("canAttackTransition", false);

                statemachineChanger.ChangeState(statemachineController.playerDodgeState);
            }

            else if (statemachineController.playerDashState.CheckIfCanDash() &&
            statemachineController.core.groundPlayerController.canWalkOnSlope &&
            GameManager.instance.gameplayController.dashInput &&
            !GameManager.instance.gameplayController.switchPlayerLeftInput)
            {
                statemachineController.core.attackController.attackIndex = 0;

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.parameter,
                0);

                //  RESET THE PARAMETER
                statemachineController.core.attackController.SetComboIndexParameter("");

                statemachineController.core.attackController.currentAttacking = false;
                statemachineController.core.attackController.canAnimationCancel = false;
                statemachineController.core.attackController.canNextAttack = false;

                statemachineController.core.attackController.lastAttackEnterTime = 0f;

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("canAttackTransition", false);

                statemachineChanger.ChangeState(statemachineController.playerDashState);
            }
        }
    }
}
