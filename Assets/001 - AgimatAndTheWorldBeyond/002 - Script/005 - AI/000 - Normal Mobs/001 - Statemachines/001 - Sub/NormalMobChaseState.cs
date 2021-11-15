using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobChaseState : NormalMobGroundState
{
    private Vector2 playerLastPos;

    public NormalMobChaseState(NormalMobStatemachineController movementController, 
        NormalMobStatemachineChanger statemachine, NormalMobRawData rawData, string animBoolName) :
        base(movementController, statemachine, rawData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Debug.Log(statemachineController.core.groundController.CheckIfPlayerIsInMyBack);

        if (statemachineController.core.fovDetection.targetTF != null)
            playerLastPos = statemachineController.core.fovDetection.targetTF.transform.position;

        if (!isExitingState)
        {
            if (statemachineController.core.fovDetection.isInFOV)
            {
                if (statemachineController.core.groundController.frontFootGroundAngle <= statemachineController.core.groundController.maxSlopeAngle ||
                    statemachineController.core.groundController.frontFootGroundAngle >= statemachineController.core.groundController.minimumSlopeAngle)
                {
                    Debug.Log("search state because slope");
                    statemachineChanger.ChangeState(statemachineController.searchState); // CHANGE TO JUMP STATE
                }
            }
            else
            {
                //  NOT IN FOV BUT NEAR OR INSIDE THE PRESENCE DETECTOR
                if (statemachineController.core.fovDetection.targetTF != null && statemachineController.core.groundController.CheckIfPlayerIsInMyBack)
                {
                    Debug.Log("change direction still chasing");
                    statemachineController.core.ChangeDirection();
                }

                else if (Vector2.Distance(statemachineController.transform.position, playerLastPos) <= rawData.checkDistanceToPlayer ||
                    statemachineController.core.groundController.frontFootGroundAngle <= statemachineController.core.groundController.maxSlopeAngle ||
                    statemachineController.core.groundController.frontFootGroundAngle >= statemachineController.core.groundController.minimumSlopeAngle)
                {
                    Debug.Log("search state because last pos or slope");
                    statemachineChanger.ChangeState(statemachineController.searchState);
                }
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        statemachineController.core.SetVelocityX(rawData.chaseSpeed * statemachineController.core.currentDirection,
            statemachineController.core.GetCurrentVelocity.y);

        statemachineController.core.groundController.SlopeMovement();
    }


}
