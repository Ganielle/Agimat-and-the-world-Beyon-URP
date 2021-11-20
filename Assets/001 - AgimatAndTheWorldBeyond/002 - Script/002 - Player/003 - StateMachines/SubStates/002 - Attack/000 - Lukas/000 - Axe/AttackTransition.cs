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

        statemachineController.core.attackController.ExitAttack = true;

        if (statemachineController.core.attackController.OnLastAttackIndex)
            statemachineController.core.attackController.OnLastAttackIndex = false;
    }



    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (!statemachineController.core.attackController.OnLastAttackIndex)
            {
                if (GameManager.instance.gameplayController.attackInput && 
                    !statemachineController.core.attackController.OnLastAttackIndex)
                {
                    GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("canAttackTransition", false);
                    statemachineController.core.attackController.ExitAttack = false;
                    GameManager.instance.gameplayController.UseAttackInput();
                    AttackInitiate();
                }

                //  This is for movement animation cancel
                else if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0)
                    statemachineController.core.attackController.ExitAttack = true;
            }
        }
    }
}
