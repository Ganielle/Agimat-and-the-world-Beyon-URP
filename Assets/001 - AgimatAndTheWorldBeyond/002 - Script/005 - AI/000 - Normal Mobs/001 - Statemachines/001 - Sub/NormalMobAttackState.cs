using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobAttackState : NormalMobGroundAttackState
{
    public NormalMobAttackState(NormalMobStatemachineController movementController, 
        NormalMobStatemachineChanger statemachine, NormalMobRawData rawData, string animBoolName) : 
        base(movementController, statemachine, rawData, animBoolName)
    {
    }
}
