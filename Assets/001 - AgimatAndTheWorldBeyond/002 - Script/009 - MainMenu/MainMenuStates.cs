using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainMenuStates
{
    public enum SSMMState
    {
        NONE,
        SPLASHSCREEN,
        MOVIE,
        MAINMENU,
    }
    private event EventHandler ssMMStateChange;
    public event EventHandler onSSMMStateChange
    {
        add
        {
            if (ssMMStateChange == null || !ssMMStateChange.GetInvocationList().Contains(value))
                ssMMStateChange += value;
        }
        remove { ssMMStateChange -= value; }
    }
    SSMMState ssMMState;
    public SSMMState CurrentSSMMState
    {
        get => ssMMState;
        set
        {
            ssMMState = value;
            ssMMStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public enum MainMenuState
    {
        NONE,
        PRESSSTART,
        MAINMENU,
        SAVEANDLOAD,
        SETTINGS,
        QUITGAME
    }
    private event EventHandler mainMenuStateChange;
    public event EventHandler onMainMenuStateChange
    {
        add
        {
            if (mainMenuStateChange == null || !mainMenuStateChange.GetInvocationList().Contains(value))
                mainMenuStateChange += value;
        }
        remove { mainMenuStateChange -= value; }
    }
    MainMenuState mainMenuState;
    public MainMenuState CurrentMainMenuState
    {
        get => mainMenuState;
        set
        {
            mainMenuState = value;
            mainMenuStateChange?.Invoke(this, EventArgs.Empty);
        }
    }
}
