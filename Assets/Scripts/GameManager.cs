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

    private void Awake() {

        if (Instance != null)
        {
            gameObject.SetActive(false);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

public void ChangeGameState(GameState newState)
{
    if(newState == Instance.currentGameState) return;

    currentGameState = newState;

    switch (currentGameState)
    {
        case GameState.Playing:
        Instance.OnPlay?.Invoke();
        break;
        case GameState.Paused:
        Instance.OnPause?.Invoke();
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
}
