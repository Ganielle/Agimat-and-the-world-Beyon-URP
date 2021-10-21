using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class FOVDetection : MonoBehaviour
{
    [SerializeField] private bool debugMode;

    [Header("Settings")]
    public float maxAngle;
    public float maxRadius;
    public Transform characterTF;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [Header("DEBUGGER")]
    [ReadOnly] public Vector3 fovLine1, fovLine2;
    [ReadOnly] public bool isInFOV = false;

    private void OnDrawGizmos()
    {
        if (!debugMode)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(characterTF.position, maxRadius);

        fovLine1 = new Vector2(Mathf.Cos(maxAngle * Mathf.Deg2Rad), Mathf.Sin((maxAngle / 2) * Mathf.Deg2Rad));
        fovLine2 = new Vector2(Mathf.Cos(-maxAngle * Mathf.Deg2Rad), Mathf.Sin((-maxAngle / 2) * Mathf.Deg2Rad));

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
        Gizmos.DrawRay(characterTF.position, transform.right * maxRadius);
    }

    private void FixedUpdate()
    {
        isInFOV = inFOV();
    }

    private bool inFOV()
    {
        Collider2D[] overlaps = new Collider2D[10];
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, maxRadius, overlaps, targetMask);

        for (int index = 0; index < count; index++)
        {
            if(overlaps[index].transform != null)
            {
                if (overlaps[index].transform.CompareTag("Player"))
                {
                    Vector2 directionBetween = (overlaps[index].transform.position - transform.position).normalized;
                    //directionBetween.x *= 0;

                    float angle = Vector2.Angle(transform.right, directionBetween);

                    Debug.Log(directionBetween + "    " + angle);

                    if (angle < maxAngle / 2)
                    {
                        if (!Physics2D.Raycast(transform.position, directionBetween, Vector2.Distance(transform.position, overlaps[index].transform.position),
                            obstacleMask))
                            return true;
                    }
                }
            }
        }
        return false;
    }
}
