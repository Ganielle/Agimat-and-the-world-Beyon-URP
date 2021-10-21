using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackCombo : PlayerGroundAttackState
{
    //  TO FIX COMBO SYSTEM
    public NormalAttackCombo(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        statemachineController.core.attackController.canChangeDirectionWhenAttacking = false;

        if (statemachineController.core.attackController.canTransitionToNextAttack)
        {
            statemachineController.core.attackController.canTransitionToNextAttack = false;

            statemachineController.core.attackController.canNextAttack = false;

            GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.parameter,
                statemachineController.core.attackController.attackIndex);
        }
        else
        {
            statemachineController.core.attackController.canExit = true;

            statemachineController.core.attackController.currentAttacking = false;

            statemachineController.core.attackController.canNextAttack = false;

            if (statemachineController.core.attackController.onLastAttackIndex)
            {
                statemachineController.core.attackController.onLastAttackIndex = false;

                statemachineController.core.attackController.attackIndex = 0;

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.parameter,
                0);
            }
            else
            {
                statemachineController.core.attackController.LastAttackEnterTime();

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.parameter,
                0);
            }
        }
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        statemachineController.core.attackController.canNextAttack = true;
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.ATTACK;

        statemachineController.core.attackController.canTransitionToNextAttack = false;

        statemachineController.core.attackController.currentAttacking = true;

        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.parameter,
            statemachineController.core.attackController.attackIndex);

        statemachineController.core.SetVelocityZero();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        ToNextAttackIndex();
    }

    private void ToNextAttackIndex()
    {
        if (GameManager.instance.gameplayController.attackInput &&
            !statemachineController.core.attackController.onLastAttackIndex &&
            statemachineController.core.attackController.canNextAttack)
        {
            statemachineController.core.attackController.canNextAttack = false;
            statemachineController.core.attackController.canTransitionToNextAttack = true;
            statemachineController.core.attackController.attackIndex++;
            GameManager.instance.gameplayController.UseAttackInput();
        }
    }
}
