using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobGroundState : NormalMobStatemachine
{
    public NormalMobGroundState(NormalMobStatemachineController movementController, 
        NormalMobStatemachineChanger statemachine, NormalMobRawData rawData, 
        string animBoolName) : base(movementController, statemachine, rawData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (!statemachineController.isGrounded)
                statemachineChanger.ChangeState(statemachineController.airState);
        }
    }
}
