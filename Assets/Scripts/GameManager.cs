using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public Action OnPlay;
    public Action OnPause;
    public Action<GameState> OnGameStateChanged;
    [SerializeField] private LoadingScreenManager loadingScreenManager;

    public GameState currentGameState;


    // will be moved to another script soon(tm)
    [Space] [Header("Sound parameters loader")]
    public AK.Wwise.RTPC MasterVolume;
    public AK.Wwise.RTPC MusicVolume;
    public AK.Wwise.RTPC SFXVolume;

    private void Awake() {

        if (Instance != null)
        {
            gameObject.SetActive(false);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        float masterV = PlayerPrefs.GetFloat("MasterVolume", 100);
        float musicV = PlayerPrefs.GetFloat("MusicVolume", 100);
        float sfxV = PlayerPrefs.GetFloat("SfxVolume", 100);

        SetVolume(masterV, MasterVolume);
        SetVolume(musicV, MusicVolume);
        SetVolume(sfxV, SFXVolume);
    }

    private void SetVolume(float value, AK.Wwise.RTPC rtpc)
    {
        rtpc.SetGlobalValue(value);
    }

    public void ChangeGameState(GameState newState)
    {
        if(newState == Instance.currentGameState) return;

        currentGameState = newState;

        switch (currentGameState)
        {
            case GameState.Playing:
            Instance.OnPlay?.Invoke();
            Time.timeScale = 1;
            break;
            case GameState.Paused:
            Instance.OnPause?.Invoke();
            Time.timeScale = 0;
            break;
            case GameState.InDialogue:
            Time.timeScale = 1;
            break;
            default:
            break;
        }

        OnGameStateChanged?.Invoke(currentGameState);
    }

#region LevelManagement
    public static void StaticLoadLevel(int index)
    {
        Instance.LoadLevel(index);
    }
    public static void StaticLoadLevel(Scene scene)
    {
        Instance.LoadLevel(scene.buildIndex);
    }

    public static void StaticQuit()
    {
        Instance.Quit();
    }

    private void Quit(){
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private void LoadLevel(int index)
    {
        AsyncOperation levelLoad = SceneManager.LoadSceneAsync(index);
        loadingScreenManager.ShowLoadingScreen(levelLoad);
    }
#endregion    
}

public enum GameState
{
    Playing,
    Paused,
    InDialogue
}
