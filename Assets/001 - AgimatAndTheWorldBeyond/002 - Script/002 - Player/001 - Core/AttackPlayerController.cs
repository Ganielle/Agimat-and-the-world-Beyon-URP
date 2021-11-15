using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayerController : MonoBehaviour
{
    [Header("SETTINGS")]
    [SerializeField] private PlayerCore playerCore;
    [SerializeField] private float delayAttackTime;

    //  BATTLE ATTACK COMBO
    [ReadOnly] public string parameter;
    [ReadOnly] public float lastAttackEnterTime;
    [ReadOnly] public int attackIndex;
    [ReadOnly] public int lastFacingDirection;
    [ReadOnly] public bool currentAttacking;
    [ReadOnly] public bool canNextAttack;
    [ReadOnly] public bool onLastAttackIndex;
    [ReadOnly] public bool canChangeDirectionWhenAttacking;
    [ReadOnly] public bool canAnimationCancel;
    [ReadOnly] public bool canTransitionToNextAttack;
    [ReadOnly] public bool canExit;

    private void Update()
    {
        DelayNextAttackCounter();
    }

    private void DelayNextAttackCounter()
    {
        //  TODO: ADD DELAY ATTACK COUNTER

        if (!onLastAttackIndex && !currentAttacking && lastAttackEnterTime != 0f && Time.time >= lastAttackEnterTime)
        {
            lastAttackEnterTime = 0f;
            attackIndex = 0;
            canNextAttack = false;

            GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(parameter, attackIndex);

            parameter = "";
        }
    }

    public void LastAttackEnterTime() => lastAttackEnterTime = Time.time + delayAttackTime;

    public void SetComboIndexParameter(string parameter) => this.parameter = parameter;

    public void SetLastFacingDirection(int value) => lastFacingDirection = value;
}
