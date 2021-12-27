using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobAttackController : MonoBehaviour
{
    // =====================================================
    public bool CanInterrupt
    {
        get => canInterrupt;
        set => canInterrupt = value;
    }

    public bool CanNextAttack
    {
        get => canNextAttack;
        set => canNextAttack = value;
    }

    public bool CanTransition
    {
        get => canTransition;
        set => canTransition = value;
    }

    public bool OnLastAttackIndex
    {
        get => onLastAttackIndex;
        set => onLastAttackIndex = value;
    }

    public int AttackIndex
    {
        get => currentAttackIndex;
        set => currentAttackIndex = value;
    }

    public float RefreshAttackTimer
    {
        get => currentRefreshAttackTimer;
        set => currentRefreshAttackTimer = value;
    }

    // =====================================================

    [Header("DEBUGGER")]
    [ReadOnly] [SerializeField] bool canInterrupt;
    [ReadOnly] [SerializeField] bool canNextAttack;
    [ReadOnly] [SerializeField] bool canTransition;
    [ReadOnly] [SerializeField] bool onLastAttackIndex;
    [ReadOnly] [SerializeField] int currentAttackIndex;
    [ReadOnly] [SerializeField] float currentRefreshAttackTimer;

    private void OnEnable()
    {
        currentRefreshAttackTimer = 0f;
    }
}
