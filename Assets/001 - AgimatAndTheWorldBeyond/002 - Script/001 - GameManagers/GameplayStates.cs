using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayStates
{
    public enum GameplayState
    {
        NONE,
        MAINMENU,
        GAMEPLAY
    }
    public event EventHandler gameplayStateChange;
    public event EventHandler onGameplayStateChange
    {
        add
        {
            if (gameplayStateChange == null || gameplayStateChange.GetInvocationList().Contains(value))
                gameplayStateChange += value;
        }
        remove { gameplayStateChange -= value; }
    }
    GameplayState gameplayState;
    public GameplayState CurrentGameplayState
    {
        get => gameplayState;
        set
        {
            gameplayState = value;
            gameplayStateChange?.Invoke(this, EventArgs.Empty);
        }
    }
}
