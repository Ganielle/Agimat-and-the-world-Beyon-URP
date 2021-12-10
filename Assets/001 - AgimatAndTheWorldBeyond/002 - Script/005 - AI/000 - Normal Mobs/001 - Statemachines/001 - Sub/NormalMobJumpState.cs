using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobJumpState : NormalMobAbilityState
{
    protected float enterTime;

    public NormalMobJumpState(NormalMobStatemachineController movementController, 
        NormalMobStatemachineChanger statemachine, NormalMobRawData rawData, string animBoolName) :
        base(movementController, statemachine, rawData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enterTime = Time.time + 0.25f;
        isDoneAbility = false;

        if (statemachineController.isGrounded)
            statemachineController.core.SetVelocityX(statemachineController.core.groundController.CalculateLaunchVelocity().x,
                statemachineController.core.groundController.CalculateLaunchVelocity().y);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= enterTime)
            isDoneAbility = true;
    }
}
