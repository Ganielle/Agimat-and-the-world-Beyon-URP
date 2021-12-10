using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobAnimationEvent : MonoBehaviour
{
    [SerializeField] private NormalMobStatemachineController statemachineController;

    public void FinishAnimationTrigger() => statemachineController.normalMobStatemachineChanger.currentState.AnimationFinishTrigger();

    public void OnLastAttackIndex() => statemachineController.core.attackController.OnLastAttackIndex = true;
}
