using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStatePlayerController : MonoBehaviour
{
    [SerializeField] private float battleStateCooldownToAdventure;

    [Header("DEBUGGER")]
    [ReadOnly] public float lastBattleState;

    private void Update()
    {
        BattleToAdventure();
    }

    public void ChangeBattleState()
    {
        GameManager.instance.PlayerStats.GetSetBattleState = PlayerStats.PlayerBattleState.BATTLE;
        lastBattleState = Time.time;
    }

    public void BattleToAdventure()
    {
        if (Time.time >= lastBattleState + battleStateCooldownToAdventure &&
            GameManager.instance.PlayerStats.GetSetBattleState != PlayerStats.PlayerBattleState.ADVENTURING)
            GameManager.instance.PlayerStats.GetSetBattleState = PlayerStats.PlayerBattleState.ADVENTURING;
    }
}
