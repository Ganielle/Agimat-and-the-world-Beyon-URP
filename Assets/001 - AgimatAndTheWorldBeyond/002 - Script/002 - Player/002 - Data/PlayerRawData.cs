using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementData", menuName = "Agimat and the World Beyond/Player Data/Movement Data")]
public class PlayerRawData : ScriptableObject
{
    [Header("PHYSICS MATERIAL")]
    public PhysicsMaterial2D lessFriction;
    public PhysicsMaterial2D fullFriction;
    public PhysicsMaterial2D noFriction;
    public float colliderNormalOffsetY = 0.03998756f;
    public float colliderRopeOffsetY = -1.84f;

    [Header("TAUNT IDLE")]
    public float idleToTauntIdleTime = 50f;
    public float tauntIdleToIdleTime = 25f;

    [Header("MOVE STATE")]
    public float movementSpeed = 10f;
    public float sprintSpeed = 15f;
    public float maxVelocityXOnGround = 8f;

    [Header("JUMP STATE")]
    public float jumpStrength = 16f;
    public float movementSpeedOnAir = 5;
    public float movementSpeedOnAirAfterSprint = 18f;
    public float maxVelocityXOnAir = 10f;
    public float maxJumpHeight = -30f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float variableJumpHeightMultiplier = 0.5f;

    [Header("GROUND CHECKER")]
    public float groundCheckRadius = 0.15f;
    public float raycastGroundDistance = 5f;
    public float floatShadowHeightOffset = 1.5f;
    public float floorCheckOffsetHeight = 0.2f;
    public float floorCheckOffsetWidth = 2f;

    [Header("SLOPE MOVEMENT")]
    public float slopeCheckDistance = 1.9f;
    public float slopeForce = 2.5f;
    public float slopeFrontFootCheckDistance = 0f;

    [Header("WALL CHECKER")]
    public Vector2 wallCheckRadius = new Vector2(0.1f, 2f);
    public float wallClimbCheckRadius = 0.15f;
    public float wallSlideVelocity = 3f;
    public float wallClimbVelocity = 3f;

    [Header("WALL JUMP")]
    public float wallJumpVelocity = 20f;
    public float wallJumpTime = 0.9f;
    public float delayToUseDash = 0.25f;
    public float delayToCheckForGround = 0.15f;
    public float delayToCheckForLedge = 0.2f;
    public Vector2 wallJumpAngle = new Vector2(5, 10);

    [Header("LEDGE CLIMB")]
    public Vector2 startOffset = new Vector2(0f, 1.8f);
    public Vector2 stopOffset = new Vector2(1f, 1.5f);

    [Header("MONKEY BAR")]
    public float monkeyBarRarCheckRadius = 0.15f;
    public float monkeyBarVelocity = 2.5f;
    public Vector2 mbStartOffset = new Vector2(0f, 1.75f);
    public float monkeyBarJumpVelocity = 20f;
    public float delayToCheckForMBAfterJump = 0.25f;

    [Header("ROPE MOVEMENT")]
    public float ropeCheckRadius = 0.1f;
    public float ropeClimbVelocity = 1.5f;
    public float ropeClimDownVelocity = 10f;
    public float ropeSwingVelocity = 25f;
    public float anchorX = 0.2f;
    public float rightAngle = -30f;
    public float leftAngle = -30;
    public float ropeJumpVelocity = 25f;
    public float ropeJumpTime = 0.4f;
    public Vector2 ropeJumpAngle = new Vector2(1, 2);

    [Header("SWITCH CHARACTER")]
    public float switchTime = 0.85f;
    public float switchCooldown = 2f;
    public float weaponSwitchTime = 0.25f;

    [Header("DASH ABILITY")]
    public float dashCooldown = 1.2f;
    public float maxHoldTime = 0.15f;
    public float holdTimeScale = 1f;
    public float dashTime = 0.15f;
    public float dashVelocity = 60f;
    public float drag = 20f;
    public float dashEndYMultiplier = 0.2f;
    public float distanceBetweenAfterImages = 0.5f;

    [Header("DODGE ABILITY")]
    public float dodgeVelocity = 15f;
    public float dodgeCooldown = 0.5f;

    [Header("HEALTH")]
    public float healthRecoveryDelay = 2f;
    public float startingHealthRecoverPerSecond = 2.5f;

    [Header("MANA")]
    public float manaRecoveryDelay = 1f;
    public float startingManaRecoveryPerSecond = 1.5f;
    public float firstSkillUsePercentage = 33.33f;
    public float secondSkillUsePercentage = 66.66f;
    public float thirdSkillUsePercentage = 100f;

    //[Header("STAMINA")]
    //public float staminaRecoveryDelay = 0.5f;
    //public float staminaRecoverWhenActive = 15f;
    //public float staminaRecoverWhenNotActive = 2f;
    //public float sprintStamina = 7f;
    //public float ledgeStamina = 5f;
    //public float wallGrabHoldStamina = 10f;
    //public float wallClimbingStamina = 15f;
    //public float dodgePercentage = 0.25f;
    //public float dashPercentage = 0.35f;
    //public float wallJumpPercentage = 0.20f;
}
