using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobStatemachine
{
    protected NormalMobStatemachineController statemachineController;
    protected NormalMobStatemachineChanger statemachineChanger;
    protected NormalMobRawData rawData;

    protected bool isAnimationFinished;
    protected bool isExitingState;

    protected string animBoolName;

    public NormalMobStatemachine(NormalMobStatemachineController movementController,
        NormalMobStatemachineChanger statemachine, NormalMobRawData rawData, string animBoolName)
    {
        statemachineController = movementController;
        statemachineChanger = statemachine;
        this.rawData = rawData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        DoChecks();

        statemachineController.core.enemyAnim.SetBool(animBoolName, true);

        isAnimationFinished = false;
        isExitingState = false;
    }

    public virtual void Exit()
    {
        statemachineController.core.enemyAnim.SetBool(animBoolName, false);
        isExitingState = true;
    }

    public virtual void LogicUpdate()
    {
        DoChecks();
    }

    public virtual void PhysicsUpdate() { }
    public virtual void DoChecks() { }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;

    public void InitiateAttack()
    {
        if (Time.time >= statemachineController.core.attackController.RefreshAttackTimer)
        {
            statemachineController.core.attackController.AttackIndex = 1;
            statemachineController.core.enemyAnim.SetInteger("attackIndex", 1);
            statemachineChanger.ChangeState(statemachineController.attackState);
        }
    }
}
