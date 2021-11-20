using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private MainMenuCore core;     

    private void Awake()
    {
        GameManager.instance.mainMenu.onSSMMStateChange += SSMMStateChange;
        GameManager.instance.mainMenu.onMainMenuStateChange += MainMenuStateChange;

        core.SSMM();
    }

    private void OnDisable()
    {
        GameManager.instance.mainMenu.onSSMMStateChange -= SSMMStateChange;
        GameManager.instance.mainMenu.onMainMenuStateChange -= MainMenuStateChange;
    }

    private void MainMenuStateChange(object sender, EventArgs e)
    {
        Debug.Log(GameManager.instance.mainMenu.CurrentMainMenuState + "   " + core.MainMenuStateToIndexAnim(GameManager.instance.mainMenu.CurrentMainMenuState));
        core.mainMenuAnim.SetInteger("indexState", core.MainMenuStateToIndexAnim(GameManager.instance.mainMenu.CurrentMainMenuState));
    }

    private void SSMMStateChange(object sender, EventArgs e)
    {
        core.SSMM();
    }

    private void Update()
    {
        core.InteractionsSSMM();

    }
}
