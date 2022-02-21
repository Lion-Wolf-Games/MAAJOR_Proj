using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private LoadingScreenManager loadingScreenManager;

    private void Awake() {

        if (Instance != null)
        {
            gameObject.SetActive(false);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

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
    
}
