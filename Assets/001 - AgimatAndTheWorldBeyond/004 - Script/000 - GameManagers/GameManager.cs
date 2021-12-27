 using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.InputSystem;

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

    [Header("DASH")]
    public GameObject dashArrowIndicator;

    [Header("LIGHT")]
    public Light directionalLight;

    [Header("ScriptReferences")]
    public GameplayInputController gameplayController;
    public MainMenuInputController mainMenuController;
    public SceneDataLoadingController sceneDataLoading;
    public EffectManager effectManager;
    public SoundManager soundManager;

    [Header("Input Controller")]
    public PlayerInput playerInput;

    [Header("Camera")]
    public Camera mainCamera;
    public Camera uiCamera;
    public Camera mouseCamera;
    public CinemachineBrain mainCameraCMBrain;
    [ReadOnly] public CinemachineVirtualCamera lastPlayerCamera;
    [ReadOnly] public CinemachineVirtualCamera currentPlayerCamera;

    private void Awake()
    {
        //Time.timeScale = 0.2f;
        instance = this;

        DebugMode();
    }

    private void Update()
    {
        //Debug.Log(PlayerStats.GetSetAnimatorStateInfo);
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
                playerInput.defaultActionMap = "Gameplay";
                playerInput.SwitchCurrentActionMap("Gameplay");

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
                playerInput.defaultActionMap = "MainMenu";
                playerInput.SwitchCurrentActionMap("MainMenu");

                mainMenu.CurrentSSMMState = mainMenuState;

                if (debugScenes)
                    SceneManager.LoadSceneAsync(firstScene, LoadSceneMode.Additive);
            }
        }
    }
}
