using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThundercloudMMBGController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float minWaitTime;
    [SerializeField] private float maxWaitTime;
    [SerializeField] private float exitWaitThunderTime;

    [Header("DEBUGGER")]
    [ReadOnly] [SerializeField] float enterThunderTime;
    [ReadOnly] [SerializeField] private float enterWaitThunderTime;
    [ReadOnly] [SerializeField] bool canThunderbolt;

    private void OnEnable()
    {
        enterThunderTime = Time.time + Random.Range(minWaitTime, maxWaitTime);
    }

    private void Update()
    {
        if (Time.time >= enterThunderTime)
        {
            if (canThunderbolt)
            {
                enterWaitThunderTime = Time.time + exitWaitThunderTime;
                animator.SetTrigger("thunderbolt");
                canThunderbolt = false;
            }
            else
            {
                if (Time.time >= enterWaitThunderTime)
                {
                    animator.ResetTrigger("thunderbolt");
                    enterThunderTime = Time.time + Random.Range(minWaitTime, maxWaitTime);
                }
            }
        }
        else
        {
            canThunderbolt = true;
        }
    }
}
