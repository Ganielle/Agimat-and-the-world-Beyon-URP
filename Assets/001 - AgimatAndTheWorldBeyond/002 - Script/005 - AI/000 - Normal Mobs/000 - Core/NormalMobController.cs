using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobController : MonoBehaviour
{
    [SerializeField] private NormalMobCore core;

    private void OnEnable()
    {
        core.FlipCheckOnEnable();

        core.onPatrolModeChange += PatrolModeChange;
    }

    private void OnDisable()
    {
        core.onPatrolModeChange -= PatrolModeChange;
    }

    private void PatrolModeChange(object sender, EventArgs e)
    {

    }
}
