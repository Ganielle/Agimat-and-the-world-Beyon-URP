using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobGroundAttackTransition : NormalMobAttackState
{
    public NormalMobGroundAttackTransition(NormalMobStatemachineController movementController,
        NormalMobStatemachineChanger statemachine, NormalMobRawData rawData, string animBoolName) : 
        base(movementController, statemachine, rawData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        statemachineController.core.attackController.CanTransition = false;
        statemachineController.core.attackController.RefreshAttackTimer = Time.time + rawData.attackTimeRefresh;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //TODO: FOR GETTING ATTACK
    }
}
