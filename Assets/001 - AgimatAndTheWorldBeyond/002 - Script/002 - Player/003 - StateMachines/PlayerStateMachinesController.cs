using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachinesController : MonoBehaviour
{
    public PlayerCore core;

    //  StateMachines
    public PlayerStateMachineChanger statemachineChanger;
    public PlayerIdleState idleState;
    public PlayerLookUpState lookingUpState;
    public PlayerLookingDownState lookingDownState;
    public PlayerLookingDone lookingUpDoneState;
    public PlayerLookingDone lookingDownDoneState;
    public PlayerTauntIdleState tauntIdleState;
    public PlayerChangeIdleDirectionState changeIdleDirectionState;
    public PlayerMoveState moveState;
    public PlayerSteepSlopeSlideState steepSlopeSlide;
    public PlayerRunningBreak runningBreakState;
    public PlayerJumpState jumpState;
    public PlayerLowLandState lowLandState;
    public PlayerHighLandState highLandState;
    public PlayerInAirState inAirState;
    public PlayerNearLedgeState nearLedgeState;
    public PlayerWallSlideState wallSlideState;
    public PlayerWallGrabState wallGrabState;
    public PlayerWallClimbState wallClimbState;
    public PlayerWallJumpState wallJumpState;
    public PlayerLedgeClimbState ledgeClimbState;
    public PlayerMonkeyBarGrabState monkeyBarGrab;
    public PlayerMonkeyBarMove monkeyBarMove;
    public PlayerMonkeyBarJumpState monkeyBarJump;
    public PlayerRopeStartGrabState ropeStartGrab;
    public PlayerRopeGrabSwingState ropeGrabSwing;
    public PlayerRopeClimbUpState ropeClimbUp;
    public PlayerRopeClimbDownState ropeClimbDown;
    public PlayerRopeJump ropeJumpState;
    public PlayerSwitchState switchPlayerState;
    public PlayerDashState playerDashState;
    public PlayerSprintState playerSprintState;
    public PlayerDodgeState playerDodgeState;
    public PlayerWeaponSwitchState weaponSwitchState;
    public NormalAttackCombo normalAttackState;

    [Header("DEBUGGER")]
    [ReadOnly] public bool isGrounded;
    [ReadOnly] public bool isTouchingWall;
    [ReadOnly] public bool isTouchingClimbWall;
    [ReadOnly] public bool isTouchingLedge;
    [ReadOnly] public bool isSameHeightToPlatform;
    [ReadOnly] public bool isTouchingMonkeyBar;
    [ReadOnly] public bool isTouchingRope;
    [ReadOnly] public bool isFrontFootTouchGround;
    [ReadOnly] public bool isFrontFootTouchSlope;
    [ReadOnly] public bool isFrontFootTouchDefaultGround;
    [ReadOnly] public bool isTouchingGroundWhileInAir;
    [ReadOnly] public Vector2 checkSlopePos;

    private void Awake()
    {
        statemachineChanger = new PlayerStateMachineChanger();
        idleState = new PlayerIdleState(this, statemachineChanger, core.playerRawData, "idle", true);
        lookingUpState = new PlayerLookUpState(this, statemachineChanger, core.playerRawData, "lookingUp", true);
        lookingUpDoneState = new PlayerLookingDone(this, statemachineChanger, core.playerRawData, "lookingUpDone", true);
        lookingDownState = new PlayerLookingDownState(this, statemachineChanger, core.playerRawData, "lookingDown", true);
        lookingDownDoneState = new PlayerLookingDone(this, statemachineChanger, core.playerRawData, "lookingDownDone", true);
        tauntIdleState = new PlayerTauntIdleState(this, statemachineChanger, core.playerRawData, "tauntIdle", true);
        changeIdleDirectionState = new PlayerChangeIdleDirectionState(this, statemachineChanger,
            core.playerRawData, "idleChangeDirection", true);
        moveState = new PlayerMoveState(this, statemachineChanger, core.playerRawData, "move", true);
        steepSlopeSlide = new PlayerSteepSlopeSlideState(this, statemachineChanger, core.playerRawData, "slopeSlide", true);
        runningBreakState = new PlayerRunningBreak(this, statemachineChanger, core.playerRawData, "runningBreak", true);
        jumpState = new PlayerJumpState(this, statemachineChanger, core.playerRawData, "inAir", true);
        inAirState = new PlayerInAirState(this, statemachineChanger, core.playerRawData, "inAir", true);
        lowLandState = new PlayerLowLandState(this, statemachineChanger, core.playerRawData, "lowLand", true);
        highLandState = new PlayerHighLandState(this, statemachineChanger, core.playerRawData, "highLand", true);
        nearLedgeState = new PlayerNearLedgeState(this, statemachineChanger, core.playerRawData, "nearLedge", true);
        wallSlideState = new PlayerWallSlideState(this, statemachineChanger, core.playerRawData, "wallSlide", true);
        wallClimbState = new PlayerWallClimbState(this, statemachineChanger, core.playerRawData, "wallClimb", true);
        wallGrabState = new PlayerWallGrabState(this, statemachineChanger, core.playerRawData, "wallGrab", true);
        wallJumpState = new PlayerWallJumpState(this, statemachineChanger, core.playerRawData, "inAir", true);
        ledgeClimbState = new PlayerLedgeClimbState(this, statemachineChanger, core.playerRawData, "ledgeClimbState", true);
        monkeyBarGrab = new PlayerMonkeyBarGrabState(this, statemachineChanger, core.playerRawData, "monkeyBarIdle", true);
        monkeyBarMove = new PlayerMonkeyBarMove(this, statemachineChanger, core.playerRawData, "monkeyBarMove", true);
        monkeyBarJump = new PlayerMonkeyBarJumpState(this, statemachineChanger, core.playerRawData, "inAir", true);
        ropeStartGrab = new PlayerRopeStartGrabState(this, statemachineChanger, core.playerRawData, "ropeStartGrab", true);
        ropeGrabSwing = new PlayerRopeGrabSwingState(this, statemachineChanger, core.playerRawData, "ropeGrabSwing", true);
        ropeClimbUp = new PlayerRopeClimbUpState(this, statemachineChanger, core.playerRawData, "ropeClimb", true);
        ropeClimbDown = new PlayerRopeClimbDownState(this, statemachineChanger, core.playerRawData, "ropeSlide", true);
        ropeJumpState = new PlayerRopeJump(this, statemachineChanger, core.playerRawData, "inAir", true);
        switchPlayerState = new PlayerSwitchState(this, statemachineChanger, core.playerRawData, "currentSwitching", true);
        playerDashState = new PlayerDashState(this, statemachineChanger, core.playerRawData, "chargeDash", true);
        playerSprintState = new PlayerSprintState(this, statemachineChanger, core.playerRawData, "sprinting", true);
        playerDodgeState = new PlayerDodgeState(this, statemachineChanger, core.playerRawData, "dodge", true);
        weaponSwitchState = new PlayerWeaponSwitchState(this, statemachineChanger, core.playerRawData,
            GameManager.instance.PlayerStats.GetSetWeaponEquipBoolInPlayerAnim, true);
        normalAttackState = new NormalAttackCombo(this, statemachineChanger, core.playerRawData, "Attack", false);

        switchPlayerState.ResetSwitch();
        weaponSwitchState.ResetWeaponSwitch();
        playerDashState.ResetCanDash();
        playerDodgeState.ResetDodge();
        ledgeClimbState.ResetCanLedgeClimb();
    }

    private void Start()
    {
        statemachineChanger.Initialize(idleState);
    }

    private void Update()
    {
        core.CurrentVelocitySetter();
        core.groundPlayerController.SlopeChecker();

        statemachineChanger.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        EnvironmentChecker();

        core.groundPlayerController.CalculateSlopeForward();
        core.groundPlayerController.CalculateGroundAngle();

        statemachineChanger.CurrentState.PhysicsUpdate();

    }

    private void EnvironmentChecker()
    {
        isGrounded = core.groundPlayerController.CheckIfTouchGround;
        isFrontFootTouchGround = core.groundPlayerController.CheckIfFrontFootTouchGround;
        isFrontFootTouchSlope = core.groundPlayerController.CheckIfFrontTouchingSlope;
        isFrontFootTouchDefaultGround = core.groundPlayerController.CheckIfFrontFootTouchDefaultGround;
        isTouchingWall = core.groundPlayerController.CheckIfTouchClimbWall;
        isTouchingClimbWall = core.groundPlayerController.CheckIfTouchClimbWall;
        isTouchingLedge = core.groundPlayerController.CheckIfTouchingLedge;
        isSameHeightToPlatform = core.groundPlayerController.PlayerToPlatformHeightCheck;
        isTouchingMonkeyBar = core.CheckIfTouchingMonkeyBar;
        isTouchingRope = core.ropePlayerController.CheckIfTouchingRope;
        isTouchingGroundWhileInAir = core.groundPlayerController.CheckIfInAirTouchGround;
        checkSlopePos = transform.position - (Vector3)(new Vector2(0f, core.colliderSize.y / 2));
    }
}
