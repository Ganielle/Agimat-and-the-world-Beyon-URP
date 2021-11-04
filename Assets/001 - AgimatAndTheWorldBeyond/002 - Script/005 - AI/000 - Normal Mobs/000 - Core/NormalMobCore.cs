using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NormalMobCore : MonoBehaviour
{
    public enum PatrolState
    {
        NONE,
        SEARCH,
        PATROL,
        BATTLE
    }
    private event EventHandler patrolModeChange;
    public event EventHandler onPatrolModeChange
    {
        add
        {
            if (patrolModeChange == null || patrolModeChange.GetInvocationList().Contains(value))
                patrolModeChange += value;
        }
        remove { patrolModeChange -= value; }
    }
    public PatrolState CurrentPatrolState
    {
        get => currentPatrolState;
        set
        {
            currentPatrolState = value;
            patrolModeChange?.Invoke(this, EventArgs.Empty);
        }
    }

    //  ==============================================================

    [Header("Settings")]
    public Rigidbody2D enemyRB;
    public Animator enemyAnim;
    public GameObject envCheckerXRotGO;
    public NormalMobRawData mobRawData;
    public FOVDetection fovDetection;

    [Header("SPRITE")]
    public SpriteRenderer enemySR;

    [Header("DEBUGGER")]
    [ReadOnly] public PatrolState currentPatrolState;
    [ReadOnly] public int currentDirection;
    [ReadOnly] public Vector2 GetCurrentVelocity;
    [ReadOnly] public Vector2 GetWorkspace;

    //  PATROL MODE
    [ReadOnly] public float enterTimePatrolState;
    [ReadOnly] public float chosenTimePatrolState;

    //  DO THE PATROL MODE

    #region PHYSICS

    public void CurrentVelocitySetter() => GetCurrentVelocity = enemyRB.velocity;

    public void SetVelocityZero()
    {
        enemyRB.velocity = Vector2.zero;
        GetCurrentVelocity = Vector2.zero;
    }

    public void SetVelocityX(float velocityX, float velocityY)
    {
        GetWorkspace.Set(velocityX, velocityY);
        enemyRB.velocity = GetWorkspace;
        GetCurrentVelocity = GetWorkspace;
    }

    public void SetVelocityY(float velocity)
    {
        GetWorkspace.Set(GetCurrentVelocity.x, velocity);
        enemyRB.velocity = GetWorkspace;
        GetCurrentVelocity = GetWorkspace;
    }

    #endregion

    #region PATROL STATES

    public void PatrolStateChanger()
    {
        if (!fovDetection.isInFOV)
        {
            if (Time.time >= enterTimePatrolState)
            {
                switch (CurrentPatrolState)
                {
                    case PatrolState.SEARCH:
                        CurrentPatrolState = PatrolState.PATROL;
                        ChangeEnterTimePatrolState(mobRawData.minPatrolTime, mobRawData.maxPatrolTime);
                        return;
                    case PatrolState.PATROL: 
                        CurrentPatrolState = PatrolState.SEARCH;
                        ChangeEnterTimePatrolState(mobRawData.minSearchTime, mobRawData.maxSearchTime);
                        return;
                }
            }
        }
        else
        {
            //  DO ATTACK STATE HERE
        }
    }

    public void ChangeDirection()
    {
        switch (currentDirection)
        {
            case 1: CheckIfShouldFlip(-1); return;
            case -1: CheckIfShouldFlip(1); return;
        }
    }

    public void ChangeEnterTimePatrolState(float min, float max)
    {
        chosenTimePatrolState = UnityEngine.Random.Range(min, max);

        enterTimePatrolState = Time.time + chosenTimePatrolState;
    }

    #endregion

    #region ENEMY SPRITE

    public void CheckIfShouldFlip(int direction)
    {
        if (direction != currentDirection)
            EnemyFlip();
    }

    private void EnemyFlip()
    {
        currentDirection *= -1;

        envCheckerXRotGO.transform.Rotate(0f, 180f, 0f);

        if (currentDirection == 1) enemySR.flipX = false;
        else enemySR.flipX = true;
    }

    public void FlipCheckOnEnable()
    {
        switch (enemySR.flipX)
        {
            case true: currentDirection = -1; return;
            case false: currentDirection = 1; return;
        }
    }

    #endregion
}
