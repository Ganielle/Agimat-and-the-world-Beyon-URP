using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatemachine
{
    protected PlayerStateMachinesController statemachineController;
    protected PlayerStateMachineChanger statemachineChanger;
    protected PlayerRawData movementData;

    protected float startTime;
    protected float lastChangeWeaponTime;
    protected float lastLedgeClimb;

    protected bool isAnimationFinished;
    protected bool isExitingState;

    public string animBoolName;

    protected bool isBoolAnim;

    public PlayerStatemachine(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName , bool isBoolAnim)
    {
        this.statemachineController = movementController;
        this.statemachineChanger = stateMachine;
        this.movementData = movementData;
        this.animBoolName = animBoolName;
        this.isBoolAnim = isBoolAnim;
    }

    public virtual void Enter()
    {
        DoChecks();

        if (isBoolAnim)
            GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool(animBoolName, true);

        startTime = Time.time;
        isAnimationFinished = false;
        isExitingState = false;
    }

    public virtual void Exit()
    {
        if (isBoolAnim)
            GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool(animBoolName, false);

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

    public void AttackInitiate()
    {
        if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LUKAS)
        {
            if (GameManager.instance.PlayerInventory.GetLukasWeapons[GameManager.instance.PlayerInventory.GetSetWeaponLukasSlotIndex].CurrentWeaponType
                == PlayerWeaponRawData.WeaponType.AXE)
            {
                statemachineController.core.attackController.attackIndex++;
                statemachineController.core.attackController.SetComboIndexParameter("axeAttackCombo");
                statemachineController.core.attackController.SetLastFacingDirection(statemachineController.core.CurrentDirection);
                statemachineChanger.ChangeState(statemachineController.normalAttackState);
            }
        }
         if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LILY)
        {
            //  TODO LILY ATTACK COMBO
            if (GameManager.instance.PlayerInventory.GetLilyWeapons[GameManager.instance.PlayerInventory.GetSetWeaponLilySlotIndex].CurrentWeaponType ==
                PlayerWeaponRawData.WeaponType.WHIP)
            {
                statemachineController.core.attackController.attackIndex++;
                statemachineController.core.attackController.SetComboIndexParameter("whipAttackCombo");
                statemachineController.core.attackController.SetLastFacingDirection(statemachineController.core.CurrentDirection);
                statemachineChanger.ChangeState(statemachineController.normalAttackState);
            }
        }
    }
}
