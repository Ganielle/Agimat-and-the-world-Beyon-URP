using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTransition : PlayerGroundAttackState
{


    public AttackTransition(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, 
        string animBoolName, bool isBoolAnim) : base(movementController, stateMachine,
            movementData, animBoolName, isBoolAnim)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        statemachineController.core.attackController.canExit = true;

        if (statemachineController.core.attackController.onLastAttackIndex)
            statemachineController.core.attackController.onLastAttackIndex = false;
    }



    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (!statemachineController.core.attackController.onLastAttackIndex)
            {
                if (GameManager.instance.gameplayController.attackInput && 
                    !statemachineController.core.attackController.onLastAttackIndex)
                {
                    GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("canAttackTransition", false);
                    statemachineController.core.attackController.canExit = false;
                    GameManager.instance.gameplayController.UseAttackInput();
                    AttackInitiate();
                }

                //  This is for movement animation cancel
                else if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0)
                    statemachineController.core.attackController.canExit = true;
            }
        }
    }
}
