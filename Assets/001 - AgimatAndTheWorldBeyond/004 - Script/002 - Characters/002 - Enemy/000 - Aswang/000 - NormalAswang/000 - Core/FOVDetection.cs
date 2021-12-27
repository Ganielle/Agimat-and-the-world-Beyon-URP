﻿using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class FOVDetection : MonoBehaviour
{
    [SerializeField] private bool debugMode;
    [SerializeField] private NormalMobCore normalAICore;

    [Header("Settings")]
    public float maxAngle;
    public float maxRadius;
    public Transform characterTF;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [Header("DEBUGGER")]
    [ReadOnly] public Vector3 fovLine1, fovLine2;
    [ReadOnly] public bool isInFOV = false;
    [ReadOnly] public GameObject targetTF;
    [ReadOnly] public int countFOV;
    [ReadOnly] public int countDetector;

    private void OnDrawGizmos()
    {
        if (!debugMode)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(characterTF.position, maxRadius);

        fovLine1 = new Vector2(Mathf.Cos(maxAngle * Mathf.Deg2Rad) * normalAICore.currentDirection, Mathf.Sin(maxAngle * Mathf.Deg2Rad));
        fovLine2 = new Vector2(Mathf.Cos(-maxAngle * Mathf.Deg2Rad) * normalAICore.currentDirection, Mathf.Sin(-maxAngle * Mathf.Deg2Rad));

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(characterTF.position, fovLine1 * maxRadius);
        Gizmos.DrawRay(characterTF.position, fovLine2 * maxRadius);

        if (!isInFOV)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;

        Gizmos.DrawRay(characterTF.position, (GameManager.instance.PlayerStats.GetSetPlayerCharacterObj.transform.position - characterTF.position).normalized
            * maxRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(characterTF.position, transform.right * maxRadius * normalAICore.currentDirection);
    }

    private void FixedUpdate()
    {
        isInFOV = inFOV();
    }

    private bool inFOV()
    {
        Collider2D[] overlaps = new Collider2D[10];
        countFOV = Physics2D.OverlapCircleNonAlloc(transform.position, maxRadius, overlaps, targetMask);

        for (int index = 0; index < countFOV; index++)
        {
            if(overlaps[index].transform != null)
            {
                if (overlaps[index].transform.CompareTag("Player"))
                {
                    targetTF = overlaps[index].gameObject;

                    Vector2 directionBetween = (targetTF.transform.position - transform.position).normalized;

                    float angle = Vector2.Angle(transform.right, directionBetween * normalAICore.currentDirection);

                    if (angle < maxAngle)
                    {
                        if (!Physics2D.Raycast(transform.position, directionBetween, 
                            Vector2.Distance(transform.position, targetTF.transform.position), obstacleMask))
                            return true;
                    }
                }
            }
        }

        targetTF = null;

        return false;
    }

    public void PresenceDetector()
    {
        Collider2D[] overlaps = new Collider2D[10];
        countDetector = Physics2D.OverlapCircleNonAlloc(transform.position, maxRadius, overlaps, targetMask);

        for (int index = 0; index < countDetector; index++)
        {
            if (overlaps[index].transform != null)
            {
                if (overlaps[index].transform.CompareTag("Player"))
                {
                    targetTF = overlaps[index].gameObject;

                    return;
                }
            }
        }
    }
}