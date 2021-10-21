using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    public PlayerJumpState(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        PlaceSmoke();

        statemachineController.core.playerSFXController.PlaySFX(statemachineController.core.playerSFXController.footAS,
            statemachineController.core.playerSFXController.landJumpClip);

        if (statemachineController.isGrounded)
            statemachineController.core.SetVelocityY(movementData.jumpStrength);

        isAbilityDone = true;
        statemachineController.inAirState.SetIsJumping();
    }

    private void PlaceSmoke()
    {
        GameManager.instance.effectManager.jumpSmokePooler.GetFromPool();

        GameManager.instance.effectManager.jumpSmokePooler.currentObjSelectedOnPool.transform.position =
            new Vector3(statemachineController.transform.position.x,
                statemachineController.transform.position.y - 0.25f, 0f);
    }
}
