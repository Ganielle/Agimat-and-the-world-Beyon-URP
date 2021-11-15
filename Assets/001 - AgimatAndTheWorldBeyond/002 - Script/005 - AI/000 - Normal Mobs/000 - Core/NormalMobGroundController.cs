using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobGroundController : MonoBehaviour
{
    [SerializeField] private NormalMobCore mobCore;

    [Header("GROUND")]
    public float maxSlopeAngle;
    public float minimumSlopeAngle;

    [Header("ENVIRONMENT")]
    public LayerMask whatIsGround;
    public LayerMask WhoIsPlayer;
    public Transform groundChecker;
    public Transform frontFootChecker;
    public Transform frontFootSlopeChecker;

    [Header("DEBUGGER ENVIRONMENT")]
    [ReadOnly] public bool canWalkOnSlope;
    [ReadOnly] public Vector3 slopeForward;
    [ReadOnly] public float groundAngle;
    [ReadOnly] public float frontFootGroundAngle;
    [ReadOnly] [SerializeField] float moveDistance;
    [ReadOnly] [SerializeField] float horizontalOnSlope;
    [ReadOnly] [SerializeField] float verticalOnSlope;

    #region PHYSICS

    public void PhysicsMaterialChanger(PhysicsMaterial2D mat) => mobCore.enemyRB.sharedMaterial = mat;

    public bool CheckIfTouchGround
    {
        get => Physics2D.OverlapCircle(groundChecker.position, mobCore.mobRawData.groundCheckRadius,
            whatIsGround);
    }

    public bool CheckIfFrontFootTouchGround
    {
        get => Physics2D.OverlapCircle(frontFootChecker.position, mobCore.mobRawData.groundCheckRadius,
            whatIsGround);
    }

    public bool CheckIfFrontFootTouchSlope
    {
        get => Physics2D.Raycast(frontFootSlopeChecker.position, Vector2.down, mobCore.mobRawData.slopeFrontFootCheckDistance,
            whatIsGround);
    }

    public bool CheckIfPlayerIsInMyBack
    {
        get => Physics2D.Raycast(transform.position, Vector2.left * mobCore.currentDirection, mobCore.fovDetection.maxRadius,
            WhoIsPlayer);
    }

    public void CalculateSlopeForward()
    {
        if (!CheckIfTouchGround)
        {
            slopeForward = transform.forward;
            return;
        }

        slopeForward = Vector3.Cross(Physics2D.Raycast(transform.position, Vector2.down, mobCore.mobRawData.slopeCheckDistance,
            whatIsGround).normal, transform.forward * mobCore.currentDirection);
    }
    public void SlopeChecker()
    {
        //  To prevent slope animation on in air while ground
        if (groundAngle == 90f && frontFootGroundAngle == 90f)
            canWalkOnSlope = false;
        //  On flat surface or walkable slope, on slope but can move
        else if (groundAngle < minimumSlopeAngle && frontFootGroundAngle >= maxSlopeAngle)
            canWalkOnSlope = true;
        //  Higher slope, on slope but cannot move
        else if (groundAngle < maxSlopeAngle)
            canWalkOnSlope = false;
        else
            canWalkOnSlope = false;
    }

    public void CalculateGroundAngle()
    {
        if (!CheckIfTouchGround)
        {
            groundAngle = 90f;
            frontFootGroundAngle = 90f;
            return;
        }

        groundAngle = Vector2.Angle(Physics2D.Raycast(transform.position, Vector2.down, mobCore.mobRawData.slopeCheckDistance,
            whatIsGround).normal, -transform.up);

        frontFootGroundAngle = Vector2.Angle(Physics2D.Raycast(frontFootSlopeChecker.position, Vector2.down, mobCore.mobRawData.slopeFrontFootCheckDistance,
            whatIsGround).normal, -transform.up);

        if (groundAngle <= maxSlopeAngle || groundAngle > minimumSlopeAngle)
            mobCore.enemyRB.sharedMaterial = mobCore.mobRawData.noFriction;

        else if (groundAngle > maxSlopeAngle || groundAngle <= minimumSlopeAngle)
            mobCore.enemyRB.sharedMaterial = mobCore.mobRawData.lessFriction;
    }

    public void SlopeMovement()
    {
        if (CheckIfTouchGround && canWalkOnSlope &&
            CheckIfFrontFootTouchSlope)
        {
            if (groundAngle <= minimumSlopeAngle)
            {
                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0f)
                    mobCore.enemyRB.sharedMaterial = mobCore.mobRawData.noFriction;
                else
                    mobCore.enemyRB.sharedMaterial = mobCore.mobRawData.lessFriction;

                moveDistance = Mathf.Abs(mobCore.GetCurrentVelocity.x);
                horizontalOnSlope = Mathf.Cos(groundAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(mobCore.GetCurrentVelocity.x);
                verticalOnSlope = Mathf.Sin(groundAngle * Mathf.Deg2Rad) * moveDistance;

                if (horizontalOnSlope != 0)
                    mobCore.SetVelocityX(-horizontalOnSlope + (1 + mobCore.currentDirection * 1f), mobCore.GetCurrentVelocity.y);

                if (CheckIfTouchGround && verticalOnSlope != 0)
                    mobCore.SetVelocityY(-verticalOnSlope);
            }
        }
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundChecker.position, mobCore.mobRawData.groundCheckRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(frontFootChecker.position, mobCore.mobRawData.groundCheckRadius);

        //  Slope Checker
        Debug.DrawLine(transform.position, transform.position + slopeForward *
            mobCore.mobRawData.slopeCheckDistance, Color.blue);
        Debug.DrawLine(transform.position, (Vector2)transform.position + Vector2.down *
            mobCore.mobRawData.slopeCheckDistance, Color.yellow);

        Debug.DrawLine(frontFootSlopeChecker.position, (Vector2)frontFootSlopeChecker.position +
            Vector2.down * mobCore.mobRawData.slopeFrontFootCheckDistance, Color.red);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position +
            Vector2.left * mobCore.currentDirection * mobCore.fovDetection.maxRadius);
    }
}
