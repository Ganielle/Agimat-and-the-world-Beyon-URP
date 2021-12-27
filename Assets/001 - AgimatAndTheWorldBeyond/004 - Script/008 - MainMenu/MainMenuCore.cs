using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenuCore : MonoBehaviour
{
    [Header("Skip")]
    [SerializeField] private float skipTime;

    [Header("SSMM")]
    public GameObject splashScreenPanelObj;
    public GameObject movieScreenPanelObj;
    public GameObject mainMenuPanelObj;

    [Header("MAINMENU")]
    public Animator mainMenuAnim;
    public GameObject postProcessing;
    public GameObject mainMenuChildObj;
    public GameObject pressStartPanelObj;
    public GameObject saveAndLoadPanelObj;
    public GameObject settingsPanelObj;
    public GameObject quitGamePanelObj;

    [Header("FADE SCREENS")]
    public LeanTweenType easeType;
    public float speedFade;
    public Image blackFadeImage;
    public Image whiteFadeImage;
    public ImageColorLeanTweenAnimation blackFade;
    public ImageColorLeanTweenAnimation whiteFade;

    [Header("MOVIE")]
    public VideoClip movieIntro;
    public VideoPlayer videoPlayer;

    [Header("DEBUGGER")]
    [ReadOnly] public bool canPressStart;
    [ReadOnly] public bool canSkip;
    [ReadOnly] public float currentSkipTime;
    [ReadOnly] public int mainMenuIndex;
    [ReadOnly] public bool canChooseInMM;

    Coroutine lastSSMMCoroutine;

    #region SSMM

    public void SSMM()
    {
        GameObject[] deactivate;

        switch (GameManager.instance.mainMenu.CurrentSSMMState)
        {
            default: 
                splashScreenPanelObj.SetActive(false);
                movieScreenPanelObj.SetActive(false);
                mainMenuPanelObj.SetActive(false);
                postProcessing.SetActive(false);

                GameManager.instance.mainMenu.CurrentSSMMState = MainMenuStates.SSMMState.SPLASHSCREEN;

                return;
            case MainMenuStates.SSMMState.SPLASHSCREEN:
                blackFadeImage.color = new Color(blackFadeImage.color.r, blackFadeImage.color.g, 
                    blackFadeImage.color.b, 1f);

                deactivate = new GameObject[]
                {
                    mainMenuPanelObj,
                    postProcessing
                };

                FadeSSMM(deactivate.ToList(), splashScreenPanelObj, 
                    5f, true, MainMenuStates.SSMMState.MOVIE);

                return;
            case MainMenuStates.SSMMState.MOVIE:

                videoPlayer.SetDirectAudioVolume(0, 1f);

                StartCoroutine(MovieIntroPlayer());

                FadeSSMM(null, movieScreenPanelObj,
                    0f, true, MainMenuStates.SSMMState.NONE);

                return;
            case MainMenuStates.SSMMState.MAINMENU:
                deactivate = new GameObject[]
                {
                    splashScreenPanelObj,
                    movieScreenPanelObj
                };

                canSkip = false;

                FadeSSMM(deactivate.ToList(), mainMenuPanelObj,
                0f, false, MainMenuStates.SSMMState.NONE);

                postProcessing.SetActive(true);

                return;
        }
    }

    public void FadeSSMM(List<GameObject> deactivatedObjects, GameObject activatedObject,
        float delayToNextState, bool isBlackFade, MainMenuStates.SSMMState nextState)
    {
        blackFade.CancelAnimation();
        whiteFade.CancelAnimation();
        if (isBlackFade)
        {
            canSkip = false;
            blackFadeImage.gameObject.SetActive(true);

            blackFade.ImageFadeTweenAnimation(easeType, speedFade, 0f, 1f, () => {
                
                if (activatedObject != null)
                    activatedObject.SetActive(true);

                if (deactivatedObjects != null)
                    foreach (GameObject go in deactivatedObjects)
                        go.SetActive(false);

                blackFade.ImageFadeTweenAnimation(easeType, speedFade, 0.5f, 0f, () =>
                {
                    canSkip = true;
                    lastSSMMCoroutine = StartCoroutine(ToNextSSMMState(delayToNextState, nextState));
                    blackFadeImage.gameObject.SetActive(false);
                });
            });
        }
        else
        {
            canSkip = false;
            whiteFadeImage.gameObject.SetActive(true);

            whiteFade.ImageFadeTweenAnimation(easeType, speedFade, 0f, 1f, () => {

                if (activatedObject != null)
                    activatedObject.SetActive(true);

                if (deactivatedObjects != null)
                    foreach (GameObject go in deactivatedObjects)
                        go.SetActive(false);

                whiteFade.ImageFadeTweenAnimation(easeType, speedFade, 0.5f, 0f, () =>
                {
                    canSkip = true;
                    lastSSMMCoroutine = StartCoroutine(ToNextSSMMState(delayToNextState, nextState));
                    whiteFadeImage.gameObject.SetActive(false);
                });
            });
        }
    }

    IEnumerator ToNextSSMMState(float delay, MainMenuStates.SSMMState nextState)
    {
        yield return new WaitForSeconds(delay);

        if (nextState != MainMenuStates.SSMMState.NONE)
            GameManager.instance.mainMenu.CurrentSSMMState = nextState;
    }

    IEnumerator MovieIntroPlayer()
    {
        videoPlayer.clip = movieIntro;

        yield return new WaitForSeconds(speedFade + 0.5f);

        videoPlayer.Play();

        yield return new WaitForSeconds(1f);

        while (videoPlayer.isPlaying)
            yield return null;

        StartCoroutine("MuteVideoPlayer");
    }

    IEnumerator MuteVideoPlayer()
    {
        GameManager.instance.mainMenu.CurrentSSMMState = MainMenuStates.SSMMState.MAINMENU;
        canSkip = false;

        while (videoPlayer.GetDirectAudioVolume(0) > 0f)
        {
            videoPlayer.SetDirectAudioVolume(0, videoPlayer.GetDirectAudioVolume(0) - Time.deltaTime * speedFade);

            yield return null;
        }

        videoPlayer.Stop();

        GameManager.instance.mainMenu.CurrentMainMenuState = MainMenuStates.MainMenuState.PRESSSTART;
    }

    public void InteractionsSSMM()
    {
        if (!GameManager.instance.mainMenuController.interactInput)
        {
            currentSkipTime = 0f;
            return;
        }

        switch (GameManager.instance.mainMenu.CurrentSSMMState)
        {
            case MainMenuStates.SSMMState.SPLASHSCREEN:
                if (!canSkip)
                    return;

                StopCoroutine(lastSSMMCoroutine);

                GameManager.instance.mainMenu.CurrentSSMMState = MainMenuStates.SSMMState.MOVIE;

                canSkip = false;

                return;
            case MainMenuStates.SSMMState.MOVIE:
                if (!canSkip)
                    return;

                currentSkipTime += Time.deltaTime;

                float totalSkipTime = currentSkipTime / skipTime;

                if (totalSkipTime > 1f)
                {
                    StopCoroutine(lastSSMMCoroutine);

                    GameManager.instance.mainMenu.CurrentSSMMState = MainMenuStates.SSMMState.MAINMENU;

                    currentSkipTime = 0f;

                    StartCoroutine("MuteVideoPlayer");

                    canSkip = false;
                }

                return;
            case MainMenuStates.SSMMState.MAINMENU:
                MainMenuEnterInteractions();
                return;
        }
    }

    #endregion

    #region MAIN MENU

    public int MainMenuStateToIndexAnim(MainMenuStates.MainMenuState state)
    {
        switch (state)
        {
            case MainMenuStates.MainMenuState.NONE: return 0;
            case MainMenuStates.MainMenuState.PRESSSTART: return 1;
            case MainMenuStates.MainMenuState.MAINMENU: return 2;
            default: return 1;
        }
    }

    public void MainMenuEnterInteractions()
    {
        switch (GameManager.instance.mainMenu.CurrentMainMenuState)
        {
            case MainMenuStates.MainMenuState.PRESSSTART:
                if (!canPressStart)
                    return;
                GameManager.instance.mainMenu.CurrentMainMenuState = MainMenuStates.MainMenuState.MAINMENU;
                canPressStart = false;
                return;
            case MainMenuStates.MainMenuState.MAINMENU:
                if (!canChooseInMM)
                    return;
                switch (mainMenuIndex)
                {
                    case 0: 
                        GameManager.instance.mainMenu.CurrentMainMenuState = MainMenuStates.MainMenuState.SAVEANDLOAD;
                        canChooseInMM = false;
                        return;
                    case 1: 
                        GameManager.instance.mainMenu.CurrentMainMenuState = MainMenuStates.MainMenuState.SETTINGS;
                        canChooseInMM = false;
                        return;
                    case 2: 
                        GameManager.instance.mainMenu.CurrentMainMenuState = MainMenuStates.MainMenuState.QUITGAME;
                        canChooseInMM = false;
                        return;
                }
                return;
        }
    }

    #endregion
}
