using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackCombo : PlayerGroundAttackState
{
    bool isOnLastFrame;

    //  TO FIX COMBO SYSTEM
    public NormalAttackCombo(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isOnLastFrame = true;
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        statemachineController.core.attackController.canNextAttack = true;
    }

    public override void Enter()
    {
        base.Enter();

        isOnLastFrame = false;

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

        //  NEXT ATTACK
        if (GameManager.instance.gameplayController.attackInput &&
            !statemachineController.core.attackController.onLastAttackIndex &&
            statemachineController.core.attackController.canNextAttack)
        {
            statemachineController.core.attackController.canNextAttack = false;
            statemachineController.core.attackController.canTransitionToNextAttack = true;
            statemachineController.core.attackController.attackIndex++;
            GameManager.instance.gameplayController.UseAttackInput();
        }

        // TRANSITION
        if (isOnLastFrame)
        {
            statemachineController.core.attackController.canChangeDirectionWhenAttacking = false;

            statemachineController.core.attackController.currentAttacking = false;

            statemachineController.core.attackController.canNextAttack = false;

            if (statemachineController.core.attackController.canTransitionToNextAttack &&
                !statemachineController.core.attackController.onLastAttackIndex)
            {
                statemachineController.core.attackController.canTransitionToNextAttack = false;

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.parameter,
                    statemachineController.core.attackController.attackIndex);

                isOnLastFrame = false;
            }

            else if (!statemachineController.core.attackController.canTransitionToNextAttack &&
                !statemachineController.core.attackController.onLastAttackIndex)
            {
                statemachineController.core.attackController.LastAttackEnterTime();

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.parameter,
                0);

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("canAttackTransition", true);

                isOnLastFrame = false;

                statemachineChanger.ChangeState(statemachineController.normalAttackTransition);
            }
            else if (statemachineController.core.attackController.onLastAttackIndex)
            {

                statemachineController.core.attackController.attackIndex = 0;

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.parameter,
                0);

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("canAttackTransition", true);

                isOnLastFrame = false;

                statemachineChanger.ChangeState(statemachineController.normalAttackTransition);
            }
        }
    }
}
