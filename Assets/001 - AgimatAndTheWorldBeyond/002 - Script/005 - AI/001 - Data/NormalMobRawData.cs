using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalMobsRawData", menuName = "Agimat and the World Beyond/Enemy/Data/Normal Mob Raw Data")]
public class NormalMobRawData : ScriptableObject
{
    [Header("PHYSICS MATERIAL")]
    public PhysicsMaterial2D lessFriction;
    public PhysicsMaterial2D fullFriction;
    public PhysicsMaterial2D noFriction;
    public float colliderNormalOffsetY;
    public float colliderRopeOffsetY;

    [Header("SEARCH")]
    public float minSearchTime;
    public float maxSearchTime;

    [Header("PATROL")]
    public float minPatrolTime;
    public float maxPatrolTime;
    public float moveSpeed;

    [Header("ALERT")]
    public float alertTimeToChase;

    [Header("CHASE")]
    public float chaseSpeed;
    public float checkDistanceToPlayer;

    [Header("GROUND")]
    public float groundCheckRadius = 0.15f;

    [Header("SLOPE MOVEMENT")]
    public float slopeCheckDistance = 1.9f;
    public float slopeForce = 2.5f;
    public float slopeFrontFootCheckDistance = 2.5f;

}
