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

        statemachineController.core.attackController.CanNextAttack = true;
    }

    public override void Enter()
    {
        base.Enter();

        isOnLastFrame = false;

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.ATTACK;

        statemachineController.core.attackController.TransitionToNextAttack = false;

        statemachineController.core.attackController.CurrentAttacking = true;

        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.Parameter,
            statemachineController.core.attackController.AttackIndex);

        statemachineController.core.SetVelocityZero();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (statemachineController.core.attackController.AttackIndex == 0 && !statemachineController.core.attackController.ExitAttack && !cancelPressed)
            statemachineController.core.attackController.ExitAttack = true;

        //  NEXT ATTACK
        if (GameManager.instance.gameplayController.attackInput &&
            !statemachineController.core.attackController.OnLastAttackIndex &&
            statemachineController.core.attackController.CanNextAttack)
        {
            statemachineController.core.attackController.CanNextAttack = false;
            statemachineController.core.attackController.TransitionToNextAttack = true;
            statemachineController.core.attackController.AttackIndex++;
            GameManager.instance.gameplayController.UseAttackInput();
        }

        // TRANSITION
        if (isOnLastFrame)
        {
            statemachineController.core.attackController.CanChangeDirection = false;

            statemachineController.core.attackController.CurrentAttacking = false;

            statemachineController.core.attackController.CanNextAttack = false;

            if (statemachineController.core.attackController.TransitionToNextAttack &&
                !statemachineController.core.attackController.OnLastAttackIndex)
            {
                statemachineController.core.attackController.TransitionToNextAttack = false;

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.Parameter,
                    statemachineController.core.attackController.AttackIndex);

                isOnLastFrame = false;
            }

            else if (!statemachineController.core.attackController.TransitionToNextAttack &&
                !statemachineController.core.attackController.OnLastAttackIndex)
            {
                statemachineController.core.attackController.LastAttackEnterTime();

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.Parameter,
                0);

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("canAttackTransition", true);

                isOnLastFrame = false;

                statemachineChanger.ChangeState(statemachineController.normalAttackTransition);
            }
            else if (statemachineController.core.attackController.OnLastAttackIndex)
            {
                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.Parameter,
                0);

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("canAttackTransition", true);

                isOnLastFrame = false;

                statemachineChanger.ChangeState(statemachineController.normalAttackTransition);
            }
        }
    }
}
