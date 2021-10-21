using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLowLandState : PlayerGroundState
{
    public PlayerLowLandState(PlayerStateMachinesController movementController,
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.LOWLAND;

        PlaceSmoke();

        if (statemachineController.core.groundPlayerController.canWalkOnSlope)
            statemachineController.core.SetVelocityZero();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0)
                statemachineChanger.ChangeState(statemachineController.moveState);

            //Slope slide
            else if (statemachineController.isGrounded && !statemachineController.core.groundPlayerController.canWalkOnSlope &&
                statemachineController.isFrontFootTouchSlope)
                statemachineChanger.ChangeState(statemachineController.steepSlopeSlide);

            else if (GameManager.instance.gameplayController.attackInput &&
                       GameManager.instance.PlayerStats.GetSetAnimatorStateInfo != PlayerStats.AnimatorStateInfo.ATTACK)
            {
                GameManager.instance.gameplayController.UseAttackInput();
                AttackInitiate();
            }

            if (isAnimationFinished)
            {
                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX == 0)
                    statemachineChanger.ChangeState(statemachineController.idleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        statemachineController.core.SetVelocityZero();
    }

    private void PlaceSmoke()
    {
        GameManager.instance.effectManager.lowLandSmokePooler.GetFromPool();

        GameManager.instance.effectManager.lowLandSmokePooler.currentObjSelectedOnPool.transform.position =
            new Vector3(statemachineController.transform.position.x,
                statemachineController.transform.position.y - 0.35f, 0f);
    }
}
