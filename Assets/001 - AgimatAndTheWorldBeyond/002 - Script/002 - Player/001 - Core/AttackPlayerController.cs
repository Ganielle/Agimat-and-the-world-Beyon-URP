using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayerController : MonoBehaviour
{
    //  ==========================================================

    public string Parameter
    {
        get => currentParameter;
        set => currentParameter = value;
    }

    public bool CurrentAttacking
    {
        get => isCurrentAttacking;
        set => isCurrentAttacking = value;
    }

    public bool CanNextAttack
    {
        get => canNextAttack;
        set => canNextAttack = value;
    }

    public bool OnLastAttackIndex
    {
        get => onLastAttackIndex;
        set => onLastAttackIndex = value;
    }

    public bool CanChangeDirection
    {
        get => changeDirectionWhenAttacking;
        set => changeDirectionWhenAttacking = value;
    }

    public bool AnimationCancel
    {
        get => animationCancel;
        set => animationCancel = value;
    }

    public bool TransitionToNextAttack
    {
        get => transitionToNextAttack;
        set => transitionToNextAttack = value;
    }

    public bool ExitAttack
    {
        get => canExit;
        set => canExit = value;
    }

    public float LastAttackTime
    {
        get => attackEnterTimeLast;
        set => attackEnterTimeLast = value;
    }

    public int AttackIndex
    {
        get => attackIndex;
        set => attackIndex = value;
    }

    public int LastDirection
    {
        get => lastFacingDirection;
        set => lastFacingDirection = value;
    }

    //  ==========================================================

    [Header("SETTINGS")]
    [SerializeField] private PlayerCore playerCore;
    [SerializeField] private float delayAttackTime;

    //  BATTLE ATTACK COMBO
    [ReadOnly] public string currentParameter;
    [ReadOnly] public float attackEnterTimeLast;
    [ReadOnly] public int attackIndex;
    [ReadOnly] public int lastFacingDirection;
    [ReadOnly] public bool isCurrentAttacking;
    [ReadOnly] public bool canNextAttack;
    [ReadOnly] public bool onLastAttackIndex;
    [ReadOnly] public bool changeDirectionWhenAttacking;
    [ReadOnly] public bool animationCancel;
    [ReadOnly] public bool transitionToNextAttack;
    [ReadOnly] public bool canExit;

    private void Update()
    {
        DelayNextAttackCounter();
    }

    private void DelayNextAttackCounter()
    {
        //  TODO: ADD DELAY ATTACK COUNTER

        if (!OnLastAttackIndex && !CurrentAttacking && LastAttackTime != 0f && Time.time >= LastAttackTime)
        {
            LastAttackTime = 0f;
            AttackIndex = 0;
            CanNextAttack = false;

            GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(Parameter, AttackIndex);

            Parameter = "";
        }
    }

    public void LastAttackEnterTime() => LastAttackTime = Time.time + delayAttackTime;

    public void SetComboIndexParameter(string parameter) => this.Parameter = parameter;

    public void SetLastFacingDirection(int value) => LastDirection = value;
}
