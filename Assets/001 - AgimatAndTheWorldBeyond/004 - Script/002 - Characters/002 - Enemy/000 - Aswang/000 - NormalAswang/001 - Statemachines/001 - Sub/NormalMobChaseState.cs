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

        if (statemachineController.core.fovDetection.targetTF != null)
            playerLastPos = statemachineController.core.fovDetection.targetTF.transform.position;

        if (!isExitingState)
        {
            if (statemachineController.core.fovDetection.isInFOV)
            {
                if (Vector2.Distance(statemachineController.transform.position, statemachineController.core.fovDetection.targetTF.transform.position) <=
                    rawData.checkDistanceToPlayer)
                    InitiateAttack();

                if (statemachineController.core.groundController.frontFootGroundAngle <= statemachineController.core.groundController.maxSlopeAngle ||
                statemachineController.core.groundController.frontFootGroundAngle >= statemachineController.core.groundController.minimumSlopeAngle)
                {
                    statemachineController.core.ChangeEnterTimePatrolState(rawData.minSearchTime,
                        rawData.maxSearchTime);
                    {
                        statemachineController.core.groundController.targetPlayer = statemachineController.core.fovDetection.targetTF.transform;
                        statemachineController.core.groundController.heightToPlayer =
                            Mathf.Abs(statemachineController.core.fovDetection.targetTF.transform.position.y +
                            (Mathf.Abs(statemachineController.core.groundController.jumpGravity)));

                        statemachineChanger.ChangeState(statemachineController.jumpState);
                    }
                }
            }
            else
            {
                //  NOT IN FOV BUT NEAR OR INSIDE THE PRESENCE DETECTOR
                if (statemachineController.core.fovDetection.targetTF != null && statemachineController.core.groundController.CheckIfPlayerIsInMyBack)
                    statemachineController.core.ChangeDirection();

                else if (Vector2.Distance(statemachineController.transform.position, playerLastPos) <= rawData.checkDistanceToPlayer ||
                    statemachineController.core.groundController.frontFootGroundAngle <= statemachineController.core.groundController.maxSlopeAngle ||
                    statemachineController.core.groundController.frontFootGroundAngle >= statemachineController.core.groundController.minimumSlopeAngle)
                {
                    statemachineController.core.ChangeEnterTimePatrolState(rawData.minSearchTime,
                        rawData.maxSearchTime);
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
