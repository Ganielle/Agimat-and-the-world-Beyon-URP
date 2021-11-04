using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMobStatemachineController : MonoBehaviour
{
    public NormalMobCore core;

    // STATEMACHJINES
    #region STATEMACHINES

    public NormalMobStatemachineChanger normalMobStatemachineChanger;
    public NormalMobSearchState searchState;
    public NormalMobPatrolState patrolState;

    #endregion

    private void Awake()
    {
        normalMobStatemachineChanger = new NormalMobStatemachineChanger();
        searchState = new NormalMobSearchState(this, normalMobStatemachineChanger, core.mobRawData, "searching");
        patrolState = new NormalMobPatrolState(this, normalMobStatemachineChanger, core.mobRawData, "patroling");
    }

    private void Start()
    {
        normalMobStatemachineChanger.Initialize(searchState);
    }

    private void Update()
    {
        core.CurrentVelocitySetter();

        normalMobStatemachineChanger.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        normalMobStatemachineChanger.currentState.PhysicsUpdate();
    }
}
