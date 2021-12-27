﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRopeClimbUpState : PlayerRopeState
{
    Vector3 ropeDirectionUp;

    public PlayerRopeClimbUpState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }


    //  TODO: FIX THE CLIMB UP PER ANIMATION AND ROTATION
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        statemachineController.core.ropePlayerController.ropeRB.bodyType = RigidbodyType2D.Kinematic;
        statemachineController.core.playerRB.bodyType = RigidbodyType2D.Kinematic;
        statemachineController.core.playerRB.freezeRotation = true;

        //  TODO: This is for storing the rotation and position from rope start grab
        parentPlayerOldRot = statemachineController.ropeStartGrab.parentPlayerOldRot;
        childPlayerOldRot = statemachineController.ropeStartGrab.childPlayerOldRot;
        ropeOldRot = statemachineController.ropeStartGrab.ropeOldRot;
        ropeOldPos = statemachineController.ropeStartGrab.ropeOldPos;

        statemachineController.core.ropePlayerController.RemoveRopePlayerHingeJointConnector();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        if (!isExitingState)
        {
            if (GameManager.instance.gameplayController.ropeInput)
            {
                if (GameManager.instance.gameplayController.jumpInput)
                {
                    ExitRopeState();
                    statemachineChanger.ChangeState(statemachineController.ropeJumpState);
                }

                else if (GameManager.instance.gameplayController.movementNormalizeY == 0 ||
                    !statemachineController.core.ropePlayerController.CanClimbUp())
                {
                    statemachineController.transform.parent = null;
                    statemachineController.core.MoveObjectToOriginalScene();
                    statemachineChanger.ChangeState(statemachineController.ropeGrabSwing);
                }
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        ClimbUp();
    }

    private void ClimbUp()
    {
        statemachineController.core.ropePlayerController.RopePosition().GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        statemachineController.transform.parent = statemachineController.core.ropePlayerController.RopePosition();

        direction = Mathf.Sign(Vector3.Dot(-statemachineController.core.ropePlayerController.RopePosition().right, Vector3.up));
        statemachineController.transform.localRotation = Quaternion.AngleAxis(direction * 90f, Vector3.forward);

        statemachineController.core.playerRB.MovePosition(new Vector2(statemachineController.core.ropePlayerController.RopePosition().position.x - (0.15f * 
            statemachineController.core.CurrentDirection), statemachineController.transform.position.y + movementData.ropeClimbVelocity * Time.fixedDeltaTime));

        //statemachineController.transform.Translate(Vector2.up * new Vector2(statemachineController.core.ropePlayerController.RopePosition().GetComponent<Rigidbody2D>().position.x 
        //    , movementData.ropeClimbVelocity) * Time.fixedDeltaTime);
    }
}