using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPlayerController : MonoBehaviour
{
    //  TODO: CHECK SLOPE CONTROLLER

    [SerializeField] private PlayerCore core;

    [Header("Ground")]
    public float maxSlopeAngle;
    public float minimumSlopeAngle;
    public LayerMask whatIsGround;
    public LayerMask groundDefault;
    public Transform playerPlatformHeightCheck;
    public Transform groundCheck;
    public Transform groundUnwalkableCheck;
    public Transform groundFrontFootCheck;
    public Transform groundFronFootSlopCheck;
    public Transform groundBackFootCheck;
    public Transform slopeCheck;

    [Header("Wall")]
    public Transform wallCheck;
    public Transform wallClimbCheck;
    public Transform ledgeCheck;

    [Header("ReadOnly")]
    [ReadOnly] public Vector2 forceOnSlope;
    [ReadOnly] public Vector3 slopeForward;
    [ReadOnly] public float groundMiddleAngle;
    [ReadOnly] public float groundFrontFootAngle;
    [ReadOnly] public bool canWalkOnSlope;
    [ReadOnly] [SerializeField] float moveDistance;
    [ReadOnly] [SerializeField] float horizontalOnSlope;
    [ReadOnly] [SerializeField] float verticalOnSlope;
    [ReadOnly] [SerializeField] float yDist;
    [ReadOnly] [SerializeField] float xDist;
    RaycastHit2D xHit, yHit;

    #region PHYSICS

    public void PhysicsMaterialChanger(PhysicsMaterial2D mat) => core.playerRB.sharedMaterial = mat;

    #endregion

    #region ENVIRONMENT

    public bool PlayerToPlatformHeightCheck
    {
        get => Physics2D.Raycast(playerPlatformHeightCheck.position, Vector2.right *
            core.CurrentDirection, core.playerRawData.wallClimbCheckRadius, whatIsGround);
    }

    public bool CheckIfTouchingLedge
    {
        get => Physics2D.Raycast(ledgeCheck.position, Vector2.right *
            core.CurrentDirection, core.playerRawData.wallClimbCheckRadius, whatIsGround);
    }

    public bool CheckIfTouchClimbWall
    {
        get => Physics2D.Raycast(wallClimbCheck.position, Vector2.right *
            core.CurrentDirection, core.playerRawData.wallClimbCheckRadius, whatIsGround);
    }

    public bool CheckIfTouchWall
    {
        get => Physics2D.OverlapBox(wallCheck.position, core.playerRawData.wallCheckRadius,
            0f, whatIsGround);
    }

    public bool CheckIfTouchGround
    {
        get => Physics2D.OverlapCircle(groundCheck.position, core.playerRawData.groundCheckRadius,
            whatIsGround);
    }

    public bool CheckIfFrontFootTouchGround
    {
        get => Physics2D.OverlapCircle(groundFrontFootCheck.position, core.playerRawData.groundCheckRadius,
            whatIsGround);
    }

    public bool CheckIfFrontFootTouchDefaultGround
    {
        get => Physics2D.OverlapCircle(groundFrontFootCheck.position, core.playerRawData.groundCheckRadius,
            groundDefault);
    }

    public bool CheckIfFrontTouchingSlope
    {
        get => Physics2D.Raycast(groundFronFootSlopCheck.position, Vector2.down, core.playerRawData.slopeFrontFootCheckDistance,
            whatIsGround);
    }

    public bool CheckIfBackFootTouchGround
    {
        get => Physics2D.OverlapCircle(groundBackFootCheck.position, core.playerRawData.groundCheckRadius,
            whatIsGround);
    }

    public bool CheckIfInAirTouchGround
    {
        get => Physics2D.OverlapCircle(groundUnwalkableCheck.position, core.playerRawData.groundCheckRadius,
               groundDefault);
    }

    public string GetGroundTag
    {
        get => Physics2D.Raycast(groundCheck.position, Vector2.down, core.playerRawData.groundCheckRadius,
                whatIsGround).transform.tag;
    }

    public Vector2 DetermineCornerPosition()
    {
        xHit = Physics2D.Raycast(wallClimbCheck.position, Vector2.right * core.CurrentDirection,
            core.playerRawData.wallClimbCheckRadius, whatIsGround);
        xDist = xHit.distance;
        core.GetWorkspace.Set((xDist + 0.015f) * core.CurrentDirection, 0f);
        yHit = Physics2D.Raycast((Vector2)ledgeCheck.position + (core.GetWorkspace),
            Vector2.down, ledgeCheck.position.y - wallClimbCheck.position.y, whatIsGround);
        yDist = yHit.distance;

        core.GetWorkspace.Set(wallClimbCheck.position.x + (xDist * core.CurrentDirection),
            ledgeCheck.position.y - yDist);

        return core.GetWorkspace;
    }

    public void CalculateSlopeForward()
    {
        if (!CheckIfTouchGround)
        {
            slopeForward = transform.forward;
            return;
        }

        slopeForward = Vector3.Cross(Physics2D.Raycast(transform.position, Vector2.down, core.playerRawData.slopeCheckDistance,
            whatIsGround).normal, transform.forward * core.CurrentDirection);
    }

    public void CalculateGroundAngle()
    {
        if (!CheckIfTouchGround)
        {
            groundMiddleAngle = 90f;
            groundFrontFootAngle = 90f;
            return;
        }

        groundMiddleAngle = Vector2.Angle(Physics2D.Raycast(transform.position, Vector2.down, core.playerRawData.slopeCheckDistance,
            whatIsGround).normal, -transform.up);

        groundFrontFootAngle = Vector2.Angle(Physics2D.Raycast(groundFronFootSlopCheck.position, Vector2.down, core.playerRawData.slopeFrontFootCheckDistance,
            whatIsGround).normal, -transform.up);


        //  Higher slope, no friction for sliding effect
        if (groundMiddleAngle <= maxSlopeAngle || groundMiddleAngle > minimumSlopeAngle)
            core.playerRB.sharedMaterial = core.playerRawData.noFriction;

        //  On flat surface or walkable, less friction for sticking on ground effect
        else if (groundMiddleAngle > maxSlopeAngle || groundMiddleAngle <= minimumSlopeAngle)
            core.playerRB.sharedMaterial = core.playerRawData.lessFriction;
    }

    public void SlopeChecker()
    {
        //  To prevent slope animation on in air while ground
        if (groundMiddleAngle == 90f && groundFrontFootAngle == 90f)
            canWalkOnSlope = false;
        //  On flat surface or walkable slope, on slope but can move
        else if (groundMiddleAngle < minimumSlopeAngle && groundMiddleAngle >= maxSlopeAngle)
            canWalkOnSlope = true;
        //  Higher slope, on slope but cannot move
        else if (groundMiddleAngle < maxSlopeAngle)
            canWalkOnSlope = false;
        else 
            canWalkOnSlope = false;
    }

    public void SlopeMovement()
    {
        if (CheckIfTouchGround && canWalkOnSlope &&
            CheckIfFrontTouchingSlope)
        {
            if (groundMiddleAngle <= minimumSlopeAngle)
            {
                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0f)
                    core.playerRB.sharedMaterial = core.playerRawData.noFriction;
                else
                    core.playerRB.sharedMaterial = core.playerRawData.lessFriction;

                moveDistance = Mathf.Abs(core.GetCurrentVelocity.x);
                horizontalOnSlope = Mathf.Cos(groundMiddleAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(core.GetCurrentVelocity.x);
                verticalOnSlope = Mathf.Sin(groundMiddleAngle * Mathf.Deg2Rad) * moveDistance;

                if (horizontalOnSlope != 0)
                    core.SetVelocityX(-horizontalOnSlope + (1 + core.CurrentDirection * 1f) , core.GetCurrentVelocity.y);

                if (CheckIfTouchGround && verticalOnSlope != 0)
                    core.SetVelocityY(-verticalOnSlope);
            }
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        //  Ground
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundCheck.position, core.playerRawData.groundCheckRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundUnwalkableCheck.position, core.playerRawData.groundCheckRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundFrontFootCheck.position, core.playerRawData.groundCheckRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundBackFootCheck.position, core.playerRawData.groundCheckRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(playerPlatformHeightCheck.position, Vector2.right *
            core.playerRawData.wallClimbCheckRadius * core.CurrentDirection);

        //  Slope Checker
        Debug.DrawLine(transform.position, transform.position + slopeForward *
            core.playerRawData.slopeCheckDistance, Color.blue);
        Debug.DrawLine(transform.position, (Vector2) transform.position + Vector2.down *
            core.playerRawData.slopeCheckDistance, Color.yellow);

        Debug.DrawLine(groundFronFootSlopCheck.position, (Vector2)groundFronFootSlopCheck.position + 
            Vector2.down * core.playerRawData.slopeFrontFootCheckDistance, Color.red);

        //  Wall Climbing
        Gizmos.color = Color.red;
        Gizmos.DrawRay(wallClimbCheck.position, Vector2.right *
            core.playerRawData.wallClimbCheckRadius * core.CurrentDirection);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(ledgeCheck.position, Vector2.right * core.playerRawData.wallClimbCheckRadius *
            core.CurrentDirection);

        Gizmos.color = Color.green;
        Gizmos.DrawCube(wallCheck.position, core.playerRawData.wallCheckRadius);
    }
}
