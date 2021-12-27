using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobGroundAttackState : NormalMobAttackState
{
    int lastAttack;

    public NormalMobGroundAttackState(NormalMobStatemachineController movementController, 
        NormalMobStatemachineChanger statemachine, NormalMobRawData rawData, string animBoolName) : 
        base(movementController, statemachine, rawData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        if (Vector2.Distance(statemachineController.transform.position, statemachineController.core.fovDetection.targetTF.transform.position) <=
                    rawData.checkDistanceToPlayer && !statemachineController.core.attackController.OnLastAttackIndex)
            statemachineController.core.attackController.CanNextAttack = true;

        else if (Vector2.Distance(statemachineController.transform.position, statemachineController.core.fovDetection.targetTF.transform.position) >
                    rawData.checkDistanceToPlayer || statemachineController.core.attackController.OnLastAttackIndex)
            statemachineController.core.attackController.CanTransition = true;
    }

    public override void Enter()
    {
        base.Enter();

        statemachineController.core.SetVelocityZero();

        lastAttack = statemachineController.core.attackController.AttackIndex;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (statemachineController.core.attackController.CanTransition
            && !statemachineController.core.fovDetection.isInFOV ||
            statemachineController.core.fovDetection.characterTF == null)
            AttackFinishTransition();

        else if (statemachineController.core.fovDetection.isInFOV)
        {
            if (statemachineController.core.attackController.CanNextAttack &&
                !statemachineController.core.attackController.OnLastAttackIndex)
            {
                statemachineController.core.attackController.AttackIndex++;
                lastAttack = statemachineController.core.attackController.AttackIndex;
                statemachineController.core.enemyAnim.SetInteger("attackIndex", statemachineController.core.attackController.AttackIndex);
                statemachineController.core.attackController.CanNextAttack = false;
            }

            else if (statemachineController.core.attackController.CanTransition ||
                statemachineController.core.attackController.OnLastAttackIndex)
                AttackFinishTransition();
        }
    }

    private void AttackFinishTransition()
    {
        statemachineController.core.attackController.AttackIndex = 0;
        statemachineController.core.enemyAnim.SetInteger("attackIndex", statemachineController.core.attackController.AttackIndex);
        statemachineController.core.attackController.OnLastAttackIndex = false;
        statemachineChanger.ChangeState(statemachineController.attackTransition);
    }
}
