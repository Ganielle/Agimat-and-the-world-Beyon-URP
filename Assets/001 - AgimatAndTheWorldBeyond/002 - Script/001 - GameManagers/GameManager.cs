﻿ using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    //  this is for calling the public reference variable in
    //  every script 
    public static GameManager instance;

    //  =======================================

    public GameplayStates gameplayStates = new GameplayStates();
    public MainMenuStates mainMenu = new MainMenuStates();

    //  PLAYER SCRIPS
    public PlayerStats PlayerStats = new PlayerStats();
    public PlayerInventory PlayerInventory = new PlayerInventory();

    //  CAMERA
    public CameraShaker cameraShaker = new CameraShaker();

    //  =======================================

    public bool debugMode;
    [ConditionalField("debugMode")] public bool isLukas;
    [ConditionalField("debugMode")] public bool isOnGameplay;
    [ConditionalField("debugMode")] public bool isOnMainMenu;
    [ConditionalField("isOnMainMenu")] public MainMenuStates.SSMMState mainMenuState;
    [ConditionalField("debugMode")] public bool debugScenes;
    [ConditionalField("debugMode")] [SerializeField] private string firstScene;

    [Header("ScriptReferences")]
    public GameplayInputController gameplayController;
    public MainMenuInputController mainMenuController;
    public SceneDataLoadingController sceneDataLoading;
    public EffectManager effectManager;
    public SoundManager soundManager;

    [Header("Camera")]
    public Camera mainCamera;
    public Camera uiCamera;
    public Camera mouseCamera;
    public CinemachineBrain mainCameraCMBrain;
    [ReadOnly] public CinemachineConfiner gameplayConfiner;

    private void Awake()
    {
        //Time.timeScale = 0.2f;
        instance = this;

        SetReferenceScripts();

        DebugMode();
    }

    private void Update()
    {
        //Debug.Log(PlayerStats.GetSetAnimatorStateInfo);
    }

    private void SetReferenceScripts()
    {
        PlayerStats = new PlayerStats();
        cameraShaker = new CameraShaker();
    }

    #region LOADING DATA SCENES

    IEnumerator LoadData()
    {
        yield return StartCoroutine(sceneDataLoading.DebugInsertWeapons());

        if (debugScenes)
        {
            SceneManager.LoadSceneAsync("CharactersScene", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync(firstScene, LoadSceneMode.Additive);
        }
    }


    #endregion

    public void CoroutineRunner(IEnumerator coroutine) => StartCoroutine(coroutine);

    public bool IntToBool(int value)
    {
        if (value == 1) return true;
        else return false;
    }

    private void DebugMode()
    {
        if (debugMode)
        {
            if (isOnGameplay)
            {
                gameplayStates.CurrentGameplayState = GameplayStates.GameplayState.GAMEPLAY;

                StartCoroutine(LoadData());

                //  Debug stamina
                PlayerStats.GetSetLukasStamina = 100f;
                PlayerStats.GetSetLilyStamina = 100f;

                //  Debug Health
                PlayerStats.GetSetLukasHealth = 100f;
                PlayerStats.GetSetLilyHealth = 100f;

                //  Debug Mana
                PlayerStats.GetSetLukasMana = 100f;
                PlayerStats.GetSetLilyMana = 100f;
            }

            if (isOnMainMenu)
            {
                mainMenu.CurrentSSMMState = mainMenuState;
            }
        }
    }
}
