using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimationEvents : MonoBehaviour
{
    [SerializeField] private MainMenuCore core;

    public void PressStartEnabler(int isActive) => core.pressStartPanelObj.SetActive(GameManager.instance.IntToBool(isActive));

    public void CanPressStartEnabler(int canPress) => core.canPressStart = GameManager.instance.IntToBool(canPress);

    public void CanChooseInMM(int canChoose) => core.canChooseInMM = GameManager.instance.IntToBool(canChoose);

    public void MainMenuPanelEnabler(int isActive) => core.mainMenuChildObj.SetActive(GameManager.instance.IntToBool(isActive));
}
