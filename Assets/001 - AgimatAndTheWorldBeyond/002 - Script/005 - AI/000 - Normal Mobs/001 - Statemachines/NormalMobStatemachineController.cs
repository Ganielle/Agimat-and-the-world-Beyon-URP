using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobStatemachineController : MonoBehaviour
{
    public NormalMobCore core;

    // STATEMACHJINES
    #region STATEMACHINES

    public NormalMobStatemachineChanger normalMobStatemachineChanger;
    public NormalMobSearchState searchState;
    public NormalMobPatrolState patrolState;
    public NormalMobInAirState airState;
    public NormalMobAlertState alertState;
    public NormalMobChaseState chaseState;
    public NormalMobGroundAttackState attackState;
    public NormalMobGroundAttackTransition attackTransition;
    public NormalMobJumpState jumpState;

    #endregion

    [Header("DEBUGGER ENVIRONMENT")]
    [ReadOnly] public bool isGrounded;
    [ReadOnly] public bool isFrontFootTouchGround;

    private void Awake()
    {
        normalMobStatemachineChanger = new NormalMobStatemachineChanger();
        searchState = new NormalMobSearchState(this, normalMobStatemachineChanger, core.mobRawData, "searching");
        patrolState = new NormalMobPatrolState(this, normalMobStatemachineChanger, core.mobRawData, "patroling");
        airState = new NormalMobInAirState(this, normalMobStatemachineChanger, core.mobRawData, "inAir");
        alertState = new NormalMobAlertState(this, normalMobStatemachineChanger, core.mobRawData, "alerted");
        chaseState = new NormalMobChaseState(this, normalMobStatemachineChanger, core.mobRawData, "chase");
        attackState = new NormalMobGroundAttackState(this, normalMobStatemachineChanger, core.mobRawData, "");
        attackTransition = new NormalMobGroundAttackTransition(this, normalMobStatemachineChanger, core.mobRawData, "canAttackTransition");
        jumpState = new NormalMobJumpState(this, normalMobStatemachineChanger, core.mobRawData, "jumping");

    }

    private void Start()
    {
        normalMobStatemachineChanger.Initialize(searchState);
    }

    private void Update()
    {
        core.CurrentVelocitySetter();

        normalMobStatemachineChanger.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        EnvironmentChecker();

        core.groundController.CalculateSlopeForward();
        core.groundController.CalculateGroundAngle();
        core.groundController.SlopeChecker();

        normalMobStatemachineChanger.currentState.PhysicsUpdate();
    }

    private void EnvironmentChecker()
    {
        isGrounded = core.groundController.CheckIfTouchGround;
        isFrontFootTouchGround = core.groundController.CheckIfFrontFootTouchGround;

        core.fovDetection.PresenceDetector();
    }
}
